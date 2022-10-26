using Microsoft.AspNetCore.SignalR;

namespace SignalRChatBlazorClient.Server.Hubs
{
    public class ChatHub : Hub
    {

        private static Dictionary<string, string> Users = new Dictionary<string, string>(); // the idea is to map username to conncetionId
        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);

            await AddMessageToChat(string.Empty, $"{username} joined to chat!");
            await base.OnConnectedAsync();
        } 

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(un => un.Key == Context.ConnectionId).Value;
            await AddMessageToChat(string.Empty, $"{username} left chat!");
        }
        public async Task AddMessageToChat(string user, string message)
        {
            await Clients.All.SendAsync("SendToClient", user, $"{message} \tat {DateTime.Now.ToString("HH:mm:ss")}");
        }
    }
}
