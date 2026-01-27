namespace SimpleChat.Shared;

public record Error(string Code, string Message)
{
    public static Error From(Error[] errors)
    {
        var codes = string.Concat(errors.Select(x => x.Code));
        var messages = string.Concat(errors.Select(x => x.Message));
        return new Error(codes, messages);
    }

    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NotFound = new("Error.NotFound", "Entity was not found.");

    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public static readonly Error ConditionNotMet = new("Error.ConditionNotMet", "The specified condition was not met.");

    public static readonly Error InvalidOperation = new("Error.InvalidOperation", "InvalidOperation: The specified condition was not met. Bad parameters list.");
}
