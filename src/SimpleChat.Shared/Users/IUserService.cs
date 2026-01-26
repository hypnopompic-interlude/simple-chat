namespace SimpleChat.Shared.Users;

public interface IUserService
{
    ValueTask<bool> CanJoinChatRoomAsync(string userName, CancellationToken cancellationToken = default);
    ValueTask JoinAsync(ChatUser user, CancellationToken cancellationToken = default);
}

public sealed class JoinChatRequest
{
    public required string UserName { get; init; }
}