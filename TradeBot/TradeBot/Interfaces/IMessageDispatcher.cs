using System;
using System.Collections.Generic;
using System.Text;
using TradeBot.Models;

namespace TradeBot.Interfaces
{
    public interface IMessageDispatcher
    {
        Task DispatchAsync(string message, CancellationToken ct);
    }
}
