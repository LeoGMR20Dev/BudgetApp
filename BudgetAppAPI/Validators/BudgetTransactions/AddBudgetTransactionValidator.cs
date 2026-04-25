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
                .NotNull().WithMessage("El monto es requerido")
                .GreaterThan(0).WithMessage("El monto debe ser mayor a cero")
                .LessThanOrEqualTo(1_000_000m).WithMessage("El monto no debe ser mayor a 1.000.000");
            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("La descripción es requerida")
                .Length(3, 50).WithMessage("La descripción debe tener entre 3 y 50 caracteres")
                .Matches(@"^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9,. ]+$").WithMessage("La descripción solo puede tener letras, números, ',', '.'");
            RuleFor(x => x.Type)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El tipo de transacción es requerido")
                .Must(x => new[] { TransactionType.INCOME, TransactionType.EXPENSE }.Contains(x))
                .WithMessage("El tipo de transacción debe ser 'INCOME' o 'EXPENSE'.");
        }
    }
}
