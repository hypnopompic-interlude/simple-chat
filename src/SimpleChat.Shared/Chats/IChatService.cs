namespace SimpleChat.Shared.Chats;

public interface IChatService
{
    ValueTask JoinAsync(string userName, CancellationToken cancellationToken = default);
    ValueTask SendMessageAsync(string message, CancellationToken cancellationToken = default);
    ValueTask LeaveAsync(CancellationToken cancellationToken = default);
}
