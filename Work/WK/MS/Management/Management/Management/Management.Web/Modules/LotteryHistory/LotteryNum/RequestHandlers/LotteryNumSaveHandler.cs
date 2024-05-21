using Serenity.Services;
using MyRequest = Serenity.Services.SaveRequest<Management.LotteryHistory.LotteryNumRow>;
using MyResponse = Serenity.Services.SaveResponse;
using MyRow = Management.LotteryHistory.LotteryNumRow;

namespace Management.LotteryHistory
{
    public interface ILotteryNumSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> {}

    public class LotteryNumSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, ILotteryNumSaveHandler
    {
        public LotteryNumSaveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}