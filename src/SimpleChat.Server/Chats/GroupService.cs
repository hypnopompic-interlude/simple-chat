using Cysharp.Runtime.Multicast;
using SimpleChat.Shared.Chats;

namespace SimpleChat.Server.Chats;

internal sealed class GroupService(IMulticastGroupProvider groupProvider) : IDisposable, IGroupService
{
    private readonly IMulticastAsyncGroup<Guid, IChatHubReceiver> _group
        = groupProvider.GetOrAddGroup<Guid, IChatHubReceiver>("ChatGroup");

    public void SendMessageToAll(MessageRecievedEvent message) => _group.All.OnReceiveMessage(message);

    public void Dispose() => _group.Dispose();

    public ValueTask AddMemberAsync(Guid connectionId, IChatHubReceiver receiver, CancellationToken cancellationToken = default)
        => _group.AddAsync(connectionId, receiver, cancellationToken);
    public ValueTask RemoveMemberAsync(Guid connectionId, CancellationToken cancellationToken = default)
        => _group.RemoveAsync(connectionId, cancellationToken);
}