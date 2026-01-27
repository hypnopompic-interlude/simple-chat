namespace SimpleChat.Shared.Chats;

public interface IChatService
{
    ValueTask JoinAsync(string userName);
    ValueTask SendMessageAsync(SendMessageRequest message);
    ValueTask LeaveAsync();
}
