using Cysharp.Runtime.Multicast.Distributed.Redis;
using MagicOnion.Server.Hubs;
using Microsoft.AspNetCore.Authorization;
using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Users;

namespace SimpleChat.Server.Chats;

[GroupConfiguration(typeof(RedisGroupProvider))]
[Authorize]
public class ChatHub : StreamingHubBase<IChatHub, IChatHubReceiver>, IChatHub
{
    private readonly IChatService _chatService;
    private readonly IUserService _userService;
    private readonly IGroupService _groupService;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public ChatHub(IChatService chatService, IUserService userService, IGroupService groupService, ICurrentUser currentUser, ISystemMessageProvider systemMessageProvider)
    {
        _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _systemMessageProvider = systemMessageProvider ?? throw new ArgumentNullException(nameof(systemMessageProvider));
    }


    protected override async ValueTask OnDisconnected()
    {
        await _groupService.RemoveMemberAsync(ConnectionId);
    }

    [AllowAnonymous]
    public async ValueTask JoinAsync(string userName, CancellationToken cancellationToken = default)
    {
        bool canJoin = await _userService.CanJoinChatRoomAsync(userName, cancellationToken);

        if (canJoin is false) 
        {
            Client.OnReceiveMessage(_systemMessageProvider.InvalidUserNameMessage);

            return; 
        }

        await _groupService.AddMemberAsync(ConnectionId, Client, cancellationToken);

        TokenResponse token = await _userService.JoinAsync(new ChatUser(ConnectionId, userName), cancellationToken);
    }        

    public ValueTask LeaveAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask SendMessageAsync(SendMessageRequest message, CancellationToken cancellationToken = default)
        => SendMessageAsync(message.Content, cancellationToken);

    public ValueTask SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        _groupService.SendMessageToAll(
            new MessageRecievedEvent
            {
                Content = message,
                UserName = _currentUser.Name
            });

        return ValueTask.CompletedTask;
    }
}