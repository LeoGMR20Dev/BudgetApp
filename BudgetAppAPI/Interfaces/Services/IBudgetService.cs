

namespace BudgetAppAPI.Interfaces.Services
{
    public interface IBudgetService<T, TData, TI>
    {
        Task<TData> GetBudgetData();
        Task<IEnumerable<T>> GetIncomes();
        Task<IEnumerable<T>> GetExpenses();
        Task<T?> GetTransactionById(Guid id);
        Task<T> AddTransaction(TI entity);
        Task<bool> DeleteTransaction(Guid id);
    }
}
