namespace SimpleChat.Shared.Chats;

public interface IChatHubReceiver
{
    void OnReceiveMessage(MessageRecievedEvent message);
    void OnReceiveMessage(string message);
    void OnUserJoined(string token);    

    void OnError(ErrorModel error);
}
