using AutoMapper;
using BudgetAppAPI.DTOs.BudgetTransactions;
using BudgetAppAPI.Models;

namespace BudgetAppAPI.AutoMappers
{
    public class BudgetTransactionMappingProfile : Profile
    {
        public BudgetTransactionMappingProfile()
        {
            CreateMap<AddBudgetTransaction, BudgetTransaction>();
            CreateMap<BudgetTransaction, BudgetTransactionForList>().ForMember(dto => dto.Id,
                m => m.MapFrom(bt => bt.BudgetTransactionId));

            CreateMap<BudgetTransaction, BudgetDataTransactionDto>()
                .ForMember(dto => dto.Id, m => m.MapFrom(bt => bt.BudgetTransactionId))
                .ForMember(dest => dest.Amount, m => m.MapFrom(bt => Math.Round(bt.Amount, 2, MidpointRounding.AwayFromZero)));
            CreateMap<BudgetTransaction, BudgetDataIncomeDto>()
                .IncludeBase<BudgetTransaction, BudgetDataTransactionDto>();

            CreateMap<BudgetTransaction, BudgetDataExpenseDto>()
                .IncludeBase<BudgetTransaction, BudgetDataTransactionDto>()
                .ForMember(dest => dest.Percentage,
                    opt => opt.Ignore());
        }
    }
}
