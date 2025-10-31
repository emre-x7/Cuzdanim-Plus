using AutoMapper;
using Cuzdanim.Application.Features.Accounts.DTOs;
using Cuzdanim.Application.Features.Budgets.DTOs;
using Cuzdanim.Application.Features.Categories.DTOs;
using Cuzdanim.Application.Features.Goals.DTOs;
using Cuzdanim.Application.Features.Transactions.DTOs;
using Cuzdanim.Application.Features.Users.DTOs;
using Cuzdanim.Domain.Entities;

namespace Cuzdanim.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User -> UserDto
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.PreferredCurrency, opt => opt.MapFrom(src => src.PreferredCurrency.ToString()));

        // Account -> AccountDto
        CreateMap<Account, AccountDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Balance.Currency.ToString()));

        // Transaction -> TransactionDto
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Amount.Currency.ToString()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Tags) ? new List<string>() : src.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()));

        // Budget -> BudgetDto
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.CategoryIcon, opt => opt.MapFrom(src => src.Category.Icon)) 
            .ForMember(dest => dest.CategoryColor, opt => opt.MapFrom(src => src.Category.Color)) 
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Amount.Currency.ToString()))
            .ForMember(dest => dest.PeriodStartDate, opt => opt.MapFrom(src => src.Period.StartDate))
            .ForMember(dest => dest.PeriodEndDate, opt => opt.MapFrom(src => src.Period.EndDate))
            .ForMember(dest => dest.Spent, opt => opt.Ignore())
            .ForMember(dest => dest.Remaining, opt => opt.Ignore())
            .ForMember(dest => dest.PercentageUsed, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());

        // Goal -> GoalDto
        CreateMap<Goal, GoalDto>()
            .ForMember(dest => dest.TargetAmount, opt => opt.MapFrom(src => src.TargetAmount.Amount))
            .ForMember(dest => dest.CurrentAmount, opt => opt.MapFrom(src => src.CurrentAmount.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TargetAmount.Currency.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => src.ProgressPercentage))
            .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => src.DaysRemaining))
            .ForMember(dest => dest.RemainingAmount, opt => opt.MapFrom(src => src.RemainingAmount.Amount));

        // Category -> CategoryDto
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
    }
}
