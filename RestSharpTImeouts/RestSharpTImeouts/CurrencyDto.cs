using System;
using System.Collections.Generic;
using System.Text;

namespace RestSharpTImeouts
{
    public class CurrencyDto
    {
        public Guid CurrencyId { get; set; }

        /// <summary>
        /// ISOCode of the Currency
        /// </summary>
        public string ISOCode { get; set; }

        /// <summary>
        /// Name of the Currency
        /// </summary>
        public string Name { get; set; }

    }
}
