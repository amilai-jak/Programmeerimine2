using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IMonthlyHoldingRepository
    {
        Task<MonthlyHolding> GetByIdAsync(int id);
        Task SaveAsync(MonthlyHolding entity);
        Task DeleteAsync(MonthlyHolding entity);
    }
}
