using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Antlr.Runtime;
using IplServerSide.Models;

namespace IplServerSide.ModelConfigurations
{
    public class BetConfiguration:EntityTypeConfiguration<Bet>
    {
        public BetConfiguration()
        {
            ToTable("Bets");
            HasKey(b => b.BetId);
            Property(b => b.NetAmountWon).HasPrecision(6, 2);
            Property(b => b.BettingTeamId).IsRequired();
            Property(b => b.UserId).IsRequired();
            Property(b => b.BetAmount).IsRequired();
            Property(b => b.MatchId).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_MatchIdAndUserId", 1) {IsUnique = true}));

            Property(b => b.UserId).IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_MatchIdAndUserId", 2) {IsUnique = true}));

            HasRequired(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId);

            HasRequired(b => b.Match)
                .WithMany(b => b.Bets);

        }
    }
}