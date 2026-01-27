using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Services;

namespace SimpleChat.Server.Chats;

public class ChatNotificationSender : INotificationSender<ChatTextMessageModel>
{
    private readonly IGroupService _groupService;

    public ChatNotificationSender(IGroupService groupService)
    {
        _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
    }

    public ValueTask NotifyAsync(ChatTextMessageModel notification, CancellationToken cancellationToken = default)
    {
        _groupService.BroadcastMessage(notification);

        return ValueTask.CompletedTask;
    }
}
