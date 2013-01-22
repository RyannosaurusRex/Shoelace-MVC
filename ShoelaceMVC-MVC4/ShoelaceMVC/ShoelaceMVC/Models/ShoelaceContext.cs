using System.Data.Entity;

namespace ShoelaceMVC.Models
{
    public class ShoelaceContext : DbContext
    {
        public ShoelaceContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}