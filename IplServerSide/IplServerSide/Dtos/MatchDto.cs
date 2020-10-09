using System;

namespace IplServerSide.Dtos
{
    public class MatchDto
    {
        public int MatchId { get; set; }
        public DateTimeOffset MatchDateTime { get; set; }
        public int TeamIdA { get; set; }
        public int TeamIdB { get; set; }

        public string TeamAShortName { get; set; }
        public string TeamBShortName { get; set; }
        public string Place { get; set; }
        public int? Result { get; set; }
        public bool IsMatchAbandoned { get; set; }
    }
}