using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GiapDepTrai.Hubs
{
    public class KitchenHub : Hub
    {
        public async Task SendOrderNotification(string message)
        {

            await Clients.Group("Kitchen").SendAsync("ReceiveOrderNotification", message);
        }

        public async Task JoinKitchenGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Kitchen");
        }

        public async Task LeaveKitchenGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Kitchen");
        }
    }
}