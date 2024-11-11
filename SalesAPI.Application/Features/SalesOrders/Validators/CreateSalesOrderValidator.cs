using FluentValidation;
using SalesAPI.Application.Features.Products.Commands;

namespace SalesAPI.Application.Features.SalesOrders.Validators
{
    public class CreateSalesOrderValidator : AbstractValidator<CreateSalesOrderCommand>
    {
        public CreateSalesOrderValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.")
                .Must(BeValidGuid).WithMessage("Product ID must be a valid GUID.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.")
                .Must(BeValidGuid).WithMessage("Customer ID must be a valid GUID.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
        }

        // Custom rule to check if a GUID is valid
        private bool BeValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }

}
