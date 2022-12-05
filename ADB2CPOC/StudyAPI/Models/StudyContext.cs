using Microsoft.EntityFrameworkCore;

namespace StudyListAPI.Models
{
    public class StudyContext : DbContext
    {
        public StudyContext(DbContextOptions<StudyContext> options)
            : base(options)
        {

        }

        public DbSet<StudyItem> StudyItems { get; set; }
    }
}
