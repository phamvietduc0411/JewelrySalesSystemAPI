using FluentValidation;
using JewelrySalesSystem.BAL.Models.Materials;

namespace JewelrySalesSystem.BAL.Validators.Materials
{
    public class MaterialPriceValidation : AbstractValidator<MaterialPrice>
    {
        public MaterialPriceValidation()
        {
            RuleFor(price => price.BuyPrice)
                .NotEmpty().WithMessage("Buy price is required.")
                .GreaterThan(0).WithMessage("Buy price must be greater than 0!");

            RuleFor(price => price.SellPrice)
                .NotEmpty().WithMessage("Sell price is required.")
                .GreaterThan(0).WithMessage("Sell price must be greater than 0!")
                .GreaterThan(price => price.BuyPrice).WithMessage("Sell price must be greater than buy price.");
        }
    }
}
