using Microsoft.EntityFrameworkCore;
using SeaBattleDB.DB;
using SeaBattleRepository.Interfaces;

namespace SeaBattleRepository.Implement
{
    public abstract class RepositoryBase<T> :
        IRepository<T> where T : class
    {
        protected readonly User29Context context;

        public RepositoryBase(User29Context context)
        {
            this.context = context;
        }

        public abstract Task CreateAsync(T entity);
        public abstract Task UpdateAsync(T entity);

        public async Task DeleteAsync(int id)
        {
            var delete = await context.Set<T>().FindAsync(id);
            if (delete == null)
            {
                await Task.CompletedTask;
                return;
            }
            context.Set<T>().Remove(delete);
        }

        public IEnumerable<T> GetByCondition(Func<T, bool> condition)
        {
            var array = context.Set<T>().AsNoTracking()
                .Where(condition).ToList();
            return array;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task SaveAsync()
        { 
            await context.SaveChangesAsync();
        }
    }
}
