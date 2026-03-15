namespace BudgetAppAPI.Interfaces.Repositories
{
    public interface IBudgetRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetIncomes();
        Task<IEnumerable<TEntity>> GetExpenses();
        Task<TEntity?> GetTransaction(Guid id);
        Task AddTransaction(TEntity entity);
        void DeleteTransaction(TEntity entity);
        Task SaveAsync();
    }
}
