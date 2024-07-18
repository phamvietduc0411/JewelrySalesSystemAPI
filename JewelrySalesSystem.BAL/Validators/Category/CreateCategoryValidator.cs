using FluentValidation;
using JewelrySalesSystem.BAL.Models.Categories;
using JewelrySalesSystem.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelrySalesSystem.BAL.Validators.Category
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCategoryValidator(IUnitOfWork unitOfWork ) {
            _unitOfWork = unitOfWork;
            RuleFor(c => c.CategoryName)
                 .NotEmpty()
                 .WithMessage("Category name is required.")
                 .Matches("^[a-zA-Z ]+$")
                 .WithMessage("Category name cannot contain special characters.")
                 .MustAsync(async (name, cancellation) => !await CheckDuplicate(name))
                 .WithMessage("Category name already exists.");
        }

        private async Task<bool> CheckDuplicate(string categoryName)
        {
            var checkResult = await _unitOfWork.Categories.CheckDuplicate(categoryName);
            return checkResult != null;
        }
    }

}

