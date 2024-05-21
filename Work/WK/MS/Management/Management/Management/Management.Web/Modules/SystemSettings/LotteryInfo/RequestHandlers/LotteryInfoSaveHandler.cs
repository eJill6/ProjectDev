using Serenity.Services;
using MyRequest = Serenity.Services.SaveRequest<Management.SystemSettings.LotteryInfoRow>;
using MyResponse = Serenity.Services.SaveResponse;
using MyRow = Management.SystemSettings.LotteryInfoRow;

namespace Management.SystemSettings
{
    public interface ILotteryInfoSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> {}

    public class LotteryInfoSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, ILotteryInfoSaveHandler
    {
        public LotteryInfoSaveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}