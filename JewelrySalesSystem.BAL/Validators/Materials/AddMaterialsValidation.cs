using FluentValidation;
using JewelrySalesSystem.BAL.Models.Materials;
using JewelrySalesSystem.DAL.Infrastructures;

namespace JewelrySalesSystem.BAL.Validators.Materials
{
    public class AddMaterialsValidation : AbstractValidator<CreateMaterialRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddMaterialsValidation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(m => m.MaterialName)
                .NotEmpty().WithMessage("Materials name is required!")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Name cannot contain special characters!")
                .MustAsync(async (materialName, cancellation) => !await CheckDuplicateAsync(materialName))
                .WithMessage("Materials name already exit!");

            RuleFor(m => m.MaterialPrice)
                .NotEmpty().WithMessage("Material price must be provided!")
                .SetValidator(new MaterialPriceValidation());
        }

        private async Task<bool> CheckDuplicateAsync(string materialName)
        {
            var existing = await _unitOfWork.Materials.CheckDuplicate(materialName);
            return existing != null;
        }
    }
}
