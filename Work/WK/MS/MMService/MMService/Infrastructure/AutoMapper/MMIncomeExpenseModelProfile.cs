using AutoMapper;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MMModel.Models.AdminBooking;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminPostTransaction;

namespace MMService.Infrastructure.AutoMapper
{
    public class MMIncomeExpenseModelProfile : Profile
    {
        public MMIncomeExpenseModelProfile()
        {
            CreateMap<MMIncomeExpenseModel, AdminPostTransactionList>()
                .ForMember(x => x.IncomeExpenseId, opt => { opt.MapFrom(y => y.Id); })
                .ForMember(x => x.Id, opt => { opt.MapFrom(y => y.SourceId); });
            CreateMap<MMIncomeExpenseModel, AdminIncomeList>();
            CreateMap<MMIncomeExpenseModel, AdminIncomeDetail>();
            CreateMap<MMBooking, AdminBookingList>();
        }
    }
}
