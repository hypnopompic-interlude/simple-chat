namespace SimpleChat.Shared.Chats;

public interface IChatHubReceiver
{
    void OnReceiveMessage(MessageRecievedEvent message);
    void OnReceiveMessage(string message);
    void OnUserJoined(string userName);

    void OnUserDisconnected(string userName);
}
