using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Application.Commons;
using SalesAPI.Application.DTOs.Products;
using SalesAPI.Application.Helpers;
using SalesAPI.Application.Interfaces.Repositories;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Features.Products.Queries
{
    public class GetProductQuery : PaginationRequest, IRequest<Result<IReadOnlyList<GetProductDto>>>
    {
    }

    internal class GetProductQueryHandler(IGenericRepositoryAsync<Product> productRepository) : IRequestHandler<GetProductQuery, Result<IReadOnlyList<GetProductDto>>>
    {
        private readonly IGenericRepositoryAsync<Product> _productRepository = productRepository;

        public async Task<Result<IReadOnlyList<GetProductDto>>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {

            var query = _productRepository.GetAllQuery();

            return await query.GetPagedAsync(
                        request,
                        cancellationToken,
                        x => x.Name,
                        x => new GetProductDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Price = x.Price,
                            Created = x.Created
                        });


        }
    }
}
