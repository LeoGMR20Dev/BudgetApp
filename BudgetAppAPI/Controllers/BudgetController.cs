using BudgetAppAPI.DTOs.BudgetTransactions;
using BudgetAppAPI.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BudgetAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : Controller
    {
        private readonly IBudgetService<BudgetTransactionForList, BudgetDataDto, AddBudgetTransaction> _budgetService;
        private readonly IValidator<AddBudgetTransaction> _addBudgetTransactionValidator;

        public BudgetController(IBudgetService<BudgetTransactionForList, BudgetDataDto, AddBudgetTransaction> budgetService,
            IValidator<AddBudgetTransaction> addBudgetTransactionValidator)
        {
            _budgetService = budgetService;
            _addBudgetTransactionValidator = addBudgetTransactionValidator;
        }

        [HttpGet("getBudgetData")]
        public async Task<IActionResult> GetBudgetData()
        {
            var result = await _budgetService.GetBudgetData();

            return result != null ? Ok(result) : NoContent();
        }

        [HttpGet("incomes")]
        public async Task<IActionResult> GetIncomes()
        {
            var result = await _budgetService.GetIncomes();

            return result.Count() == 0 ? NoContent() : Ok(result);
        }


        [HttpGet("expenses")]
        public async Task<IActionResult> GetExpenses()
        {
            var result = await _budgetService.GetExpenses();

            return result.Count() == 0 ? NoContent() : Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetTransactionForList>> GetTransactionById([FromRoute] Guid id)
        {
            var result = await _budgetService.GetTransactionById(id);

            return result == null ? NoContent() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] AddBudgetTransaction transaction)
        {
            var validationResult = await _addBudgetTransactionValidator.ValidateAsync(transaction);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var result = await _budgetService.AddTransaction(transaction);

            return CreatedAtAction(nameof(GetTransactionById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            var result = await _budgetService.DeleteTransaction(id);

            return result ? NoContent() : NotFound();
        }
    }
}
