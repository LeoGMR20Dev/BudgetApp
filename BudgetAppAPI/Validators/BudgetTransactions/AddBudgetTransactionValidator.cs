using BudgetAppAPI.Constants;
using BudgetAppAPI.DTOs.BudgetTransactions;
using FluentValidation;

namespace BudgetAppAPI.Validators.BudgetTransactions
{
    public class AddBudgetTransactionValidator : AbstractValidator<AddBudgetTransaction>
    {
        public AddBudgetTransactionValidator()
        {
            RuleFor(x => x.Amount)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("The amount is required")
                .GreaterThan(0).WithMessage("The amount has to be greater than zero")
                .LessThanOrEqualTo(1_000_000m).WithMessage("The amount can't be greater than 1.000.000");
            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The description is required")
                .Length(3, 50).WithMessage("The description must be between 3 and 50 characters")
                .Matches(@"^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9,. ]+$").WithMessage("The description can only have letters, numbers, ',', '.'");
            RuleFor(x => x.Type)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The type of the transaction is required")
                .Must(x => new[] { TransactionType.INCOME, TransactionType.EXPENSE }.Contains(x))
                .WithMessage("El tipo de transacción debe ser 'INCOME' o 'EXPENSE'.");
        }
    }
}
