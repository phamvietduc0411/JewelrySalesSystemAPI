using AutoMapper;
using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Counters;
using JewelrySalesSystem.BAL.Models.Roles;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRoleRequest> _createValidator;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateRoleRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = validator;
        }

        public async Task<CreateRoleRequest> AddRoleAsync(CreateRoleRequest createRoleRequest)
        {
            var validationResult = await _createValidator.ValidateAsync(createRoleRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var result = _unitOfWork.Roles.AddEntity(_mapper.Map<Role>(createRoleRequest));

            await _unitOfWork.CompleteAsync();

            return createRoleRequest;
        }

        public async Task<List<GetRoleResponse>> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllEntitiesAsync();
            return _mapper.Map<List<GetRoleResponse>>(roles);
        }

    }
}
