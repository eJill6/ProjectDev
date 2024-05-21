using JxBackendService.Model.Enums;
using System;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class ComputeBetBonusProfitLoss
    {
        public int UserID { get; set; }

        /// <summary>有效投注金額</summary>
        public decimal ProfitLossMoney { get; private set; }

        /// <summary>總投注金額</summary>
        public decimal AllBetMoney { get; private set; }

        public int BetResultType { get; set; }

        /// <summary>是否沒有抽水(預設為false有抽水)</summary>
        public bool IsNoRebateAmount { get; set; }

        /// <summary>
        /// 統一設定有效/總投注額的方法,避免屬性各自設定造成邏輯沒有統一的情況
        /// </summary>
        public void SetBetMoneys(bool isComputeAdmissionBetMoney, decimal validBetMoney, decimal allBetMoney)
        {
            AllBetMoney = allBetMoney;

            if (isComputeAdmissionBetMoney)
            {
                ProfitLossMoney = validBetMoney;
            }
            else
            {
                ProfitLossMoney = AllBetMoney;
            }
        }
    }

    public class InsertTPGameProfitlossParam : ComputeBetBonusProfitLoss
    {
        public DateTime ProfitLossTime { get; set; }

        public string ProfitLossType => ProfitLossTypeName.KY.Value;

        public string ProfitLossType_FD => ProfitLossTypeName.FD.Value;

        /// <summary>輸贏金額</summary>
        public decimal WinMoney { get; set; }

        /// <summary>派獎金額</summary>
        public decimal PrizeMoney => AllBetMoney + WinMoney;

        public string Memo { get; set; }

        public string PlayID { get; set; }

        public string GameType { get; set; }

        public DateTime BetTime { get; set; }

        /// <summary>
        /// 紀錄SQLite轉過來的KEY, 以便之後更新狀態回SQLite
        /// </summary>
        public string KeyId { get; set; }

        /// <summary>是否忽略此筆資料</summary>
        public bool IsIgnore { get; set; }

        /// <summary>因應AG sqlite有兩個資料來源</summary>
        public string FromSource { get; set; }
    }    

    public class SaveTPGameProfitlossResult
    {
        public string KeyId { get; set; }

        public string ErrorMsg { get; set; }
    }

    public class SaveLocalBetLogParam
    {
        public string KeyId { get; set; }
        public SaveBetLogFlags SaveBetLogFlag { get; set; }
    }
}