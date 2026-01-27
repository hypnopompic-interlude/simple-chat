namespace SimpleChat.Shared.Services;

public interface INotificationSender<T>
    where T : class
{
    ValueTask NotifyAsync(T notification, CancellationToken cancellationToken = default);
}

