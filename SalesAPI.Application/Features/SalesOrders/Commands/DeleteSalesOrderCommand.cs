using MediatR;
using SalesAPI.Application.Commons;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Features.Products.Commands
{
    public class DeleteSalesOrderCommand(Guid id) : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; } = id;


    }

    internal class DeleteSalesOrderCommandHandler(IGenericRepositoryAsync<SalesOrder> salesOrderRepository) : IRequestHandler<DeleteSalesOrderCommand, Result<Guid>>
    {
        private readonly IGenericRepositoryAsync<SalesOrder> _salesOrderRepository = salesOrderRepository;

        public async Task<Result<Guid>> Handle(DeleteSalesOrderCommand request, CancellationToken cancellationToken)
        {
            if(request.Id == Guid.Empty) return Result<Guid>.Failure("Sales Order Id is Required.");
            var salesOrder = await salesOrderRepository.GetByIdAsync(request.Id);
            if (salesOrder == null || salesOrder.IsDeleted)
            {
                return Result<Guid>.Failure("Sales Order not found.");
            }

            salesOrder.IsDeleted = true;

            await _salesOrderRepository.UpdateAsync(salesOrder);

            return Result<Guid>.Success(salesOrder.Id);
        }
    }
}
