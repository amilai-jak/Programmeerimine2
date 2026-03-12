using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IMonthlyStateRepository
    {
        Task<MonthlyState> GetByIdAsync(int id);
        Task SaveAsync(MonthlyState entity);
        Task DeleteAsync(MonthlyState entity);
    }
}
