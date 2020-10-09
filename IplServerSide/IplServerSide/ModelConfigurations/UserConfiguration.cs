using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using IplServerSide.Models;

namespace IplServerSide.ModelConfigurations
{
    public class UserConfiguration:EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            HasKey(u => u.UserId);
            Property(u => u.PassKey).HasMaxLength(24).IsRequired();
            Property(u => u.UserName).HasMaxLength(24).IsRequired();

            HasMany(u => u.Bets)
                .WithRequired(b => b.User);
        }
    }
}