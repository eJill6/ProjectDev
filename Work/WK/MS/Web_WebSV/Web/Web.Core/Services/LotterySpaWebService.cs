using ControllerShareLib.Services;

namespace Web.Core.Services;

public class LotterySpaWebService : LotterySpaService
{
    public LotterySpaWebService()
    {
    }

    //Web沒有支援龍虎(包含)之後的彩種
    public override int[] GetSpecialRuleIds() => new int[] { 65, 66, 67, 68, 69, 70, 71, 72, 73 };
}