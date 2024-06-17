using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace SignalRHub
{
    public class NoticationHub : Hub
    {
        private static int _connectedClients = 0;
        private static Dictionary<string, string> _users = new Dictionary<string, string>();

        public override async Task<Task> OnConnectedAsync()
        {
            string userName = Context.User.Identity.Name;
            string conID = Context.ConnectionId;

            _users.Add(userName, conID);
            _connectedClients++;
            string allUserSerilized = AllLiveUsers();
            await Clients.All.SendAsync("noOfConnectedClients", _connectedClients, allUserSerilized);
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            bool isRemoved = _users.Remove(Context.User.Identity.Name);
            _connectedClients--;

            string allUserSerilized = AllLiveUsers();
            await Clients.All.SendAsync("noOfConnectedClients", _connectedClients, allUserSerilized);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToAllClients(string user, string message)
        {
            await Clients.Others.SendAsync("getMessage", user, message);
        }

        public async Task SendNotificationTo(string userName,string message)
        {
            string conID = _users[userName];
            await Clients.Client(conID).SendAsync("noticationReceived",message);
        }

        private string AllLiveUsers()
        {
            List<string> users = _users.Select(user => user.Key).ToList();
            string serilizedUsers = JsonSerializer.Serialize(users);
            return serilizedUsers;
        }
    }
}
