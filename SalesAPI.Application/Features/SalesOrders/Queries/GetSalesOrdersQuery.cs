using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Application.Commons;
using SalesAPI.Application.DTOs.SalesOrders;
using SalesAPI.Application.Helpers;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Features.Products.Queries
{
    public class GetSalesOrdersQuery : PaginationRequest, IRequest<Result<IReadOnlyList<GetSalesOrderDto>>>
    {
    }

    internal class GetSalesOrdersQueryHandler(IGenericRepositoryAsync<SalesOrder> salesOrderRepository) : IRequestHandler<GetSalesOrdersQuery, Result<IReadOnlyList<GetSalesOrderDto>>>
    {
        private readonly IGenericRepositoryAsync<SalesOrder> _salesOrderRepository = salesOrderRepository;

        public async Task<Result<IReadOnlyList<GetSalesOrderDto>>> Handle(GetSalesOrdersQuery request, CancellationToken cancellationToken)
        {

            var query = _salesOrderRepository.GetAllQuery()
                .Include(x => x.Product);

            return await query.GetPagedAsync(
                        request,
                        cancellationToken,
                        x => x.Product.Name,  
                        x => new GetSalesOrderDto  
                        {
                            Id = x.Id,
                            CustomerId = x.CustomerId,
                            ProductId = x.ProductId,
                            ProductName = x.Product.Name, 
                            Quantity = x.Quantity,    
                            UnitPrice = x.UnitPrice,  
                            OrderDate = x.Created     
                        });

           
        }
    }
}
