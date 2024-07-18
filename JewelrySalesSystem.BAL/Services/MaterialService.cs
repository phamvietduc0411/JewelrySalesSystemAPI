using AutoMapper;
using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Materials;
using JewelrySalesSystem.DAL.Common;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //changes here
        private readonly IValidator<CreateMaterialRequest> _createValidator;

        public MaterialService(
            IUnitOfWork unitOfWork
            , IMapper mapper, IValidator<CreateMaterialRequest> validator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _createValidator = validator;
        }

        public async Task<PaginatedList<GetMaterialResponse>> PaginationAsync(
            string? searchTerm,
            string? sortColumn,
            string? sortOrder,
            bool isActive,
            int page,
            int pageSize)
        => _mapper.Map<PaginatedList<GetMaterialResponse>>(await _unitOfWork.Materials.PaginationAsync(searchTerm, sortColumn, sortOrder, isActive, page, pageSize));

        public async Task<Material> AddAsync(Material material)
        {
            var result = _unitOfWork.Materials.AddEntity(material);

            await _unitOfWork.CompleteAsync();

            return result;
        }

        public async Task UpdateAsync(UpdateMaterialRequest updateMaterialRequest)
        {
            var material = await _unitOfWork.Materials.GetEntityByIdAsync(updateMaterialRequest.MaterialId);
            

            if (material != null)
            {
                var product = await _unitOfWork.Products.GetByNameAsync(material.MaterialName);

                if (product != null)
                {
                    product.ProductName = updateMaterialRequest.MaterialName;
                    _unitOfWork.Products.UpdateEntity(product);
                }
                
                material.MaterialName = updateMaterialRequest.MaterialName;              
                _unitOfWork.Materials.UpdateEntity(material);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<GetMaterialResponse?> GetByIdWithIncludeAsync(int id) => _mapper.Map<GetMaterialResponse>(await _unitOfWork.Materials.GetByIdWithIncludeAsync(id));

        public async Task<CreateMaterialRequest> AddAsync(CreateMaterialRequest createMaterialRequest)
        {
            //changes here
            var validation = await _createValidator.ValidateAsync(createMaterialRequest);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.Errors);
            }

            var material = new Material
            {
                MaterialName = createMaterialRequest.MaterialName,
                MaterialPrices = new List<MaterialPriceList>
                {
                    new MaterialPriceList
                    {
                        SellPrice = createMaterialRequest.MaterialPrice.SellPrice,
                        BuyPrice = createMaterialRequest.MaterialPrice.BuyPrice
                    }
                }
            };

            var product = new Product
            {
                ProductName = createMaterialRequest.MaterialName,
                ProductPrice = createMaterialRequest.MaterialPrice.SellPrice,
                ProductTypeId = 2,
                UnitId = 2,
                Quantity = 50,
                CounterId = 1
            };

            var produtResult = _unitOfWork.Products.AddEntity(product);

            var materialResult = _unitOfWork.Materials.AddEntity(material);

            await _unitOfWork.CompleteAsync();

            return createMaterialRequest;
        }

        public async Task<GetMaterialResponse?> GetByIdAsync(int id) => _mapper.Map<GetMaterialResponse>(await _unitOfWork.Materials.GetEntityByIdAsync(id));

        public async Task<List<GetMaterialResponse>> GetAllGoldMaterials() => _mapper.Map<List<GetMaterialResponse>>(await _unitOfWork.Materials.GetAllGoldMaterials());

        public async Task DeleteAsync(int id)
        {
            var result = await _unitOfWork.Materials.GetEntityByIdAsync(id);

            if (result != null)
            {
                if (result.IsActive)
                {
                    result.IsActive = false;
                }
                else
                {
                    result.IsActive = true;
                }
                _unitOfWork.Materials.UpdateEntity(result);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
