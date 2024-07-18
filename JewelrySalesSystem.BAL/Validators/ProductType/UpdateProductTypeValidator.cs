using FluentValidation;
using JewelrySalesSystem.BAL.Models.ProductTypes;
using JewelrySalesSystem.DAL.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelrySalesSystem.BAL.Validators.ProductType
{
    public class UpdateProductTypeValidator : AbstractValidator<UpdateTypeRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductTypeValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product type name is required.")
                .Matches("^[a-zA-Z0-9 ]*$")
                .WithMessage("Product type name must not contain special characters.")
                .MustAsync(async (name, cancellation) => !await ProductTypeExists(name))
                .WithMessage("A product type with the same name already exists.");
        }

        private async Task<bool> ProductTypeExists(string name)
        {
            var existingProductType = await _unitOfWork.ProductTypes.CheckDuplicate(name);

            return existingProductType != null;
        }
    }
}
