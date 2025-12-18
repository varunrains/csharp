using TradeBot.Interfaces;
using TradeBot.Models;

namespace TradeBot.CommonHandler
{
    internal class MessageDispatcher : IMessageDispatcher
    {
        private readonly IEnumerable<IMessageHandler> _handlers;

        public MessageDispatcher(IEnumerable<IMessageHandler> handlers)
        {
            _handlers = handlers;
        }

        public async Task DispatchAsync(string message, CancellationToken ct)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(message))
                {
                    await handler.HandleMessageAsync(message, ct);
                }
            }
        }
    }
}
