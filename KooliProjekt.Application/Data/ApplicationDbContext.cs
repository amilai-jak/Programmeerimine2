using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AssetClass> AssetClasses { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<MonthlyState> MonthlyStates { get; set; }
        public DbSet<MonthlyHolding> MonthlyHoldings { get; set; }
    }
}
