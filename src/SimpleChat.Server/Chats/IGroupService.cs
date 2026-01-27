using SimpleChat.Shared;
using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Users;

namespace SimpleChat.Server.Chats;

public interface IGroupService: IMembershipService
{
    void BroadcastMessage(ChatTextMessageModel message);
    void SendMessage(ChatUser receipient, ChatTextMessageModel message);
    void SendToken(ChatUser receipient, string token);
    bool Contains(ChatUser user);
    void SendError(ChatUser receipient, Error error);
}

public interface IMembershipService
{
    ValueTask AddMemberAsync(ChatUser user, IChatHubReceiver receiver, CancellationToken cancellationToken = default);
    ValueTask RemoveMemberAsync(ChatUser user, CancellationToken cancellationToken = default);
}