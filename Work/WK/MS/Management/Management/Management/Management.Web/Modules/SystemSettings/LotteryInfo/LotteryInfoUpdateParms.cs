using Serenity.Services;
using System.Collections.Generic;
using MyRow = Management.SystemSettings.LotteryInfoRow;
namespace Management.Web.Modules.SystemSettings.LotteryInfo
{
    //這邊一定要繼承ServiceRequest從Grid裡的參數才會帶來Endpoint
    public class LotteryInfoUpdateParam : ServiceRequest
    {
        public List<UpdateLotteryInfoRequest> items { get; set; }
    }
}
