using SalesAPI.Domain.Entities;

namespace SalesAPI.Application.Interfaces.Services
{
    public interface ISalesHubService
    {
        Task BroadcastSaleUpdate(SalesOrder salesOrder);
    }
}
