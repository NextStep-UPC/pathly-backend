using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using pathly_backend.Sessions.Application.Interfaces;
using pathly_backend.Sessions.Application.Dtos;

namespace pathly_backend.Sessions.Interfaces.Hubs;

[Authorize]
public class SessionChatHub : Hub
{
    private readonly IChatService _chatSvc;
    public SessionChatHub(IChatService chatSvc)
    {
        _chatSvc = chatSvc;
    }

    public override async Task OnConnectedAsync()
    {
        var http = Context.GetHttpContext();
        var sessionId = Guid.Parse(http.Request.Query["sessionId"]);
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId.ToString());
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(Guid sessionId, string content)
    {
        var senderId = Guid.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        // Guardar en BD
        var dto = await _chatSvc.SaveMessageAsync(sessionId, senderId, content);
        // Emitir a todos en el grupo
        await Clients.Group(sessionId.ToString())
            .SendAsync("ReceiveMessage", dto);
    }
}