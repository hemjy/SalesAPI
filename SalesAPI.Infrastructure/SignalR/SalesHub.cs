using Microsoft.AspNetCore.SignalR;
using SalesAPI.Domain.Entities;

namespace SalesAPI.Infrastructure.SignalR
{
    public sealed class SalesHub : Hub
    {
        // Method to send sales order updates to clients
        public async Task SendSaleUpdate(SalesOrder salesOrder)
        {
            await Clients.All.SendAsync("ReceiveSaleUpdate", salesOrder);
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Conected",$"{Context.ConnectionId} has joined");
           
        }
    }
}
