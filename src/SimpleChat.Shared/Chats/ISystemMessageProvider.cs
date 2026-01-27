using SimpleChat.Shared.Users;

namespace SimpleChat.Shared.Chats;

public interface ISystemMessageProvider
{
    string GetInvalidUserNameMessage(string userName);
    string SystemPrefix { get; }
    string GetUserJoinedChatMessage(string userName);
    string GetUserLeaveChatMessage(string userName);
    string GetSystemGreatingMessage();
    ChatUser GetSystem();
}
