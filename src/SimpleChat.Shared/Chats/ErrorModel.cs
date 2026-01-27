using MessagePack;

namespace SimpleChat.Shared.Chats;

[MessagePackObject]
public class ErrorModel
{
    [Key(0)]
    public required string Code {  get; set; }
    [Key(1)]
    public required string Message { get; set; }
}
