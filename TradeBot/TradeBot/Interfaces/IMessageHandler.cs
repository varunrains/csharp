using TradeBot.Models;

namespace TradeBot.Interfaces
{
    public interface IMessageHandler
    {
        bool CanHandle(string message);
        Task HandleMessageAsync(string message, CancellationToken cancellationToken);
    }
}
