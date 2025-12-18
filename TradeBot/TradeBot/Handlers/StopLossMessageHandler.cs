using System;
using System.Collections.Generic;
using System.Text;
using TradeBot.Helpers.MagicStrings;
using TradeBot.Interfaces;
using TradeBot.Models;

namespace TradeBot.Handlers
{
    internal class StopLossMessageHandler : IMessageHandler
    {
        public bool CanHandle(string message)
            => message.StartsWith(BotCommands.SET_STOP_LOSS, StringComparison.OrdinalIgnoreCase) == true;

        public Task HandleMessageAsync(string message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
