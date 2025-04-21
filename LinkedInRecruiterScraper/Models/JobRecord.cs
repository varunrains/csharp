using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInRecruiterScraper.Models
{
    public class JobRecord
    {
        public int Id { get; set; }
       
        public string Title { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string JobDescription { get; set; }
        public string JobUrl { get; set; }

        public string HiringManagerLink { get; set; }
        //public DateTime PostedDate { get; set; }
    }
}
