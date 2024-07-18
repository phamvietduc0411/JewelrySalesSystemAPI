using AutoMapper;
using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.ProductTypes;
using JewelrySalesSystem.BAL.Models.Roles;
using JewelrySalesSystem.BAL.Models.Users;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProductTypeRequest> _createValidator;
        private readonly IValidator<UpdateTypeRequest> _updateValidator;


        public ProductTypeService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateProductTypeRequest> createValidator, IValidator<UpdateTypeRequest> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        public async Task<ProductTypeIdCollectionResponse?> GetAllProductsByProductTypeIdAsync(int productTypeId)
            => _mapper.Map<ProductTypeIdCollectionResponse>(await _unitOfWork.ProductTypes.GetAllProductsByProductTypeIdAsync(productTypeId));


        public async Task<List<GetProductTypeResponse>> GetAllAsync() => _mapper.Map<List<GetProductTypeResponse>>(await _unitOfWork.ProductTypes.GetAllAsync());

        public async Task<GetProductTypeResponse?> GetByIdAsync(int id) => _mapper.Map<GetProductTypeResponse>(await _unitOfWork.ProductTypes.GetEntityByIdAsync(id));

        public async Task<CreateProductTypeRequest> AddAsync(CreateProductTypeRequest productType)
        {
            var validateResult = await _createValidator.ValidateAsync(productType);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }

            var result = _unitOfWork.ProductTypes.AddEntity(_mapper.Map<ProductType>(productType));
            await _unitOfWork.CompleteAsync();

            return productType;
        }

        public async Task<UpdateTypeRequest> UpdateAsync(UpdateTypeRequest productType)
        {
            var validateResult = await _updateValidator.ValidateAsync(productType);
            if (!validateResult.IsValid)
            {
                throw new ValidationException(validateResult.Errors);
            }
            _unitOfWork.ProductTypes.UpdateEntity(_mapper.Map<ProductType>(productType));
            await _unitOfWork.CompleteAsync();

            return productType;
        }
    }
}
