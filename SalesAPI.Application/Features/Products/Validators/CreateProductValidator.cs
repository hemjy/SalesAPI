using FluentValidation;
using SalesAPI.Application.Features.Products.Commands;

namespace SalesAPI.Application.Features.Products.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

        }
    }
}
