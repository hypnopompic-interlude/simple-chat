namespace SimpleChat.Server.Authentication;

public class JwtBearerSettings
{
    public const string Section = "JwtBearerSettings";

    public required string SecretKey { get; set; }
    public int ExpiresSec { get; set; }
}