using MagicOnion;

namespace SimpleChat.Shared.Chats;

public interface IChatHub : IStreamingHub<IChatHub, IChatHubReceiver>, IChatService
{ 
}
