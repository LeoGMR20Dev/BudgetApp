namespace BudgetAppAPI.DTOs.BudgetTransactions
{
    public class BudgetTransactionForList
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
    }
}
