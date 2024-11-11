using MediatR;
using Microsoft.Extensions.Logging;
using SalesAPI.Application.Commons;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Application.Interfaces.Services;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Features.Products.Commands
{
    public class DeleteSalesOrderCommand(Guid id) : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; } = id;


    }

    internal class DeleteSalesOrderCommandHandler(
        IGenericRepositoryAsync<SalesOrder> salesOrderRepository,
        ILogger<DeleteSalesOrderCommandHandler> logger,
        ISalesHubService salesHubService
        ) : IRequestHandler<DeleteSalesOrderCommand, Result<Guid>>
    {
        private readonly IGenericRepositoryAsync<SalesOrder> _salesOrderRepository = salesOrderRepository;
        private readonly ISalesHubService _salesHubService = salesHubService;
        private readonly ILogger<DeleteSalesOrderCommandHandler> _logger = logger;

        public async Task<Result<Guid>> Handle(DeleteSalesOrderCommand request, CancellationToken cancellationToken)
        {
            if(request.Id == Guid.Empty) return Result<Guid>.Failure("Sales Order Id is Required.");
            var salesOrder = await salesOrderRepository.GetByIdAsync(request.Id);
            if (salesOrder == null || salesOrder.IsDeleted)
            {
                return Result<Guid>.Failure("Sales Order not found.");
            }
            _logger.LogInformation("Get Sales Order with Id: {Id}", salesOrder.Id);
            salesOrder.IsDeleted = true;

            await _salesOrderRepository.UpdateAsync(salesOrder);

            _logger.LogInformation("Sales Order with Id: {Id} Deleted the Send Notification", salesOrder.Id);
            await _salesHubService.BroadcastSaleUpdate(salesOrder);

            return Result<Guid>.Success(salesOrder.Id);
        }
    }
}
