using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class MonthlyHoldingRepository : BaseRepository<MonthlyHolding>, IMonthlyHoldingRepository
    {
        public MonthlyHoldingRepository(ApplicationDbContext dbContext) : 
            base(dbContext)
        {
        }

        public override async Task<MonthlyHolding> GetByIdAsync(int id)
        {
            return await DbContext
                .MonthlyHoldings
                .Include(mh => mh.Asset)
                .Include(mh => mh.MonthlyState)
                .Where(mh => mh.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
