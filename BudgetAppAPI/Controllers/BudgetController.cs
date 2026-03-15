using BudgetAppAPI.DTOs.BudgetTransactions;
using BudgetAppAPI.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : Controller
    {
        private readonly IBudgetService<BudgetTransactionForList, BudgetDataDto, AddBudgetTransaction> _budgetService;

        public BudgetController(IBudgetService<BudgetTransactionForList, BudgetDataDto, AddBudgetTransaction> budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpGet("getBudgetData")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBudgetData()
        {
            var result = await _budgetService.GetBudgetData();

            return result != null ? Ok(result) : NoContent();
        }

        [HttpGet("incomes")]
        [AllowAnonymous]
        public async Task<IActionResult> GetIncomes()
        {
            var result = await _budgetService.GetIncomes();

            return result.Count() == 0 ? NoContent() : Ok(result);
        }


        [HttpGet("expenses")]
        [AllowAnonymous]
        public async Task<IActionResult> GetExpenses()
        {
            var result = await _budgetService.GetExpenses();

            return result.Count() == 0 ? NoContent() : Ok(result);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BudgetTransactionForList>> GetTransactionById([FromRoute] Guid id)
        {
            var result = await _budgetService.GetTransactionById(id);

            return result == null ? NoContent() : Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddTransaction([FromBody] AddBudgetTransaction transaction)
        {
            var result = await _budgetService.AddTransaction(transaction);

            return CreatedAtAction(nameof(GetTransactionById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            var result = await _budgetService.DeleteTransaction(id);

            return result ? NoContent() : NotFound();
        }
    }
}
