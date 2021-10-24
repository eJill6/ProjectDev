using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Model.ViewModel.LotteryCenter
{
    public class LotteryCenterResultModel
    {
        public LotteryCenterResultModel()
        {

        }

        public List<GameTypeInfoResultModel> All { get; set; }

        public List<LotteryInfoResultModel> Recommend { get; set; }
    }

    public class GameTypeInfoResultModel
    {
        public GameTypeInfoResultModel()
        {

        }

        public int GameTypeID { get; set; }

        public string GameType { get; set; }

        public string TypeURL { get; set; }

        public int GroupPriority { get; set; }

        public int APPGroupPriority { get; set; }

        public bool IsWebUsed { get; set; }

        public bool IsAppUsed { get; set; }

        public bool IsTrendShow { get; set; }

        public List<LotteryInfoResultModel> LotteryInfo { get; set; }
    }

    public class LotteryInfoResultModel
    {
        public LotteryInfoResultModel()
        {

        }

        /// <summary>
        /// 彩种ID
        /// </summary>
        public int LotteryID { get; set; }

        /// <summary>
        /// 彩种类型
        /// </summary>
        public string LotteryType { get; set; }

        public string TypeURL { get; set; }

        public int GameTypeID { get; set; }

        public string OfficialLotteryUrl { get; set; }

        public string NumberTrendUrl { get; set; }

        public int Priority { get; set; }

        public int GroupPriority { get; set; }

        public int APPPriority { get; set; }

        public int HotNew { get; set; }

        public int UserType { get; set; }

        public string Notice { get; set; }

        public int MaxBonusMoney { get; set; }

        public int RecommendSort { get; set; }

        //20210219 因為此次新增雙贏彩票，雙贏彩票屬於第三方跟六合彩那些AMD彩種一起放在「更多遊戲」裡面，
        //但 App Team原來是用 GameTypeInfo裡面的 IsTrendShow 判斷內容List是否需要出現走勢圖
        //第三方彩票沒有走勢圖，所以會變成需要在 List內的Model加上 IsTrendShow ，讓App Team自己去看List中的遊戲是否要顯示走勢圖
        public bool IsTrendShow { get; set; }

        public string LogoImageUrl { get; set; } = string.Empty;

        public bool IsThirdPartyGame 
        { 
            get
            {
                return (!ThirdPartyProductCode.IsNullOrEmpty() || !ThirdPartySubGameCode.IsNullOrEmpty());
            }

            set{ } 
        }

        public string ThirdPartyProductCode
        {
            get
            {
                if (ThirdPartySubGameCodes.GetSingle(TypeURL) != null)
                {
                    return ThirdPartySubGameCodes.GetSingle(TypeURL).PlatformProduct.Value;
                }
                else if (PlatformProduct.GetSingle(TypeURL) != null)
                {
                    return PlatformProduct.GetSingle(TypeURL).Value;
                }
                else
                {
                    return string.Empty;
                }
            }

            set { }
        }

        public string ThirdPartySubGameCode
        {
            get
            {
                var subGameCode = ThirdPartySubGameCodes.GetSingle(TypeURL);

                if (subGameCode != null && !subGameCode.IsPrimary)
                {
                    return TypeURL;
                }

                return string.Empty;
            }

            set { }
        }

        public string ThirdPartyLoadingImageUrl { get; set; } = string.Empty;
    }
}
