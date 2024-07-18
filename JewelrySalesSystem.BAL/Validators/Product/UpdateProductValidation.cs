
using FluentValidation;
using JewelrySalesSystem.BAL.Models.Products;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Validators.Product
{
    public class UpdateProductValidation : AbstractValidator<UpdateProductRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductValidation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product Name is required!")
                .Matches(@"^[a-zA-Z0-9 ]+$").WithMessage("Product Name cannot contain special characters!");
                //.MustAsync(async (productName, cancellation) => !await CheckDuplicateAsync(productName)).WithMessage("Product Name already exits!");

            RuleFor(p => p.PercentPriceRate)
                .NotEmpty().WithMessage("Percent Price Rate is required!")
                .InclusiveBetween(0, 100).WithMessage("Percent Price Rate must greater than 0!");

            RuleFor(p => p.ProductionCost)
                .NotEmpty().WithMessage("Production cost is required!")
                .GreaterThan(0).WithMessage("Production cost must greater than 0!");

            RuleFor(p => p.FeaturedImage)
                .NotEmpty().WithMessage("Featured Image is required!");
        }

        private async Task<bool> CheckDuplicateAsync(string productName)
        {
            var existing = await _unitOfWork.Products.CheckDuplicate(productName);
            return existing != null;
        }

    }
}
