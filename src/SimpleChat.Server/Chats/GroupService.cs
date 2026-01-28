using Cysharp.Runtime.Multicast;
using Cysharp.Runtime.Multicast.Remoting;
using SimpleChat.Shared;
using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Users;
using System.Collections.Concurrent;
using System.Reflection;

namespace SimpleChat.Server.Chats;

internal sealed class GroupService(IMulticastGroupProvider groupProvider) : IDisposable, IGroupService
{
    private const string GroupName = "PublicChatGroup";

    private readonly IMulticastAsyncGroup<ChatUser, IChatHubReceiver> _group
        = groupProvider.GetOrAddGroup<ChatUser, IChatHubReceiver>(GroupName);

    public void Dispose() => _group.Dispose();

    public ValueTask AddMemberAsync(ChatUser user, IChatHubReceiver receiver, CancellationToken cancellationToken = default)
        => _group.AddAsync(user, receiver, cancellationToken);

    public ValueTask RemoveMemberAsync(ChatUser user, CancellationToken cancellationToken = default)
        => _group.RemoveAsync(user, cancellationToken);

    public void SendToken(ChatUser user, string token)
    {
        _group.Single(user).OnUserJoined(token);
    }

    public void SendError(ChatUser user, Error error) 
        => _group.Single(user)?.OnError(new ErrorModel { Code = error.Code, Message = error.Message });

    public bool Contains(ChatUser user)
    {
        var groupType = _group.GetType();

        var remoteGroupField = _group.GetType()
            .GetField(
                "_remoteGroup",
                BindingFlags.Instance | BindingFlags.NonPublic);

        var remoteGroupFieldValue = remoteGroupField?.GetValue(_group);
        var remoteGroupFieldType = remoteGroupFieldValue?.GetType();

        var receiversFieldValue = remoteGroupFieldType?
            .GetField(
                "_receivers",
                BindingFlags.Instance | BindingFlags.NonPublic)
            .GetValue(remoteGroupFieldValue)

        as ConcurrentDictionary<ChatUser, IRemoteReceiverWriter>;

        //    as Cysharp.Runtime.Multicast.Remoting.RemoteGroup<ChatUser, IChatHubReceiver>;

        return receiversFieldValue is not null && receiversFieldValue.Keys.Any(x => x.Name.Equals(user.Name));
    }

    public void BroadcastMessage(ChatTextMessageModel message)
        => _group.All.OnReceiveMessage(new MessageRecievedEvent { Message = message });

    public void SendMessage(ChatUser receipient, ChatTextMessageModel message) 
        => _group.Single(receipient)?.OnReceiveMessage(new MessageRecievedEvent { Message = message });
}