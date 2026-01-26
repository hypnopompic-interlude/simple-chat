using SimpleChat.Shared.Chats;

namespace SimpleChat.Server.Chats;

public interface IGroupService
{
    ValueTask AddMemberAsync(Guid connectionId, IChatHubReceiver receiver, CancellationToken cancellationToken = default);
    ValueTask RemoveMemberAsync(Guid connectionId, CancellationToken cancellationToken = default);
    void SendMessageToAll(MessageRecievedEvent message);
}