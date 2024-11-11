using Microsoft.EntityFrameworkCore;
using SalesAPI.Application.Commons;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Helpers
{
    public static class IQueryableExtensions
    {
        public static async Task<Result<IReadOnlyList<TDto>>> GetPagedAsync<TEntity, TDto>(
            this IQueryable<TEntity> query,
            PaginationRequest request,
            CancellationToken cancellationToken,
            Func<TEntity, string> searchTextSelector,
            Func<TEntity, TDto> projectionSelector)
            where TEntity : class, IEntityBase
        {
            // Apply filters based on the request
            query = ApplyFilters(query, request, searchTextSelector);

            // Get the total count for pagination
            var total = await query.CountAsync(cancellationToken);

            // Select the data with pagination and projection
            // Apply pagination before the projection
            var pagedQuery = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            // Select the data and project it to the DTO
            var items =  pagedQuery
                .Select(projectionSelector)  // Projection to DTO
                .ToList();  

            // Return paged result
            return Result<IReadOnlyList<TDto>>.Success(items, request.PageNumber, request.PageSize, total);
        }

        private static IQueryable<TEntity> ApplyFilters<TEntity>(
            IQueryable<TEntity> query,
            PaginationRequest request,
            Func<TEntity, string> searchTextSelector)
            where TEntity : class, IEntityBase
        {
            // Apply filtering based on the Id
            query = query.AsNoTracking();
            if (request.Id.HasValue)
            {
                query = query.Where(x => x.Id == request.Id.Value);
            }

            // Apply filtering based on the StartDate and EndDate
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                query = query.Where(x => x.Created >= request.StartDate.Value && x.Created <= request.EndDate.Value);
            }

            // Apply filtering based on the search text
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var searchText = request.SearchText.ToLower().Trim();
                query = query.Where(x => searchTextSelector(x).ToLower().Contains(searchText));
            }

            return query;
        }
    }

}
