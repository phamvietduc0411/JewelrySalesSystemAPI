using FluentValidation;
using JewelrySalesSystem.BAL.Models.Users;
using JewelrySalesSystem.DAL.Entities;
using JewelrySalesSystem.DAL.Infrastructures;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelrySalesSystem.BAL.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(user => user.UserName)
                .NotEmpty()
                .WithMessage("User name is required.")
                .Matches("^[a-zA-Z0-9]+$")
                .Length(2, 50)
                .MustAsync(async (userName, cancellation) => !await CheckDuplicateAsync(userName, "userName"))
                .WithMessage("UserName with the same name already exists.");

            RuleFor(user => user.FullName)
                .NotEmpty().Length(2, 50)
                .Matches("^[a-zA-Z ]+$")
                .WithMessage("Full name cannot contain special characters.");

            RuleFor(user => user.PhoneNumber).NotEmpty()
                .Length(10, 10)
                .Matches(@"\d{10}$")
                .WithMessage("Phone number must be a valid 10-digit number.")
                .MustAsync(async (phoneNumber, cancellation) => !await CheckDuplicateAsync(phoneNumber, "phoneNumber"))
                .WithMessage("PhoneNumber with the same name already exists.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email must be a valid email address.")
                .MustAsync(async (email, cancellation) => !await CheckDuplicateAsync(email, "email"))
                .WithMessage("Email with the same name already exists.");


            RuleFor(user => user.Password)
                .NotEmpty();

            RuleFor(user => user.Address)
                .Matches("^[a-zA-Z0-9/ ]+$")
                .WithMessage("Address cannot contain special characters.");

            RuleFor(user => user.RoleId)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Role ID must be greater than 0.")
                .MustAsync(async (roleId, cancellation) => await CheckRoleId(roleId))
                .WithMessage("Role ID does not exist .");

        }

        private async Task<bool> CheckDuplicateAsync(string details, string options)
        {
            var existing = await _unitOfWork.Users.CheckDuplicate(details, options);
            return existing != null;
        }

        private async Task<bool> CheckRoleId(int id)
        {
            return await _unitOfWork.Users.CheckRoleIdExists(id);
        }
    }
}
