using System;
using System.Collections.Generic;
using System.Text;

namespace SharepointToAzure.Models
{
    public class FilesWithRepositoryInformation
    {
        public Guid FileId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RepositoryType { get; set; }
        public string SegmentationType { get; set; }
        public string LocationUrl { get; set; }
        public string FolderUrl { get; set; }
    }
}
