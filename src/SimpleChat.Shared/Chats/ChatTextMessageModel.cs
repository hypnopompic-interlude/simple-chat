using MessagePack;

namespace SimpleChat.Shared.Chats;

[MessagePackObject]
public class ChatTextMessageModel 
{
    public static ChatTextMessageModel Create(string author, string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(author);
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        return new ChatTextMessageModel { Author = author, Text = text };
    }        

    [Key(0)]
    public required string Author { get; set; }
    [Key(1)]
    public required string Text { get; set; }
}
