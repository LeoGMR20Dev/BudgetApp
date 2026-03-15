namespace BudgetAppAPI.DTOs.BudgetTransactions
{
    public class AddBudgetTransaction
    {
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
    }
}
