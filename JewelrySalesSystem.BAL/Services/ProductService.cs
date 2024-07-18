using AutoMapper;
using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Gems;
using JewelrySalesSystem.BAL.Models.Materials;
using JewelrySalesSystem.BAL.Models.Products;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //changes here
        private readonly IValidator<CreateProductRequest> _addProductValidator;
        private readonly IValidator<UpdateProductRequest> _updateProductValidator;

        public ProductService(
            IUnitOfWork unitOfWork
            , IMapper mapper, IValidator<CreateProductRequest> validator, IValidator<UpdateProductRequest> updateProductValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _addProductValidator = validator;
            _updateProductValidator = updateProductValidator;
        }

        public async Task<PaginatedList<GetProductResponse>> ProductPaginationAsync(
            int productTypeId,
            int? counterId,
            int? categoryId,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page,
            int pageSize)
        {
            var result = _mapper.Map<PaginatedList<GetProductResponse>>(await _unitOfWork.Products.JewelryPaginationAsync(productTypeId, counterId, categoryId, searchTerm, sortColumn, sortOrder, isActive, page, pageSize));

            foreach (var item in result.Items)
            {
                foreach (var gem in item.Gems)
                {
                    var tempGem = _mapper.Map<GetGemResponse>(await _unitOfWork.Gems.GetByIdWithIncludeAsync(gem.GemId));

                    if (tempGem != null)
                    {
                        gem.Origin = tempGem.Origin;
                        gem.Carat = tempGem.Carat;
                        gem.Shape = tempGem.Shape;
                        gem.Cut = tempGem.Cut;
                        gem.Color = tempGem.Color;
                        gem.Clarity = tempGem.Clarity;
                    }

                    float price = 0;

                    var temp = await _unitOfWork.Gems.GetEntityByIdAsync(gem.GemId);

                    if (temp != null)
                    {
                        price = await _unitOfWork.Gems.GetGemPriceAsync(temp);

                        float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(temp.ShapeId);

                        price = price * (1 + shapePriceRate / 100);
                    }

                    gem.Price = price;
                }

                foreach (var material in item.Materials)
                {
                    material.Weight = await _unitOfWork.Products.GetWeightAsync(item.ProductId, material.MaterialId) / 100;
                }
            }

            return result;
        }

        public async Task<PaginatedList<GetGemProductResponse>> GemPaginationAsync(
            int productTypeId,
            int? counterId,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page,
            int pageSize)
        {
            var result = _mapper.Map<PaginatedList<GetGemProductResponse>>(await _unitOfWork.Products.PaginationAsync(productTypeId, counterId, searchTerm, sortColumn, sortOrder, isActive, page, pageSize));

            foreach (var item in result.Items)
            {
                var gem = await _unitOfWork.Gems.GetByNameWithIncludeAsync(item.ProductName);

                if (gem != null)
                {
                    item.Origin = gem.Origin.Name;
                    item.Carat = gem.Carat.Weight;
                    item.Shape = gem.Shape.Name;
                    item.Cut = gem.Cut.Level;
                    item.Color = gem.Color.Name;
                    item.Clarity = gem.Clarity.Level;
                }
            }
            return result;
        }

        public async Task<PaginatedList<GetMaterialProductResponse>> MaterialPaginationAsync(
            int productTypeId,
            int? counterId,
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page,
            int pageSize)
        {
            var result = _mapper.Map<PaginatedList<GetMaterialProductResponse>>(await _unitOfWork.Products.PaginationAsync(productTypeId, counterId, searchTerm, sortColumn, sortOrder, isActive, page, pageSize));

            foreach (var item in result.Items)
            {
                var material = await _unitOfWork.Materials.GetByNameWithIncludeAsync(item.ProductName);

                if (material != null)
                {
                    item.MaterialPrice = _mapper.Map<MaterialPrice>(material.MaterialPrices.SingleOrDefault());
                }
            }
            return result;
        }

        public async Task<CreateProductRequest> AddAsync(CreateProductRequest createProductRequest)
        {
            //change here
            var validation = await _addProductValidator.ValidateAsync(createProductRequest);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            var productGems = new List<ProductGem>();

            if (createProductRequest.Gems.Count > 0)
            {
                foreach (var item in createProductRequest.Gems)
                {
                    productGems.Add(new ProductGem
                    {
                        GemId = item
                    });
                }
            }

            var productMaterials = new List<ProductMaterial>();

            if (createProductRequest.Materials.Count > 0)
            {
                foreach (var item in createProductRequest.Materials)
                {
                    productMaterials.Add(new ProductMaterial
                    {
                        MaterialId = item.MaterialId,
                        Weight = item.Weight * 100
                    });
                }
            }

            var product = new Product()
            {
                ProductName = createProductRequest.ProductName,
                PercentPriceRate = createProductRequest.PercentPriceRate,
                ProductionCost = createProductRequest.ProductionCost,
                FeaturedImage = createProductRequest.FeaturedImage,
                CategoryId = createProductRequest.CategoryId,
                Quantity = createProductRequest.Quantity,
                UnitId = 1,
                CounterId = createProductRequest.CounterId,
                ProductTypeId = 3,
                GenderId = createProductRequest.GenderId,
                ProductPrice = await CalculateProductPrice(createProductRequest),
                ProductGems = productGems,
                ProductMaterials = productMaterials
            };

            var result = _unitOfWork.Products.AddEntity(product);

            await _unitOfWork.CompleteAsync();

            return createProductRequest;
        }

        public async Task UpdateAsync(UpdateProductRequest updateProductRequest)
        {
            //changes here
            var validation = await _updateProductValidator.ValidateAsync(updateProductRequest);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            var productGems = new List<ProductGem>();
            if (updateProductRequest.Gems.Count > 0)
            {
                foreach (var item in updateProductRequest.Gems)
                {
                    productGems.Add(new ProductGem
                    {
                        GemId = item
                    });
                }
            }

            var productMaterials = new List<ProductMaterial>();
            if (updateProductRequest.Materials.Count > 0)
            {
                foreach (var item in updateProductRequest.Materials)
                {
                    productMaterials.Add(new ProductMaterial
                    {
                        MaterialId = item.MaterialId,
                        Weight = item.Weight * 100
                    });
                }
            }

            var product = new Product()
            {
                ProductId = updateProductRequest.ProductId,
                ProductName = updateProductRequest.ProductName,
                PercentPriceRate = updateProductRequest.PercentPriceRate,
                ProductionCost = updateProductRequest.ProductionCost,
                FeaturedImage = updateProductRequest.FeaturedImage,
                Quantity = updateProductRequest.Quantity,
                CounterId = updateProductRequest.CounterId,
                CategoryId = updateProductRequest.CategoryId,
                ProductTypeId = 3,
                GenderId = updateProductRequest.GenderId,
                ProductGems = productGems,
                ProductMaterials = productMaterials
            };


            await _unitOfWork.Products.UpdateProduct(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GetProductResponse?> GetByIdAsync(int id) => _mapper.Map<GetProductResponse>(await _unitOfWork.Products.GetByIdWithIncludeAsync(id));

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Products.DeleteProduct(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GetProductResponse?> GetByIdWithIncludeAsync(int id)
        {
            var result = _mapper.Map<GetProductResponse>(await _unitOfWork.Products.GetByIdWithIncludeAsync(id));

            foreach (var item in result.Gems)
            {
                float price = 0;

                var gem = await _unitOfWork.Gems.GetEntityByIdAsync(item.GemId);

                if (gem != null)
                {
                    price = await _unitOfWork.Gems.GetGemPriceAsync(gem);

                    float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(gem.ShapeId);

                    price = price * (1 + shapePriceRate / 100);
                }

                item.Price = price;
            }

            return result;
        }

        private async Task<float> CalculateProductPrice(CreateProductRequest productResponse)
        {
            float productPrice = 0;

            productPrice += productResponse.ProductionCost;

            if (productResponse.Gems.Count > 0)
            {
                foreach (var item in productResponse.Gems)
                {
                    var gem = await _unitOfWork.Gems.GetEntityByIdAsync(item);

                    if (gem != null)
                    {
                        float price = await _unitOfWork.Gems.GetGemPriceAsync(gem);

                        float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(gem.ShapeId);

                        productPrice += price * (1 + shapePriceRate / 100);
                    }
                }
            }

            if (productResponse.Materials.Count > 0)
            {
                foreach (var item in productResponse.Materials)
                {
                    var temp = await _unitOfWork.Materials.GetByIdWithIncludeAsync(item.MaterialId);

                    if (temp != null)
                    {
                        var materialPrice = temp.MaterialPrices.SingleOrDefault();

                        if (materialPrice != null) productPrice += ((item.Weight / 100) * materialPrice.SellPrice) * 375 / 100;
                    }
                }
            }
            productPrice += (productPrice * (productResponse.PercentPriceRate) / 100);

            return productPrice;
        }

        public async Task UpdateProductPriceAsync()
        {
            var products = await _unitOfWork.Products.GetJewelryAndMaterialProducts();

            foreach (var item in products)
            {
                if (item.ProductTypeId == 2)
                {
                    var material = await _unitOfWork.Materials.GetByNameWithIncludeAsync(item.ProductName);

                    if (material != null)
                    {
                        var price = await _unitOfWork.MaterialPrices.GetNewestMaterialPriceByMaterialIdAsync(material.MaterialId);

                        item.ProductPrice = price.SellPrice;
                    }
                }
                else
                {
                    float productPrice = 0;

                    productPrice += item.ProductionCost;

                    if (item.ProductGems.Count > 0)
                    {
                        foreach (var temp in item.ProductGems)
                        {
                            var gem = await _unitOfWork.Gems.GetEntityByIdAsync(temp.GemId);

                            if (gem != null)
                            {
                                float price = await _unitOfWork.Gems.GetGemPriceAsync(gem);

                                float shapePriceRate = await _unitOfWork.Gems.GetShapePriceRateAsync(gem.ShapeId);

                                productPrice += price * (1 + shapePriceRate / 100);
                            }
                        }
                    }

                    if (item.ProductMaterials.Count > 0)
                    {
                        foreach (var temp in item.ProductMaterials)
                        {
                            var material = await _unitOfWork.Materials.GetByIdWithIncludeAsync(temp.MaterialId);

                            if (material != null)
                            {
                                var materialPrice = material.MaterialPrices.SingleOrDefault();

                                if (materialPrice != null) productPrice += ((temp.Weight / 100) * materialPrice.SellPrice) * 375 / 100;
                            }
                        }
                    }
                    productPrice += (productPrice * (item.PercentPriceRate) / 100);

                    item.ProductPrice = productPrice;
                }
            }

            _unitOfWork.Products.UpdateAllProducts(products);

            await _unitOfWork.CompleteAsync();
        }
    }
}
