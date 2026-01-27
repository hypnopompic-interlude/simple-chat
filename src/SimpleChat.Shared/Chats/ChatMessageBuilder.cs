namespace SimpleChat.Shared.Chats;

public class ChatMessageBuilder : IMessageBuilder
{
    private string _content = default!;
    public IMessageBuilder WithContent(string content) 
    { 
        _content = content; 
        return this;
    }
 
    private string _prefix = default!;

    public IMessageBuilder WithPrefix(string prefix)
    {
        _prefix = prefix;
        return this;
    }

    public Result<string> Build()
    {
        if (ValidatePrefix(_prefix) is false || ValidateContent(_content) is false)
        {
            return Error.ConditionNotMet;
        }
        return string.Format("{0} : {1}", _prefix, _content);
    }

    private bool ValidateContent(string content) => !string.IsNullOrWhiteSpace(content);
    private bool ValidatePrefix(string content) => !string.IsNullOrWhiteSpace(content);
}
