using AutoMapper;
using BudgetAppAPI.Constants;
using BudgetAppAPI.DTOs.BudgetTransactions;
using BudgetAppAPI.Interfaces.Repositories;
using BudgetAppAPI.Interfaces.Services;
using BudgetAppAPI.Models;

namespace BudgetAppAPI.Services
{
    public class BudgetService : IBudgetService<BudgetTransactionForList, BudgetDataDto, AddBudgetTransaction>
    {
        private readonly IBudgetRepository<BudgetTransaction> _budgetRepository;
        private IMapper _mapper;

        public BudgetService(IBudgetRepository<BudgetTransaction> budgetRepository,
            IMapper mapper)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
        }

        public async Task<BudgetDataDto> GetBudgetData()
        {
            var data = new BudgetDataDto();

            var expenses = await _budgetRepository.GetExpenses();
            var incomes = await _budgetRepository.GetIncomes();

            data.Incomes = incomes.Select(inc => _mapper.Map<BudgetDataIncomeDto>(inc)).ToList();
            data.Expenses = expenses.Select(exp => _mapper.Map<BudgetDataExpenseDto>(exp)).ToList();

            var totalExpenses = Math.Round(data.Expenses.Sum(exp => exp.Amount), 2, MidpointRounding.AwayFromZero);
            var totalIncomes = Math.Round(data.Incomes.Sum(inc => inc.Amount), 2, MidpointRounding.AwayFromZero);

            data.TotalExpenses = totalExpenses;
            data.TotalIncomes = totalIncomes;
            data.AvailableBudget = totalIncomes - totalExpenses;

            var isIncomesGreaterThanZero = totalIncomes >= 0;

            foreach (var exp in data.Expenses)
            {
                exp.Percentage = isIncomesGreaterThanZero ?
                    Math.Round(exp.Amount / totalIncomes, 2, MidpointRounding.AwayFromZero) : 0;
            }

            data.TotalExpensesPercentage = isIncomesGreaterThanZero ?
                Math.Round(totalExpenses / totalIncomes, 2, MidpointRounding.AwayFromZero) : 0;

            return data;
        }

        public async Task<IEnumerable<BudgetTransactionForList>> GetExpenses()
        {
            var budgetTransactions = await _budgetRepository.GetExpenses();

            return budgetTransactions.Select(bt => _mapper.Map<BudgetTransactionForList>(bt)).ToList();
        }

        public async Task<IEnumerable<BudgetTransactionForList>> GetIncomes()
        {
            var budgetTransactions = await _budgetRepository.GetIncomes();

            return budgetTransactions.Select(bt => _mapper.Map<BudgetTransactionForList>(bt)).ToList();
        }

        public async Task<BudgetTransactionForList?> GetTransactionById(Guid id)
        {
            var budgetTransaction = await _budgetRepository.GetTransaction(id);

            return _mapper.Map<BudgetTransactionForList?>(budgetTransaction);
        }

        public async Task<BudgetTransactionForList> AddTransaction(AddBudgetTransaction entity)
        {
            var budgetTransaction = _mapper.Map<BudgetTransaction>(entity);

            var totalIncomes = (await _budgetRepository.GetIncomes()).Sum(x => x.Amount);
            var totalExpenses = (await _budgetRepository.GetExpenses()).Sum(x => x.Amount);
            var totalBudget = totalIncomes - totalExpenses;
            var isExpense = budgetTransaction.Type == TransactionType.EXPENSE;

            if (isExpense && totalBudget <= 0)
            {
                throw new BadHttpRequestException("You can't register a expense if you don't have any money in your budget");
            }

            if (isExpense && totalBudget < budgetTransaction.Amount)
            {
                throw new BadHttpRequestException("The entered amount of money can't be greater than the budget");
            }

            await _budgetRepository.AddTransaction(budgetTransaction);
            await _budgetRepository.SaveAsync();

            var budgetTransactionDto = _mapper.Map<BudgetTransactionForList>(budgetTransaction);

            return budgetTransactionDto;
        }

        public async Task<bool> DeleteTransaction(Guid id)
        {
            var budgetTransaction = await _budgetRepository.GetTransaction(id);

            if (budgetTransaction == null)
            {
                return false;
            }

            _budgetRepository.DeleteTransaction(budgetTransaction);
            await _budgetRepository.SaveAsync();

            return true;
        }
    }
}
