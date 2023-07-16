using JxBackendService.Common.Util;
using System;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    /// <summary>
    /// 給後台前端用的ViewModel
    /// </summary>
    public class TPGameMoneyInfoViewModel : BaseTPGameMoneyInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// 訂單類型
        /// </summary>
        public string OrderTypeName { get; set; }

        public override string GetMoneyID()
        {
            //只是繼承使用父類欄位
            throw new InvalidProgramException($"{nameof(GetMoneyID)} is not support");
        }

        public override string GetPrimaryKeyColumnName()
        {
            //只是繼承使用父類欄位
            throw new InvalidProgramException($"{nameof(GetPrimaryKeyColumnName)} is not support");
        }

        public string StatusName { get; set; }

        public string OrderTimeText
        {
            get { return OrderTime.ToFormatDateTimeString(); }
            set { }
        }

        public string HandleTimeText
        {
            get { return HandTime.ToFormatDateTimeString(); }
            set { }
        }

        public string AmountText
        {
            get { return Amount.ToCurrency(); }
            set { }
        }
    }
}