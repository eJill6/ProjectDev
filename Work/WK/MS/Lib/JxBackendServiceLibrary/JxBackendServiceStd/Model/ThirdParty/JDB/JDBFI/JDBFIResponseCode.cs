using Castle.Core.Internal;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.ThirdParty.JDB
{
    public class JDBFIResponseCode : BaseStringValueModel<JDBFIResponseCode>
    {
        private JDBFIResponseCode()
        {
            ResourceType = typeof(TPResponseCodeElement);
        }

        /// <summary>成功</summary>
        public static readonly JDBFIResponseCode Success = new JDBFIResponseCode()
        {
            Value = "0000"
        };

        /// <summary>帳號已存在</summary>
        public static readonly JDBFIResponseCode AccountAlreadyExist = new JDBFIResponseCode()
        {
            Value = "7602",
            ResourcePropertyName = nameof(TPResponseCodeElement.JDB_7602)
        };

        /// <summary>Failed</summary>
        public static readonly JDBFIResponseCode Failed = new JDBFIResponseCode()
        {
            Value = "9999",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.Fail)
        };

        /// <summary>無數據</summary>
        public static readonly JDBFIResponseCode DataDoesNotExist = new JDBFIResponseCode()
        {
            Value = "9015",
            ResourcePropertyName = nameof(TPResponseCodeElement.JDB_9015)
        };

        /// <summary>玩家进行游戏或停留在游戏大厅时，不得提款</summary>
        public static readonly JDBFIResponseCode NoWithdrawalInGame = new JDBFIResponseCode()
        {
            Value = "6901",
            ResourcePropertyName = nameof(TPResponseCodeElement.JDB_6901)
        };

        /// <summary> User balance is zero </summary>
        public static readonly JDBFIResponseCode JDB_6002 = new JDBFIResponseCode()
        {
            Value = "6002",
            ResourcePropertyName = nameof(TPResponseCodeElement.JDB_6002)
        };

        /// <summary> User is suspended. </summary>
        public static readonly JDBFIResponseCode JDB_7502 = new JDBFIResponseCode()
        {
            Value = "7502",
            ResourcePropertyName = nameof(TPResponseCodeElement.JDB_7502)
        };

        public static string CompleteMessage(string code, string err_text)
        {
            string message = GetName(code);

            if (message.IsNullOrEmpty())
            {
                return err_text;
            }

            return message;
        }
    }
}