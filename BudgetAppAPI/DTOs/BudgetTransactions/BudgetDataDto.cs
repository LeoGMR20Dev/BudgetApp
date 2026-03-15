namespace BudgetAppAPI.DTOs.BudgetTransactions
{
    public class BudgetDataDto
    {
        public decimal TotalIncomes { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalExpensesPercentage { get; set; }
        public decimal AvailableBudget { get; set; }
        public List<BudgetDataIncomeDto> Incomes { get; set; } = new List<BudgetDataIncomeDto>();
        public List<BudgetDataExpenseDto> Expenses { get; set; } = new List<BudgetDataExpenseDto>();
    }

    public class BudgetDataTransactionDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!;
    }

    public class BudgetDataIncomeDto : BudgetDataTransactionDto { }

    public class BudgetDataExpenseDto : BudgetDataTransactionDto { 
        public decimal Percentage { get; set; }
    }
}
