using BudgetAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetAppAPI.Contexts
{
    public class BudgetContext : DbContext
    {
        public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
        {

        }

        public DbSet<BudgetTransaction> BudgetTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
