namespace TradeBot.Configuration;

public class TelegramBotOptions
{
    public const string SectionName = "TelegramBot";
    
    public string BotToken { get; set; } = string.Empty;
    public List<long> AllowedUserIds { get; set; } = [];
    public List<string> AllowedUsernames { get; set; } = [];
}
