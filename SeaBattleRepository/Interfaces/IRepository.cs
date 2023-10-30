namespace SeaBattleRepository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        IEnumerable<T> GetByCondition(Func<T, bool> condition);
        Task SaveAsync();
    }
}
