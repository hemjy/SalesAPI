using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SalesAPI.Application.Commons;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Application.Interfaces.Services;
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
            ISalesHubService salesHubService,
            ILogger<CreateSalesOrderCommandHandler> logger,
            IGenericRepositoryAsync<Product> productRepository
        ) : IRequestHandler<CreateSalesOrderCommand, Result<Guid>>
    {
        private readonly IGenericRepositoryAsync<SalesOrder> _salesOrderRepository = salesOrderRepository;
        private readonly UserManager<User> userManager = userManager;
        private readonly IGenericRepositoryAsync<Product> _productRepository = productRepository;
        private readonly ISalesHubService _salesHubService = salesHubService;
        private readonly ILogger<CreateSalesOrderCommandHandler> _logger = logger;

        public async Task<Result<Guid>> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
        {

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result<Guid>.Failure("Product not found.");
            }

            var salesOrder = SalesOrder.Create(productId: request.ProductId, customerId: request.CustomerId, quantity: request.Quantity, unitPrice: request.UnitPrice);

            await _salesOrderRepository.AddAsync(salesOrder);

            // Broadcasting the update via SignalR
            _logger.LogInformation("Broadcasting the update after add new sale order with Id: {Id}", salesOrder.Id);
            await _salesHubService.BroadcastSaleUpdate(salesOrder);
            return Result<Guid>.Success(salesOrder.Id);
        }
    }

}
