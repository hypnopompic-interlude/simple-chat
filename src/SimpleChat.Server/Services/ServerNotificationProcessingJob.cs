using Quartz;
using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Services;

namespace SimpleChat.Server.Services;

[DisallowConcurrentExecution]
public class ServerNotificationProcessingJob : IJob
{
    public const int ProcessingTimeSpanSec = 10;

    private readonly ISystemMessageProvider _messageProvider;
    private readonly ILogger<ServerNotificationProcessingJob> _logger;
    private readonly INotificationSender<ChatTextMessageModel> _sender;

    public ServerNotificationProcessingJob(
        ILogger<ServerNotificationProcessingJob> logger,
        INotificationSender<ChatTextMessageModel> sender,
        ISystemMessageProvider messageProvider)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(ProcessingTimeSpanSec));

        try
        {
            await _sender.NotifyAsync(
                ChatTextMessageModel.Create(
                    _messageProvider.GetSystem().Name,
                    _messageProvider.GetSystemGreatingMessage()),
                cts.Token);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
