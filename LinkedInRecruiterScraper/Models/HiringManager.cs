using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInRecruiterScraper.Models
{
    public record HiringManager
    {
        public int Id { get; set; }

        public required string HiringManagerLink { get; set; }

        public required string ConnectionDegree { get; set; }
    }
}
