using MessagePack;

namespace SimpleChat.Shared.Chats;

[MessagePackObject]
public class MessageRecievedEvent
{
    [Key(0)]
    public required string UserName { get; init; }
    [Key(1)]
    public required string Content { get; init; }
    [Key(2)]
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
