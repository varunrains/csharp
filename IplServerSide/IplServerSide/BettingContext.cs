using IplServerSide.ModelConfigurations;
using IplServerSide.Models;
using System.Data.Entity;

namespace IplServerSide
{
    public partial class BettingContext : DbContext
    {
        public BettingContext()
            : base("name=BettingContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BetConfiguration());
            modelBuilder.Configurations.Add(new MatchConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new TeamConfiguration());
        }
    }
}
