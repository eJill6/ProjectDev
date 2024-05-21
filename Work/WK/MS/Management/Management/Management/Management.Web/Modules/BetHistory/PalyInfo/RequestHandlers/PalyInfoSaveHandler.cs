using Serenity.Services;
using MyRequest = Serenity.Services.SaveRequest<Management.BetHistory.PalyInfoRow>;
using MyResponse = Serenity.Services.SaveResponse;
using MyRow = Management.BetHistory.PalyInfoRow;

namespace Management.BetHistory
{
    public interface IPalyInfoSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> {}

    public class PalyInfoSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, IPalyInfoSaveHandler
    {
        public PalyInfoSaveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}