using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> UserConnections = new();

    public async Task NotifyUserStatus(string userId, bool isOnline)
    {
        await Clients.All.SendAsync("UpdateUserStatus", userId, isOnline);
    }
    public async Task NotifyTyping(string fromUserId, string toUserId, bool isTyping)
    {
        if (UserConnections.TryGetValue(toUserId, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("UserTyping", fromUserId, isTyping);
            Console.WriteLine($"{fromUserId} is typing to {toUserId}: {isTyping}");
        }
    }

    public override async Task OnConnectedAsync()
    {
        // Assume the user ID is passed as a query parameter
        var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

        if (!string.IsNullOrEmpty(userId))
        {
            UserConnections[userId] = Context.ConnectionId;
            Console.WriteLine($"User connected: {userId}, ConnectionId: {Context.ConnectionId}");
            
            // Notify others that the user is online
            await NotifyUserStatus(userId, true);

        }
        else
        {
            Console.WriteLine("Connection without userId query parameter.");
        }
        // Notify others that the user is online

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // Remove the user from the connections dictionary
        var userId = UserConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (userId != null)
        {
            UserConnections.TryRemove(userId, out _);
            Console.WriteLine($"User disconnected: {userId}");

            // Notify others that the user is offline
            await NotifyUserStatus(userId, false);
        }


        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string toUserId, string message)
    {
        if (UserConnections.TryGetValue(toUserId, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            Console.WriteLine($"Message sent to {toUserId}: {message}");
        }
        else
        {
            Console.WriteLine($"User {toUserId} is not connected.");
        }
    }

   

}
