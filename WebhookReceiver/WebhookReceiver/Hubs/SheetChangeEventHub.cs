using Microsoft.AspNetCore.SignalR;
namespace WebhookReceiver.Hubs
{
    public class SheetChangeEventHub : Hub
    {
        public async Task JoinFileGroup(string fileId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, fileId);
        }
    }
}
