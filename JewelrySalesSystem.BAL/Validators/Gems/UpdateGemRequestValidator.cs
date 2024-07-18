using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Gems;
using JewelrySalesSystem.DAL.Infrastructures;
using JewelrySalesSystem.DAL.Interfaces;

namespace JewelrySalesSystem.BAL.Validators.Gems
{
    public class UpdateGemRequestValidator : AbstractValidator<UpdateGemRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateGemRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(g => g.GemId)
                .NotEmpty()
                .WithMessage("GemName is required")
                .MustAsync(async (GemId, cancellation) => await CheckId(GemId, "GemId"))
                .WithMessage("Id not found");
                

            RuleFor(g => g.GemName)
                .NotEmpty()
                .WithMessage("GemName is required")
                .Matches("^[a-zA-Z0-9 ]+$")
                .WithMessage("GemName cannot contain special characters.");

            RuleFor(g => g.FeaturedImage)
                .NotEmpty()
                .WithMessage("FeatureImage is required");


            RuleFor(g => g.CaratId)
                .NotEmpty()
                .WithMessage("CaratId is required")
                 .MustAsync(async (caratId, cancellation) => await CheckId(caratId, "CaratId"))
                 .WithMessage("CaratId does not exist.");

            RuleFor(g => g.ClarityId)
               .NotEmpty()
               .WithMessage("ClarityId is required")
                 .MustAsync(async (ClarityId, cancellation) => await CheckId(ClarityId, "ClarityId"))
                 .WithMessage("ClarityId does not exist.");

            RuleFor(g => g.ColorId)
               .NotEmpty()
               .WithMessage("ColorId is required")
                 .MustAsync(async (ColorId, cancellation) => await CheckId(ColorId, "ColorId"))
                 .WithMessage("ColorId does not exist.");

            RuleFor(g => g.CutId)
               .NotEmpty()
               .WithMessage("CutId is required")
               .MustAsync(async (CutId, cancellation) => await CheckId(CutId, "CutId"))
               .WithMessage("CutId does not exist.");

            RuleFor(g => g.OriginId)
               .NotEmpty()
               .WithMessage("OriginId is required")
               .MustAsync(async (OriginId, cancellation) => await CheckId(OriginId, "OriginId"))
               .WithMessage("OriginId does not exist.");

            RuleFor(g => g.ShapeId)
               .NotEmpty()
               .WithMessage("ShapeId is required")
               .MustAsync(async (ShapeId, cancellation) => await CheckId(ShapeId, "ShapeId"))
               .WithMessage("ShapeId does not exist.");
        }

        private async Task<bool> CheckId(int id, string type)
        {
            var result = await _unitOfWork.Gems.CheckId(id, type);
            return result;
        }

    }
}
