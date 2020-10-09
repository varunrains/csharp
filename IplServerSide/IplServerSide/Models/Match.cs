namespace IplServerSide.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Match
    {
        public int MatchId { get; set; }

        public DateTimeOffset MatchDateTime { get; set; }

        public int TeamIdA { get; set; }

        public int TeamIdB { get; set; }
        public string Place { get; set; }

        public int? Result { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }

        public virtual Team TeamA { get; set; }

        public virtual Team TeamB { get; set; }
    }
}
