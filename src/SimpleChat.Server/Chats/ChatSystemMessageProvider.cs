using SimpleChat.Shared.Chats;
using SimpleChat.Shared.Users;

namespace SimpleChat.Server.Chats;

public class ChatSystemMessageProvider : ISystemMessageProvider
{
    public const string Server = "[SERVER]";

    public string SystemPrefix => Server;

    public string GetInvalidUserNameMessage(string userName)
    {
        return string.Format("Invalid User Name - {0}", userName);
    }

    private static readonly ChatUser _system = new ChatUser(Guid.Empty, Server);

    public ChatUser GetSystem() => _system;

    public string GetSystemGreatingMessage() => string.Format("Hello chat members at {0}}", DateTime.UtcNow);

    public string GetUserJoinedChatMessage(string userName) => string.Format("{0} Has Joined Chat at {1}", userName, DateTime.UtcNow);

    public string GetUserLeaveChatMessage(string userName) => string.Format("{0} Has Leaved Chat at {1}", userName, DateTime.UtcNow);
}
