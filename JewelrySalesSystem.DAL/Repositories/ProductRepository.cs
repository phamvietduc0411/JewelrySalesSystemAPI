using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;
using JewelrySalesSystem.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace JewelrySalesSystem.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(
            JewelryDbContext context,
            ILogger logger) : base(context, logger)
        {
        }

        public async Task<PaginatedList<Product>> PaginationAsync
            (int productTypeId
            , int? counterId
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize)
        {
            IQueryable<Product> productsQuery = _dbSet
                                       .Where(p => p.ProductTypeId == productTypeId)
                                       .Include(p => p.ProductType)
                                       .Include(p => p.Counter)
                                       .Include(p => p.Unit);

            if (isActive) productsQuery = productsQuery.Where(p => p.IsActive);

            if (counterId != null) productsQuery = productsQuery.Where(p => p.CounterId == counterId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                productsQuery = productsQuery.Where(c =>
                    c.ProductName.Contains(searchTerm));
            }

            if (sortOrder?.ToLower() == "asc")
            {
                productsQuery = productsQuery.OrderBy(GetSortProperty(sortColumn));
            }
            else
            {
                productsQuery = productsQuery.OrderByDescending(GetSortProperty(sortColumn));
            }

            var products = await PaginatedList<Product>.CreateAsync(productsQuery, page, pageSize);

            return products;
        }

        public async Task<PaginatedList<Product>> JewelryPaginationAsync
            (int productTypeId
            , int? counterId
            , int? categoryId
            , string? searchTerm
            , string? sortColumn
            , string? sortOrder
            , bool isActive
            , int page
            , int pageSize)
        {
            IQueryable<Product> productsQuery = _dbSet
                                                .Where(p => p.ProductTypeId == productTypeId)
                                                .Include(p => p.ProductGems)
                                                    .ThenInclude(g => g.Gem)
                                                .Include(p => p.ProductMaterials)
                                                    .ThenInclude(m => m.Material)
                                                        .ThenInclude(g => g.MaterialPrices
                                                        .OrderByDescending(g => g.EffDate)
                                                        .Take(1))
                                                .Include(p => p.Category)
                                                .Include(p => p.ProductType)
                                                .Include(p => p.Gender)
                                                .Include(p => p.Counter)
                                                .Include(p => p.Unit);

            if (isActive) productsQuery = productsQuery.Where(p => p.IsActive);

            if (counterId != null) productsQuery = productsQuery.Where(p => p.CounterId == counterId);

            if (categoryId != null) productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                productsQuery = productsQuery.Where(c =>
                    c.ProductName.Contains(searchTerm));
            }

            if (sortOrder?.ToLower() == "asc")
            {
                productsQuery = productsQuery.OrderBy(GetSortProperty(sortColumn));
            }
            else
            {
                productsQuery = productsQuery.OrderByDescending(GetSortProperty(sortColumn));
            }

            var products = await PaginatedList<Product>.CreateAsync(productsQuery, page, pageSize);

            return products;
        }

        private static Expression<Func<Product, object>> GetSortProperty(string? sortColumn)
        => sortColumn?.ToLower() switch
        {
            "price" => product => product.ProductPrice,
            //"dob" => product => product.DoB,
            _ => product => product.ProductId
        };

        public async Task<Product?> GetByIdWithIncludeAsync(int id)
        {
            var result = await _dbSet.Include(p => p.ProductGems)
                                    .ThenInclude(g => g.Gem)
                               .Include(p => p.ProductMaterials)
                                    .ThenInclude(m => m.Material)
                                        .ThenInclude(g => g.MaterialPrices
                                        .OrderByDescending(g => g.EffDate)
                                        .Take(1))
                               .Include(p => p.Category)
                               .Include(p => p.ProductType)
                               .Include(p => p.Gender)
                               .Include(p => p.Counter)
                               .Include(p => p.Unit)
                               .FirstOrDefaultAsync(p => p.ProductId == id);

            if (result == null) return null;

            return result;
        }

        /*Delete Product*/
        //Change status of product = false
        public async Task DeleteProduct(int id)
        {
            var checkExistProduct = await _dbSet.FindAsync(id) ?? throw new Exception($"Product with {id} not found");
            if (checkExistProduct.IsActive)
            {
                checkExistProduct.IsActive = false;
            }
            else
            {
                checkExistProduct.IsActive = true;
            }
            _dbSet.Update(checkExistProduct);
        }

        public async Task UpdateProduct(Product product)
        {
            //Check exist
            //var checkExistProductTask = _dbSet.FindAsync(product.ProductId);
            var checkExistProduct = await _dbSet.FindAsync(product.ProductId);
            if (checkExistProduct != null)
            {
                //Update data
                checkExistProduct.ProductName = product.ProductName;
                checkExistProduct.PercentPriceRate = product.PercentPriceRate;
                checkExistProduct.ProductionCost = product.ProductionCost;
                checkExistProduct.IsActive = product.IsActive;
                checkExistProduct.FeaturedImage = product.FeaturedImage;
                checkExistProduct.CategoryId = product.CategoryId;
                checkExistProduct.ProductTypeId = product.ProductTypeId;
                checkExistProduct.GenderId = product.GenderId;
                checkExistProduct.Counter = product.Counter;


                var existProductGems = await _context.ProductGems.Where(pe => pe.ProductId == product.ProductId).ToListAsync();

                var existingProductGemsDict = existProductGems.ToDictionary(pg => pg.GemId, pg => pg);


                var newProductGems = product.ProductGems.Where(g => !existingProductGemsDict.ContainsKey(g.GemId))
                                                        .Select(g => new ProductGem
                                                        {
                                                            GemId = g.GemId,
                                                            ProductId = product.ProductId
                                                        }).ToList();
                var removedProductGems = existingProductGemsDict.Values.Where(eg => !product.ProductGems.Any(g => g.GemId == eg.GemId)).ToList();
                _context.ProductGems.RemoveRange(removedProductGems);
                _context.ProductGems.AddRange(newProductGems);


                var existProductMaterials = await _context.ProductMaterials.Where(ma => ma.ProductId == product.ProductId).ToListAsync();
                var existingProductMaterialsDict = existProductMaterials.ToDictionary(pm => pm.MaterialId, pm => pm);
                var newProductMaterials = product.ProductMaterials.Where(m => !existingProductMaterialsDict.ContainsKey(m.MaterialId))
                          .Select(m => new ProductMaterial
                          {
                              MaterialId = m.MaterialId,
                              ProductId = product.ProductId,
                              Weight = m.Weight,
                          });
                //var removedProductMaterials = product.ProductMaterials.Where(m => !existingProductMaterialsDict.ContainsKey(m.MaterialId, m.Weight));
                var updatedProductMaterials = product.ProductMaterials.Where(m => existingProductMaterialsDict.ContainsKey(m.MaterialId))
                              .Select(m => new ProductMaterial
                              {
                                  Id = existingProductMaterialsDict[m.MaterialId].Id,
                                  MaterialId = m.MaterialId,
                                  ProductId = product.ProductId,
                                  Weight = m.Weight,
                              });
                var removedProductMaterials = existingProductMaterialsDict.Values.Where(em => !product.ProductMaterials.Any(m => m.MaterialId == em.MaterialId)).ToList();
                _context.ProductMaterials.RemoveRange(removedProductMaterials);
                _context.ProductMaterials.UpdateRange(updatedProductMaterials);
                _context.ProductMaterials.AddRange(newProductMaterials);
                



            }
            else
            {
                throw new Exception($"Product with ID = {product.ProductId} not found!");
            }
            _dbSet.Update(checkExistProduct);
        }

        //changes here
        public async Task<Product> CheckDuplicate(string productName) => await _dbSet.FirstOrDefaultAsync(p => p.ProductName == productName);

        public async Task<bool> CategoryExit(int categoryId) => await _dbSet.AnyAsync(p => p.CategoryId == categoryId);

        public async Task<bool> ProductTypeExit(int productTypeId) => await _dbSet.AnyAsync(p => p.ProductTypeId == productTypeId);

        public async Task<bool> GenderExit(int genderId) => await _dbSet.AnyAsync(p => p.GenderId == genderId);

        public async Task<float> GetWeightAsync(int productId, int materialId) => await _context.ProductMaterials.Where(p => p.ProductId == productId
                                            && p.MaterialId == materialId)
                                            .Select(p => p.Weight)
                                            .FirstOrDefaultAsync();

        public async Task<List<Product>> GetJewelryAndMaterialProducts()
            => await _dbSet.Where(p => p.ProductTypeId != 4 && p.IsActive)
                           .Include(p => p.ProductGems)
                           .Include(p => p.ProductMaterials)
                           .ToListAsync();

        public void UpdateAllProducts(List<Product> products) => _dbSet.UpdateRange(products);

        public async Task<Product?> GetByNameAsync(string name) => await _dbSet.FirstOrDefaultAsync(p => p.ProductName.Equals(name));
    }
}
