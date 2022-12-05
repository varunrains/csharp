using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetCryptoHolders.Dtos
{
    public class HolderDto
    {
        public string TokenName { get; set; }
        public string Date { get; set; }

        public string NumberOfTokenHolders { get; set; }
    }
}
