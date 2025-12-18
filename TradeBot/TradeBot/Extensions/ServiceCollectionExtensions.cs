using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TradeBot.BackgroundServices;
using TradeBot.CommonHandler;
using TradeBot.Configuration;
using TradeBot.Handlers;
using TradeBot.Interfaces;

namespace TradeBot.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure TelegramBot options
        services.Configure<TelegramBotOptions>(configuration.GetSection(TelegramBotOptions.SectionName));

        // Register TelegramBotClient
        var botToken = configuration.GetSection(TelegramBotOptions.SectionName).Get<TelegramBotOptions>()?.BotToken
            ?? throw new InvalidOperationException("TelegramBot:BotToken is not configured");
        
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));
        services.AddSingleton<IMessageDispatcher, MessageDispatcher>();
        services.AddTransient<IMessageHandler, StopLossMessageHandler>();

        // Register background services
        services.AddHostedService<TelegramPollingService>();

        return services;
    }
}
