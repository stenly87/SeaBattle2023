using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SeaBattleRepository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T> SearchEntryByConditionAsync(Expression<Func<T, bool>> condition);
        IEnumerable<T> GetByCondition(Func<T, bool> condition);
        Task SaveAsync();
    }
}
