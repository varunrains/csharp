using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCryptoHolders.Models
{
    public class WhaleAlert
    {
        public Guid Id { get; set; }
        public string Cursor { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string JsonDetail { get; set; }

        public DateTimeOffset LastUpdatedDate { get; set; }
    }
}
