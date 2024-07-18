using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using JewelrySalesSystem.BAL.Interfaces;
using JewelrySalesSystem.BAL.Models.Invoices;

namespace JewelrySalesSystem.BAL.Validators.Invoices
{
    public class CreateInvoiceItemRequestValidator : AbstractValidator<CreateInvoiceItemRequest>
    {
        private readonly IProductService _productService;
        public CreateInvoiceItemRequestValidator(IProductService productService)
        {
            _productService = productService;


            RuleFor(i => i.ProductId).NotEmpty().WithMessage("ProductId is required")
                .Must(MustBeAnId).WithMessage("ProductId is not an integer or not greater than 0")
                .MustAsync(async (id, cancellationToken) => await beValidProduct(id, cancellationToken)).WithMessage($"Product Id is not existed");

            RuleFor(x => x.Quantity)
                .Must(MustBeAnId).WithMessage("Quantity is not an integer or not greater than 0");
                
        }

        private async Task<bool> beValidProduct(int id, CancellationToken cancellationToken)
        {
            var checkExist = await _productService.GetByIdAsync(id);
            if (checkExist != null)
            {
                return true;
            }
            return false;
        }
        private static bool MustBeAnId<T>(T value)
        {
            if (value == null)
                return true;

            if (value is int intValue && intValue > 0)
                return true;

            //if (value is string stringValue)
            //    return int.TryParse(stringValue, out _);

            return false;
        }
    }
}
