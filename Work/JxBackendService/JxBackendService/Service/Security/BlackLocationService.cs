using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Security;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using static IPToolModel.Enums.IPVersion_Enum;

namespace JxBackendService.Service.Security
{
    public class BlackLocationService : BaseService, IBlackLocationService, IBlackLocationReadService
    {
        private readonly IBlackIpListRep _blackIpListRep;
        private readonly IBlackIpRangeListRep _blackIpRangeListRep;
        private readonly IBlackAreaListRep _blackAreaListRep;
        private readonly IWhiteIpListReadService _whiteIpListReadService;

        public BlackLocationService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _blackIpListRep = ResolveJxBackendService<IBlackIpListRep>();
            _blackIpRangeListRep = ResolveJxBackendService<IBlackIpRangeListRep>();
            _blackAreaListRep = ResolveJxBackendService<IBlackAreaListRep>();
            _whiteIpListReadService = ResolveJxBackendService<IWhiteIpListReadService>();
        }

        public bool CreateBlackIp(string ipAddress, BlackIpType blackIpType, string remark, string userName)
        {
            BlackIpList blackIpList = _blackIpListRep.GetSingle(ipAddress, blackIpType);

            if (blackIpList != null)
            {
                _blackIpListRep.DeleteByProcedure(blackIpList);
            }

            BaseReturnDataModel<long> returnModel = _blackIpListRep.CreateByProcedure(new BlackIpList()
            {
                Ip = ipAddress,
                IType = blackIpType.Value,
                Remark = remark,
                IsWork = true,
                UserName = userName,
            });

            return returnModel.IsSuccess;
        }

        public bool IsFrontSideLoginIpActive(JxIpInformation ipInformation)
        {
            if (_whiteIpListReadService.IsActive(ipInformation.DestinationIP, WhiteIpType.FrontSideLogin))
            {
                return true;
            }

            BlackIpType blackIpType = BlackIpType.Login;
            BlackIpList blackIpList = _blackIpListRep.GetSingle(ipInformation.DestinationIP, blackIpType);

            if (blackIpList != null && blackIpList.IsWork)
            {
                LogUtil.ForcedDebug($"用戶登入IP黑名單攔截：用户名:{EnvLoginUser.LoginUser.UserName}," +
                        $"{nameof(ipInformation.SourceIP)}:{ipInformation.SourceIP}," +
                        $"{nameof(ipInformation.DestinationIP)}:{ipInformation.DestinationIP}");
                return false;
            }

            try
            {
                if (_blackIpRangeListRep.IsActive(ipInformation, blackIpType))
                {
                    LogUtil.ForcedDebug($"用戶登入IP黑名單攔截：用户名:{EnvLoginUser.LoginUser.UserName}," +
                            $"{nameof(ipInformation.SourceIP)}:{ipInformation.SourceIP}," +
                            $"{nameof(ipInformation.DestinationIP)}:{ipInformation.DestinationIP}");

                    return false;
                }
            }
            catch (Exception ex)
            {   
                LogUtil.Error(ex);//怕轉型錯誤
                LogUtil.ForcedDebug("ipInformation:" + ipInformation.ToJsonString());
            }

            try
            {
                ExistInBlackAreaListResult existInBlackAreaListResult = _blackAreaListRep.GetExistInBlackAreaList(ipInformation, blackIpType);

                if (existInBlackAreaListResult != null && existInBlackAreaListResult.IsExist)
                {
                    LogUtil.ForcedDebug($"用戶登入地區黑名單攔截：用户名:{EnvLoginUser.LoginUser.UserName}," +
                            $"{nameof(ipInformation.SourceIP)}:{ipInformation.SourceIP}," +
                            $"{nameof(ipInformation.DestinationIP)}:{ipInformation.DestinationIP}");

                    return false;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);//怕轉型錯誤
                LogUtil.ForcedDebug("ipInformation:" + ipInformation.ToJsonString());
            }

            return true;
        }
    }
}
