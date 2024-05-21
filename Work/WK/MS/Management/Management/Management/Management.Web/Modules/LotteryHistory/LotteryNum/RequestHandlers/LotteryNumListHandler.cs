using Serenity.Services;
using MyRequest = Serenity.Services.ListRequest;
using MyResponse = Serenity.Services.ListResponse<Management.LotteryHistory.LotteryNumRow>;
using MyRow = Management.LotteryHistory.LotteryNumRow;

namespace Management.LotteryHistory
{
    public interface ILotteryNumListHandler : IListHandler<MyRow, MyRequest, MyResponse> {}

    public class LotteryNumListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, ILotteryNumListHandler
    {
        public LotteryNumListHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}