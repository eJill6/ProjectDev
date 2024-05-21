using Serenity.Services;
using MyRequest = Serenity.Services.RetrieveRequest;
using MyResponse = Serenity.Services.RetrieveResponse<Management.SystemSettings.LotteryInfoRow>;
using MyRow = Management.SystemSettings.LotteryInfoRow;

namespace Management.SystemSettings
{
    public interface ILotteryInfoRetrieveHandler : IRetrieveHandler<MyRow, MyRequest, MyResponse> {}

    public class LotteryInfoRetrieveHandler : RetrieveRequestHandler<MyRow, MyRequest, MyResponse>, ILotteryInfoRetrieveHandler
    {
        public LotteryInfoRetrieveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}