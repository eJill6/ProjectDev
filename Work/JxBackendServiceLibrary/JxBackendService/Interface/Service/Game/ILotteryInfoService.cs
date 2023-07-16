using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Game
{
    public interface ILotteryInfoService
    {
        Dictionary<int, string> GetLotteryTypeMap();

        Dictionary<int, string> GetPlayTypeNameMap();

        Dictionary<int, string> GetPlayTypeRadioNameMap();

        string GetCurrentLotteryNumMapKey(int lotteryID, string issueNo, int? userID);
    }
}