using Serenity.Services;
using MyRequest = Serenity.Services.ListRequest;
using MyResponse = Serenity.Services.ListResponse<Management.SystemSettings.LotteryInfoRow>;
using MyRow = Management.SystemSettings.LotteryInfoRow;

namespace Management.SystemSettings
{
    public interface ILotteryInfoListHandler : IListHandler<MyRow, MyRequest, MyResponse> {}

    public class LotteryInfoListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, ILotteryInfoListHandler
    {
        public LotteryInfoListHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}