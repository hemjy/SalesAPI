using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Application.Commons;
using SalesAPI.Application.DTOs.SalesOrders;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Features.SalesOrders.Queries
{
    public class GetHighestSoldProductQuery : IRequest<Result<GetHighestSoldProductDto>>{}

    internal class GetHighestSoldProductQueryHandler(IGenericRepositoryAsync<SalesOrder> salesOrderRepository, IGenericRepositoryAsync<Product> productRepository) : IRequestHandler<GetHighestSoldProductQuery, Result<GetHighestSoldProductDto>>
    {
        private readonly IGenericRepositoryAsync<SalesOrder> _salesOrderRepository = salesOrderRepository;
        private readonly IGenericRepositoryAsync<Product> _productRepository = productRepository;

        public async Task<Result<GetHighestSoldProductDto>> Handle(GetHighestSoldProductQuery request, CancellationToken cancellationToken)
        {
            var highestSoldProductQuery = _salesOrderRepository.GetAllQuery()
                 .Where(o => !o.IsDeleted)
                 .Include(o => o.Product)
                 .GroupBy(o => new {o.Product.Name, o.ProductId})
                 .OrderByDescending(g => g.Sum(o => o.Quantity))
                 .Select( o => new {
                    o.Key.Name, 
                    Quantity = o.Sum(o => o.Quantity)
                 });

            var highestSoldProduct = await highestSoldProductQuery.FirstOrDefaultAsync(cancellationToken);

            var highestPricedProductQuery  = _productRepository.GetAllQuery()
                 .Where(p => !p.IsDeleted)
                 .OrderByDescending(p => p.Price)
                 .Select(p => new {
                      p.Name,
                     p.Price
                 });
            var highestPricedProduct = await highestPricedProductQuery.FirstOrDefaultAsync(cancellationToken);


            return Result<GetHighestSoldProductDto>.Success(new GetHighestSoldProductDto
            {
                HighestPrice = highestPricedProduct?.Price,
                HighestPricedProduct = highestPricedProduct?.Name,
                HighestSoldProduct = highestSoldProduct?.Name,
                HighestSoldProductQuantity = highestSoldProduct?.Quantity
            });
        }
    }
}
