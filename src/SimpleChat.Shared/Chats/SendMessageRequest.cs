using MessagePack;

namespace SimpleChat.Shared.Chats;

[MessagePackObject]
public class SendMessageRequest
{
    [Key(0)]
    public required string Content { get; init; }
}