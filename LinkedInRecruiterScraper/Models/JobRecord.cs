using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInRecruiterScraper.Models
{
    public record JobRecord
    {
        public int Id { get; set; }
       
        public required string Title { get; set; }
        public required string Company { get; set; }
        public required string Location { get; set; }
        public required string JobDescription { get; set; }
        public required string JobUrl { get; set; }

        public string? HiringManagerLink { get; set; }
        //public DateTime PostedDate { get; set; }
    }
}
