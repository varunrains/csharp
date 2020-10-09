namespace IplServerSide.Models
{
    using System.Collections.Generic;

    public partial class Team
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Team()
        {
            MatchesOfA = new List<Match>();
            MatchesOfB = new List<Match>();
        }

        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamShortName { get; set; }
        public ICollection<Match> MatchesOfA { get; set; }
        public ICollection<Match> MatchesOfB { get; set; }
    }
}
