using IPToolModel;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;

namespace JxBackendService.Repository
{
    public class IpDataRep : BaseDbRepository<IpData>, IIpDataRep
    {
        public IpDataRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        /// <summary>
        /// 从DB記錄的資料取得IP地区
        /// </summary>
        public string GetIpArea(JxIpInformation ipInfo)
        {
            string sql = $@"                
                DECLARE @area NVARCHAR(1000);
                SET @area = {InlodbType.Inlodb.Value}.[dbo].[fn_GetIPArea](@LoginIp, @IPVersion, @IPNumber);
				SELECT @area
            ";

            var param = new
            {
                LoginIp = ipInfo.DestinationIP.ToVarchar(128),
                IPVersion = ipInfo.DestinationIPVersion,
                IPNumber = ipInfo.DestinationIPNumber.Value.ToString().ToVarchar(128)
            };

            return DbHelper.QueryFirstOrDefault<string>(sql, param);
        }

    }
}
