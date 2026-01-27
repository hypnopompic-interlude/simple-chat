using MessagePack;

namespace SimpleChat.Shared.Chats;

[MessagePackObject]
public class MessageRecievedEvent
{
    [Key(0)]
    public required ChatTextMessageModel Message { get; set; }
    [Key(1)]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
