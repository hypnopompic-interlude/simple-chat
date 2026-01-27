namespace SimpleChat.Server.Chats;

public sealed class ChatHubSettings
{
    public const string Section = "ChatHubSettings";

    public required string RedisConnectionString {  get; set; }
}
