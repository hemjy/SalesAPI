using MediatR;
using SalesAPI.Application.Commons;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Application.Interfaces.Services;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }


    }

    internal class CreateProductCommandHandler(IGenericRepositoryAsync<Product> productRepository) : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IGenericRepositoryAsync<Product> _productRepository = productRepository;
        

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productExisted = await _productRepository.IsUniqueAsync(x => x.Name.Trim().ToLower() == request.Name.Trim().ToLower() && !x.IsDeleted);
            if (productExisted)
            {
                return Result<Guid>.Failure("Product Already Exist.");
            }

            var newProduct = Product.Create(request.Name, request.Price);

            await _productRepository.AddAsync(newProduct);
            return Result<Guid>.Success(newProduct.Id);
        }
    }
}
