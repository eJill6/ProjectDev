using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.MiseOrder;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Helpers
{
    public class MsHelpers
    {
        public static List<SelectListItem> GetMsLotterySelectListItem()
        {
            var lottery = MiseOrderGameId.GetAll()
                .Where(x => x.Product == PlatformProduct.Lottery)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Value
                })
                .ToList();

            lottery.Insert(0, new SelectListItem
            {
                Text = "全部",
                Value = null,
                Selected = true
            });

            return lottery;
        }
        public static string GetLotteryName(int id)
        {
            string result = MiseOrderGameId.GetAll()
                .FirstOrDefault(x => x.Product == PlatformProduct.Lottery && x.Value == id.ToString())?.Name ?? string.Empty;
            return result;
        }
    }
}
