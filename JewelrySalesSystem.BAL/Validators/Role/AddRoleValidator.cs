using FluentValidation;
using JewelrySalesSystem.BAL.Models.Roles;
using JewelrySalesSystem.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelrySalesSystem.BAL.Validators.Role
{
    public class AddRoleValidator : AbstractValidator<CreateRoleRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddRoleValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(r => r.RoleName)
                .NotEmpty()
                .WithMessage("Role is required")
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("Role cannot contain special characters.")
                .MustAsync(async (name, cancellation) => !await CheckDuplicate(name))
                .WithMessage("Role already exists ."); 

        }

        private async Task<bool> CheckDuplicate(string name)
        {
            var existing = await _unitOfWork.Roles.CheckDuplicate(name);
            return existing != null;
        }
    }
}
