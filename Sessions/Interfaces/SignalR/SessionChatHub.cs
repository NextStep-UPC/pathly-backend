using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace pathly_backend.Sessions.Interfaces.SignalR;

[Authorize]
public class SessionChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var sessionId = Context.GetHttpContext()!.Request.Query["sessionId"];
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(Guid sessionId, string message)
    {
        var user = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await Clients.Group(sessionId.ToString())
            .SendAsync("ReceiveMessage", user, message, DateTime.UtcNow);
    }
}