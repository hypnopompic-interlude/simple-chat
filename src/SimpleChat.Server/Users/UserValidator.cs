using SimpleChat.Shared.Users;

namespace SimpleChat.Server.Users;

internal sealed class UserValidator : IUserValidator
{
    public bool IsValidNickName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)
            || Char.IsLetter(name[0]) is false
            || name.Any(x => char.IsLetterOrDigit(x) is false) is true)
        {
            return false;
        }
        return true;
    }

    public bool IsValidNickName(ChatUser user) => user is not null && IsValidNickName(user.Name);
}