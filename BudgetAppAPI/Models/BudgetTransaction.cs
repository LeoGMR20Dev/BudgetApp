using BudgetAppAPI.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetAppAPI.Models
{
    [Table("BudgetTransactions")]
    public class BudgetTransaction
    {
        [Key]
        [Column("budget_transaction_id")]
        public Guid BudgetTransactionId { get; set; }

        [Column("description")]
        [MaxLength(255)]
        public string Description { get; set; } = null!;

        [Column("amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column("type")]
        [AllowedValues(
            TransactionType.INCOME,
            TransactionType.EXPENSE,
            ErrorMessage = $"Only '{TransactionType.INCOME}' and '{TransactionType.EXPENSE}' value are accepted"
            )]
        public string Type { get; set; } = null!;

        [Column("created_at", TypeName = "datetimeoffset")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at", TypeName = "datetimeoffset")]
        public DateTimeOffset UpdatedAt { get; set; }

        [Column("state")]
        public bool State { get; set; } = true;
    }
}
