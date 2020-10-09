using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using IplServerSide.Models;

namespace IplServerSide.ModelConfigurations
{
    public class MatchConfiguration:EntityTypeConfiguration<Match>
    {
        public MatchConfiguration()
        {
            ToTable("Matches");
            HasKey(m => m.MatchId);

            Property(m => m.MatchDateTime).IsRequired();
            Property(m => m.TeamIdA).IsRequired();
            Property(m => m.TeamIdB).IsRequired();
            Property(m => m.Place).IsRequired().HasMaxLength(256);


            HasRequired(m => m.TeamA)
                .WithMany(m => m.MatchesOfA)
                .HasForeignKey(x => x.TeamIdA);

            HasRequired(m => m.TeamB)
                .WithMany(m => m.MatchesOfB)
                .HasForeignKey(x => x.TeamIdB);
        }
    }
}