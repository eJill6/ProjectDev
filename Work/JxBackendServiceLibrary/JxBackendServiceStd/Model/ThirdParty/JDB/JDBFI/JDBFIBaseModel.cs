using JxBackendService.Model.Enums.ThirdParty.JDB;

namespace JxBackendService.Model.ThirdParty.JDB.JDBFI
{
    public class JDBFIBaseRequestModel
    {
        /// <summary>
        /// action
        /// </summary>
        public int Action { get; private set; }

        /// <summary>
        /// 当前系统时间
        /// </summary>
        public string Ts { get; set; }

        public void SetApiAction(JDBApiAction apiAction)
        {
            Action = apiAction.Value;
        }
    }

    public class JDBFIBaseParentRequestModel : JDBFIBaseRequestModel
    {
        /// <summary>
        /// 代理账号
        /// </summary>
        public string Parent { get; set; }
    }

    public class JDBFIBaseResponseModel
    {
        public string Status { get; set; }

        public string Err_text { get; set; }
    }
}