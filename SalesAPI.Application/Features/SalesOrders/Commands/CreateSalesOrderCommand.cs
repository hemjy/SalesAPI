using MediatR;
using Microsoft.AspNetCore.Identity;
using SalesAPI.Application.Commons;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Features.Products.Commands
{
    public class CreateSalesOrderCommand : IRequest<Result<Guid>>
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

       
    }

    internal class CreateSalesOrderCommandHandler(
            IGenericRepositoryAsync<SalesOrder> salesOrderRepository,
            UserManager<User> userManager, 
            IGenericRepositoryAsync<Product> productRepository
        ) : IRequestHandler<CreateSalesOrderCommand, Result<Guid>>
    {
        private readonly IGenericRepositoryAsync<SalesOrder> _salesOrderRepository = salesOrderRepository;
        private readonly UserManager<User> userManager = userManager;
        private readonly IGenericRepositoryAsync<Product> _productRepository = productRepository;

        public async Task<Result<Guid>> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
        {

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result<Guid>.Failure("Product not found.");
            }

            var salesOrder = SalesOrder.Create(productId: request.ProductId, customerId: request.CustomerId, quantity: request.Quantity, unitPrice: request.UnitPrice);


            await _salesOrderRepository.AddAsync(salesOrder);
            return Result<Guid>.Success(salesOrder.Id);
        }
    }

}
