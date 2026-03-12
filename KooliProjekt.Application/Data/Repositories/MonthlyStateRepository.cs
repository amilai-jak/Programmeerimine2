namespace KooliProjekt.Application.Data.Repositories
{
    public class MonthlyStateRepository : BaseRepository<MonthlyState>, IMonthlyStateRepository
    {
        public MonthlyStateRepository(ApplicationDbContext dbContext) : 
            base(dbContext)
        {
        }
    }
}
