using MagicOnion.Server.Hubs;
using SimpleChat.Shared.Users;

namespace SimpleChat.Server.Chats;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class ExceptionHandlingFilterAttribute : StreamingHubFilterAttribute
{
    private readonly ILogger<ExceptionHandlingFilterAttribute> _logger;
    private readonly IGroupService _groupService;
    private readonly ICurrentUser _currentUser;

    public ExceptionHandlingFilterAttribute(ILogger<ExceptionHandlingFilterAttribute> logger, IGroupService groupService, ICurrentUser currentUser)
    {
        _logger = logger;
        _groupService = groupService;
        _currentUser = currentUser;
    }

    public override async ValueTask Invoke(StreamingHubContext context, Func<StreamingHubContext, ValueTask> next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);

            context.ServiceContext.CallContext.Status = Grpc.Core.Status.DefaultCancelled;

            _groupService.SendError(
                new ChatUser(context.ConnectionId, _currentUser.Name),
                new Shared.Error(ex.GetType().Name, ex.Message));
        }
    }
}
