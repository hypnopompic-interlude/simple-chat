using Cysharp.Runtime.Multicast;
using SimpleChat.Shared;
using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Users;

namespace SimpleChat.Server.Chats;

internal sealed class GroupService(IMulticastGroupProvider groupProvider) : IDisposable, IGroupService
{
    private const string GroupName = "PublicChatGroup";

    private readonly IMulticastAsyncGroup<Guid, IChatHubReceiver> _group
        = groupProvider.GetOrAddGroup<Guid, IChatHubReceiver>(GroupName);

    public void Dispose() => _group.Dispose();

    public ValueTask AddMemberAsync(ChatUser user, IChatHubReceiver receiver, CancellationToken cancellationToken = default)
        => _group.AddAsync(user.ConnectionID, receiver, cancellationToken);

    public ValueTask RemoveMemberAsync(ChatUser user, CancellationToken cancellationToken = default)
        => _group.RemoveAsync(user.ConnectionID, cancellationToken);

    public void SendToken(ChatUser user, string token)
    {
        _group.Single(user.ConnectionID).OnUserJoined(token);
    }

    public void SendError(ChatUser user, Error error) 
        => _group.Single(user.ConnectionID)?.OnError(new ErrorModel { Code = error.Code, Message = error.Message });

    public bool Contains(ChatUser user)
    {
        return _group.Single(user.ConnectionID) is not null;
    }

    public void BroadcastMessage(ChatTextMessageModel message)
        => _group.All.OnReceiveMessage(new MessageRecievedEvent { Message = message });

    public void SendMessage(ChatUser receipient, ChatTextMessageModel message) 
        => _group.Single(receipient.ConnectionID)?.OnReceiveMessage(new MessageRecievedEvent { Message = message });
}