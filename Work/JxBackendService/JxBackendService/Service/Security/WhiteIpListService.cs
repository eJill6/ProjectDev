using JxBackendService.Interface.Repository.Security;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Entity.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Security
{
    public class WhiteIpListService : BaseService, IWhiteIpListService, IWhiteIpListReadService
    {
        private readonly IWhiteIpListRep _whiteIpListRep;

        public WhiteIpListService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _whiteIpListRep = ResolveJxBackendService<IWhiteIpListRep>();
        }

        //public bool Create(string ipAddress, WhiteIpType WhiteIpType, string remark)
        //{
        //    WhiteIpList WhiteIpList = _whiteIpListRep.GetSingle(ipAddress, WhiteIpType);

        //    if (WhiteIpList == null)
        //    {
        //        BaseReturnDataModel<long> returnModel = _whiteIpListRep.CreateByProcedure(new WhiteIpList()
        //        {
        //            Ip = ipAddress,
        //            IType = WhiteIpType.Value,
        //            Remark = remark,
        //            IsWork = true,
        //        });

        //        return returnModel.IsSuccess;
        //    }
        //    else if (!WhiteIpList.IsWork)
        //    {
        //        WhiteIpList.IsWork = true;
        //        return _whiteIpListRep.UpdateByProcedure(WhiteIpList);
        //    }

        //    return true;
        //}

        public bool IsActive(string ipAddress, WhiteIpType whiteIpType)
        {
            WhiteIpList whiteIpList = _whiteIpListRep.GetSingle(ipAddress, whiteIpType);

            if (whiteIpList == null)
            {
                return false;
            }

            return whiteIpList.IsWork;
        }
    }
}
