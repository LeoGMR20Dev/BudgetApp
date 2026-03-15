using BudgetAppAPI.Constants;
using BudgetAppAPI.Contexts;
using BudgetAppAPI.Interfaces.Repositories;
using BudgetAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetAppAPI.Repositories
{
    public class BudgetRepository : IBudgetRepository<BudgetTransaction>
    {
        private readonly BudgetContext _context;

        public BudgetRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BudgetTransaction>> GetIncomes() =>
            await _context.BudgetTransactions.Where(bt => bt.Type == TransactionType.INCOME && bt.State == true)
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync();

        public async Task<IEnumerable<BudgetTransaction>> GetExpenses() =>
            await _context.BudgetTransactions.Where(bt => bt.Type == TransactionType.EXPENSE && bt.State == true)
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync();

        public async Task<BudgetTransaction?> GetTransaction(Guid id) =>
            await _context.BudgetTransactions
            .FirstOrDefaultAsync(bt => bt.BudgetTransactionId == id);

        public async Task AddTransaction(BudgetTransaction entity)
        {
            await _context.BudgetTransactions.AddAsync(entity);
        }

        public void DeleteTransaction(BudgetTransaction entity)
        {
            _context.BudgetTransactions.Attach(entity);
            _context.BudgetTransactions.Entry(entity).Property(bt => bt.State).CurrentValue = false;
            _context.BudgetTransactions.Entry(entity).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
