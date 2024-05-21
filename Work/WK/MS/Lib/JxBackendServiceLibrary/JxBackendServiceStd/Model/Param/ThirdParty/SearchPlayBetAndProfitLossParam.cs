using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class BaseSearchPlayBetAndProfitLossParam : BasePagingRequestParam
    {
        public string ProductCode { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public PlatformProduct Product => PlatformProduct.GetSingle(ProductCode);
    }

    public class SearchTPGamePlayInfoParam : BaseSearchPlayBetAndProfitLossParam
    {
        public int? UserID { get; set; }

        /// <summary>
        /// 投注紀錄的查詢類型(因為WCF不支援用列舉型別，所以用int)
        /// </summary>
        public int PlayBetRecordQueryTypeValue { get; set; }

        /// <summary>
        /// 投注紀錄的查詢類型的值
        /// </summary>
        public string QueryTypeText { get; set; }

        /// <summary>
        /// 給Mobile API多參數篩選用
        /// </summary>
        public List<int> IsWins { get; set; }

        public int? IsWin { get; set; }
    }

    public abstract class CommonSearchTPGameProfitLossParam : BaseSearchPlayBetAndProfitLossParam
    {
        public abstract int? SearchUserID { get; }

        public abstract bool IsSearchVIPAgent { get; }

        public string ProfitLossType { get; set; }

        public bool IsCalculateStat { get; set; }
    }

    public class SearchTPGameProfitLossParam : CommonSearchTPGameProfitLossParam
    {
        public int UserID { get; set; }

        public override int? SearchUserID => UserID;

        public override bool IsSearchVIPAgent => false;
    }

    public class SearchPlatformUserProfitLossParam : CommonSearchTPGameProfitLossParam
    {
        public SearchPlatformUserProfitLossParam()
        {
            IsCalculateStat = false;
        }

        public int? UserID { get; set; }

        public override int? SearchUserID => UserID;

        public override bool IsSearchVIPAgent => false;
    }
}