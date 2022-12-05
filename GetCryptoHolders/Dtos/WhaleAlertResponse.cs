using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCryptoHolders.Dtos
{
    public class WhaleAlertResponse
    {
        public string result { get; set; }

        public string cursor { get; set; }

        public int count { get; set; }
        public List<Transactions> transactions { get; set; }
    }

    public class Transactions
    {
        public string blockchain { get; set; }

        public string symbol { get; set; }

        public string transaction_type { get; set; }

        public double amount_usd { get; set; }

        public FromAddress from { get; set; }

        public ToAddress to { get; set; }

    }

    public class FromAddress
    {
        public string owner_type { get; set; }
    }

    public class ToAddress
    {
        public string owner_type { get; set; }
    }


}
