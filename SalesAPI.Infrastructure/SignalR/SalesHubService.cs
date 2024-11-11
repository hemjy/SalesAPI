using Microsoft.AspNetCore.SignalR;
using SalesAPI.Application.Interfaces.Services;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Infrastructure.SignalR
{
    public class SalesHubService : ISalesHubService
    {
        private readonly IHubContext<SalesHub> _hubContext;

        public SalesHubService(IHubContext<SalesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastSaleUpdate(SalesOrder salesOrder)
        {
            // Broadcast the sales order update to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveSaleUpdate", salesOrder);
        }
    }
}
