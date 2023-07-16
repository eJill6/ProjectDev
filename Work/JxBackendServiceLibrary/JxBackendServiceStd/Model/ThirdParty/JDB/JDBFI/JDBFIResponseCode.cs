using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.JDB
{
    public class JDBFIResponseCode : BaseStringValueModel<JDBFIResponseCode>
    {
        private JDBFIResponseCode()
        { }

        /// <summary>成功</summary>
        public static readonly JDBFIResponseCode Success = new JDBFIResponseCode() { Value = "0000" };

        /// <summary>帳號已存在</summary>
        public static readonly JDBFIResponseCode AccountAlreadyExist = new JDBFIResponseCode() { Value = "7602" };

        /// <summary>Failed</summary>
        public static readonly JDBFIResponseCode Failed = new JDBFIResponseCode() { Value = "9999" };

        /// <summary>無數據</summary>
        public static readonly JDBFIResponseCode DataDoesNotExist = new JDBFIResponseCode() { Value = "9015" };

        /// <summary>玩家进行游戏或停留在游戏大厅时，不得提款</summary>
        public static readonly JDBFIResponseCode NoWithdrawalInGame = new JDBFIResponseCode() { Value = "6901" };
    }
}