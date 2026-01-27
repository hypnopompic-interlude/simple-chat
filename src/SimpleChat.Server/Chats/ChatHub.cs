using Cysharp.Runtime.Multicast.Distributed.Redis;
using MagicOnion.Server;
using MagicOnion.Server.Hubs;
using Microsoft.AspNetCore.Authorization;
using SimpleChat.Shared;
using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Users;

namespace SimpleChat.Server.Chats;

[GroupConfiguration(typeof(RedisGroupProvider))]

[FromTypeFilter(typeof(ExceptionHandlingFilterAttribute))]
public class ChatHub : StreamingHubBase<IChatHub, IChatHubReceiver>, IChatHub
{
    private readonly IUserValidator _userValidator;
    private readonly IGroupService _groupService;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    private readonly ITokenGenerator _tokenGenerator;
    public ChatHub(
        IGroupService groupService,        
        ISystemMessageProvider systemMessageProvider,
        IUserValidator userValidator,
        ITokenGenerator tokenGenerator,
        ICurrentUser currentUser 
        )
    {
        _currentUser = currentUser;
        _groupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        _systemMessageProvider = systemMessageProvider ?? throw new ArgumentNullException(nameof(systemMessageProvider));
        _userValidator = userValidator ?? throw new ArgumentNullException(nameof(userValidator));
        _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
    }

    protected override async ValueTask OnDisconnected()
    {
        await LeaveAsync();
    }

    [AllowAnonymous]
    public async ValueTask JoinAsync(string userName)
    {
        var user = new ChatUser(ConnectionId, userName);

        bool isMember = _groupService.Contains(user);

        if (_userValidator.IsValidNickName(user) is false) 
        {
            Client.OnReceiveMessage(
                new MessageRecievedEvent 
                {
                    Message = ChatTextMessageModel.Create(
                        userName,
                        _systemMessageProvider.GetInvalidUserNameMessage(userName))
                });

            return; 
        }
        try
        {
            await _groupService.AddMemberAsync(user, Client);

            string token = _tokenGenerator.GenerateToken(user);

            _groupService.SendToken(user, token);
        }
        catch (Exception ex)
        {
            if (_groupService.Contains(user))
            {
                await _groupService.RemoveMemberAsync(user);
            }

            HandleError(user, new Error("Error.CannotJoinChat", ex.Message));

            return;
        }

        _groupService.BroadcastMessage(
            ChatTextMessageModel.Create(
                    _systemMessageProvider.SystemPrefix,
                    _systemMessageProvider.GetUserJoinedChatMessage(userName)
                    ));
    }

    private void HandleError(ChatUser user, Error error)
    {
        Context.CallContext.Status = Grpc.Core.Status.DefaultCancelled;

        _groupService.SendError(user, error);
    }
    [Authorize]
    public async ValueTask LeaveAsync()
    {
        var user = new ChatUser(ConnectionId, _currentUser?.Name ?? string.Empty);

        if (_groupService.Contains(user) is false || _userValidator.IsValidNickName(user) is false)
        {
            HandleError(user, Error.NotFound);

            return;
        }

        await _groupService.RemoveMemberAsync(user);

        _groupService.BroadcastMessage(
            ChatTextMessageModel.Create(
                _systemMessageProvider.SystemPrefix,
                _systemMessageProvider.GetUserLeaveChatMessage(user.Name)
                ));
    }
    [Authorize]
    public ValueTask SendMessageAsync(SendMessageRequest message)
    {
        if (_userValidator.IsValidNickName(_currentUser?.Name ?? string.Empty))
        {
            _groupService.BroadcastMessage(ChatTextMessageModel.Create(_currentUser.Name, message.Content));
        }

        return ValueTask.CompletedTask;
    }
}