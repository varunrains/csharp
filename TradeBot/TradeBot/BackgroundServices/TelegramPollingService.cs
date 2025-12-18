using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TradeBot.Configuration;
using TradeBot.Interfaces;

namespace TradeBot.BackgroundServices;

public class TelegramPollingService : BackgroundService
{
    private readonly ILogger<TelegramPollingService> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly TelegramBotOptions _options;

    public TelegramPollingService(
        ILogger<TelegramPollingService> logger, 
        ITelegramBotClient botClient,
        IOptions<TelegramBotOptions> options,
        IMessageDispatcher messageDispatcher)
    {
        _logger = logger;
        _botClient = botClient;
        _options = options.Value;
        _messageDispatcher = messageDispatcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TelegramPollingService is starting");

        // Verify bot connection before starting to receive updates
        try
        {
            var me = await _botClient.GetMe(stoppingToken);
            _logger.LogInformation("Bot connected: @{BotUsername} (ID: {BotId})", me.Username, me.Id);
        }
        catch (ApiRequestException ex)
        {
            _logger.LogError(ex, "Failed to connect to Telegram API. Verify your BotToken is correct. Error: {ErrorCode}", ex.ErrorCode);
            throw;
        }

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message],
            DropPendingUpdates = true
        };

        // Start the polling loop - this runs in the background
        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken
        );

        _logger.LogInformation("Bot is now listening for messages");

        // Keep the service running until cancellation is requested
        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException)
        {
            // Expected during graceful shutdown - no action needed
        }
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        if (message.Text is not { } messageText)
            return;

        var userId = message.From?.Id ?? 0;
        var username = message.From?.Username ?? "Unknown";
        var chatId = message.Chat.Id;

        // Check if user is authorized
        if (!IsUserAuthorized(userId, username))
        {
            _logger.LogWarning("Unauthorized access attempt from @{Username} (UserId: {UserId}, ChatId: {ChatId})", 
                username, userId, chatId);
            return;
        }

        _logger.LogInformation("Received message from @{Username} (UserId: {UserId}, ChatId: {ChatId}): {Message}", 
            username, userId, chatId, messageText);

        // Process message and send appropriate response
        var (success, responseMessage) = await ProcessMessageAsync(messageText, cancellationToken);
        
        await SendResponseAsync(botClient, chatId, success, responseMessage, cancellationToken);
    }

    private async Task<(bool Success, string Message)> ProcessMessageAsync(string messageText, CancellationToken cancellationToken)
    {
        try
        {
            await _messageDispatcher.DispatchAsync(messageText, cancellationToken);
            return (true, $"✅ Command received: {messageText}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message: {Message}", messageText);
            return (false, "❌ Error processing your command. Please try again.");
        }
    }

    private async Task SendResponseAsync(ITelegramBotClient botClient, long chatId, bool success, string message, CancellationToken cancellationToken)
    {
        try
        {
            await botClient.SendMessage(
                chatId: chatId,
                text: message,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send response to chat {ChatId}", chatId);
        }
    }

    private bool IsUserAuthorized(long userId, string? username)
    {
        // If no restrictions configured, allow all users
        if (_options.AllowedUserIds.Count == 0 && _options.AllowedUsernames.Count == 0)
            return true;

        // Check by user ID (preferred - IDs are immutable)
        if (_options.AllowedUserIds.Contains(userId))
            return true;

        // Check by username (case-insensitive, but usernames can change)
        if (!string.IsNullOrEmpty(username) && 
            _options.AllowedUsernames.Any(u => u.Equals(username, StringComparison.OrdinalIgnoreCase)))
            return true;

        return false;
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiEx => $"Telegram API Error [{apiEx.ErrorCode}]: {apiEx.Message}",
            HttpRequestException httpEx => $"Network Error: {httpEx.Message}",
            _ => exception.Message
        };

        _logger.LogError(exception, "Telegram Bot error: {ErrorMessage}", errorMessage);
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TelegramPollingService is stopping");
        await base.StopAsync(cancellationToken);
    }
}
