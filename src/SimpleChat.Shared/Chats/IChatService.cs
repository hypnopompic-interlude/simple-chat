namespace SimpleChat.Shared.Chats;

public interface IChatService
{
    ValueTask ConnectAsync(string userName);
    ValueTask JoinAsync();
    ValueTask SendMessageAsync(SendMessageRequest message);
    ValueTask LeaveAsync();
}
