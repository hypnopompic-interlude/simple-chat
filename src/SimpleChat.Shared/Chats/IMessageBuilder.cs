namespace SimpleChat.Shared.Chats;

public interface IMessageBuilder: IContentSetter, IPrefixSetter
{
    Result<string> Build();
}

public interface IContentSetter
{
    IMessageBuilder WithContent(string content);
}

public interface IPrefixSetter
{
    IMessageBuilder WithPrefix(string prefix);
}

