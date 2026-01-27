namespace SimpleChat.Shared.Users;

public interface ITokenGenerator
{
    string GenerateToken(ChatUser user);
}