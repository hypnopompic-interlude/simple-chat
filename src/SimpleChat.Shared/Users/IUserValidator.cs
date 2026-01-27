namespace SimpleChat.Shared.Users;

public interface IUserValidator
{
    bool IsValidNickName(ChatUser user);
    bool IsValidNickName(string name);
}
