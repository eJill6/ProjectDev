using System;

namespace JxBackendService.Model.ThirdParty.CQ9SL
{
    public class CQ9Response<T>
    {
        public T Data { get; set; }

        public CQ9Status Status { get; set; }
    }

    public class CQ9Status
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public DateTime Datetime { get; set; }

        public string TraceCode { get; set; }

        public string Latency { get; set; }

        public bool IsSuccess => Code == CQ9Code.Success;

        public string ErrorMessage
        {
            get
            {
                if (IsSuccess)
                {
                    return null;
                }

                return $"{Code}-{Message}";
            }
        }
    }

    public class CQ9PlayerInfo
    {
        public string Account { get; set; }

        public string Nickname { get; set; }

        public string Password { get; set; }
    }

    public class CQ9PlayerLogin
    {
        public string Usertoken { get; set; }
    }

    public class CQ9LobbyLink
    {
        public string Token { get; set; }

        public string Url { get; set; }
    }

    public class CQ9PlayerBalance
    {
        public decimal Balance { get; set; }

        public string Currency { get; set; }
    }

    public class CQ9TransactionRecord
    {
        public string Action { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public decimal Before { get; set; }

        public string Currency { get; set; }

        public DateTime Endtime { get; set; }

        public DateTime Eventtime { get; set; }

        public string Mtcode { get; set; }

        public string Remark { get; set; }

        public string Status { get; set; }

        public CQ9TransactionAction TransactionAction => CQ9TransactionAction.GetSingle(Action);

        public CQ9TransactionStatus TransactionStatus => CQ9TransactionStatus.GetSingle(Status);
    }
}