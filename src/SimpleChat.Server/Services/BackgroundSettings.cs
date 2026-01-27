namespace SimpleChat.Server.Services;

public class BackgroundSettings
{
    public const string Section = "BackgroundSettings";
    public int PeriodicNotificationTimeSpanSec { get; set; } = 60;
}
