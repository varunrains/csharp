using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using IplServerSide.Models;

namespace IplServerSide.ModelConfigurations
{
    public class TeamConfiguration:EntityTypeConfiguration<Team>
    {
        public TeamConfiguration()
        {
            ToTable("Teams");
            HasKey(t => t.TeamId);
            Property(t => t.TeamName).IsRequired().HasMaxLength(256);
            Property(t => t.TeamShortName).IsRequired().HasMaxLength(8);

            //HasMany(t => t.Matches)
            //    .WithRequired(a => a.TeamA)
            //    .HasForeignKey(a => a.TeamIdA);

            //HasMany(t => t.Matches)
            //    .WithRequired(a => a.TeamB)
            //    .HasForeignKey(a => a.TeamIdB);
        }
    }
}