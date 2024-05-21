using AutoMapper;
using MS.Core.MM.Models.Wallets;
using MS.Core.MMModel.Models.Wallet;
using MS.Core.Models.Models;

namespace MMService.Infrastructure.AutoMapper
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<ResWalletInfo, WalletInfoViewModel>();
            CreateMap<ResIncomeInfo, IncomeInfoViewModel>();
            CreateMap<ResExpenseInfo, ExpenseInfoViewModel>();
            CreateMap<PageResultModel<ResIncomeInfo>, IncomeSummaryViewModel>();
            CreateMap<PageResultModel<ResExpenseInfo>, ExpenseSummaryViewModel>();
        }
    }
}
