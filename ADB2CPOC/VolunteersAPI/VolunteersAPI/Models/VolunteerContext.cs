using Microsoft.EntityFrameworkCore;

namespace VolunteerListAPI.Models
{
    public class VolunteerContext : DbContext
    {
        public VolunteerContext(DbContextOptions<VolunteerContext> options)
            : base(options)
        {

        }

        public DbSet<VolunteerItem> VolunteerItems { get; set; }
    }
}
