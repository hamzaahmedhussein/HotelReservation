using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace HotelReservation.Hubs
{
    public class ChatHub : Hub
    {
        // Store the users' connections
        private static ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();

        // When a user connects, associate the user ID with their connection ID
        public override Task OnConnectedAsync()
        {
            string userId = Context.GetHttpContext().Request.Query["userId"];
            _userConnections[userId] = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        // When a user disconnects, remove their connection ID
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            _userConnections.TryRemove(userId, out _);

            return base.OnDisconnectedAsync(exception);
        }

        // Method to send a message from one user to another
        public async Task SendMessage(string senderId, string receiverId, string content)
        {
            if (_userConnections.TryGetValue(receiverId, out var receiverConnectionId))
            {
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, content);
            }
        }
    }
}
