using FluentValidation;
using JewelrySalesSystem.BAL.Models.Products;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Validators.Product
{
    public class AddProductValidation : AbstractValidator<CreateProductRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductValidation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product Name is required!")
                .Matches(@"^[a-zA-Z0-9 ]+$").WithMessage("Product Name cannot contain special characters!")
                .MustAsync(async (productName, cancellation) => !await CheckDuplicateAsync(productName)).WithMessage("Product Name already exits!");

            RuleFor(p => p.PercentPriceRate)
                .NotEmpty().WithMessage("Percent Price Rate is required!")
                .InclusiveBetween(0,100).WithMessage("Percent Price Rate must greater than 0!");

            RuleFor(p => p.ProductionCost)
                .NotEmpty().WithMessage("Production cost is required!")
                .GreaterThan(0).WithMessage("Production cost must greater than 0!");

            RuleFor(p => p.FeaturedImage)
                .NotEmpty().WithMessage("Featured Image is required!");


            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("Category ID is required!")
                .MustAsync(async (categoryId, cancellation) => await AlreadyExitId(categoryId, "CategoryId"))
                .WithMessage("Category ID does not exit!");

            //RuleFor(p => p.ProductTypeId)
            //    .NotEmpty().WithMessage("Product Type ID is required!")
            //    .MustAsync(async (productTypeId, cancellation) => await AlreadyExitId(productTypeId, "ProductTypeId"))
            //    .WithMessage("Product Type ID does not exit!");

            RuleFor(p => p.GenderId)
                .NotEmpty().WithMessage("Gender ID is required!")
                .MustAsync(async (genderId, cancellation) => await AlreadyExitId(genderId, "GenderId"))
                .WithMessage("Gender ID does not exit!");          

            RuleFor(p => p.Gems)
                .NotEmpty().WithMessage("Gem is required!");

            RuleFor(p => p.Materials)
                .NotEmpty().WithMessage("Materials is required!");
        }

        private async Task<bool> CheckDuplicateAsync(string productName)
        {
            var existing = await _unitOfWork.Products.CheckDuplicate(productName);
            return existing != null;
        }

        private async Task<bool> AlreadyExitId(int id, string option)
        {
            switch(option.ToLower()) 
            {
                case "categoryid":
                    return await _unitOfWork.Products.CategoryExit(id);
                case "producttypeid":
                    return await _unitOfWork.Products.ProductTypeExit(id);
                case "genderid":
                    return await _unitOfWork.Products.GenderExit(id);               
                default:
                    return false;
            }
        }
    }
}
