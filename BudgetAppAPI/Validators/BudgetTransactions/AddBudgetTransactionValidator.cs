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
        }
    }
}
