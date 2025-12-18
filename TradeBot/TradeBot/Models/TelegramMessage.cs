using System;
using System.Collections.Generic;
using System.Text;

namespace TradeBot.Models
{
    public class TelegramMessage
    {
        public long MessageId { get; set; }
        public TelegramChat Chat { get; set; } = default!;
        public string? Text { get; set; }
        public DateTime Date { get; set; }
    }

    public class TelegramChat
    {
        public long Id { get; set; }
        public string? Type { get; set; }
    }
}
