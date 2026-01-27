namespace SimpleChat.Shared.Chats;

public interface IChatHubReceiver
{
    bool IsConnected { get; }

    void OnReceiveMessage(MessageRecievedEvent message);
    
    void OnUserJoined(string token);    

    void OnError(ErrorModel error);
}
