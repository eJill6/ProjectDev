using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService
{
    public class TPGameStoredProcedureService : BaseService, ITPGameStoredProcedureService
    {
        private readonly IPlatformProductService _platformProductService;

        private readonly IJxCacheService _jxApplication;

        private readonly IThirdPartyUserAccountRep _thirdPartyUserAccountRep;

        public TPGameStoredProcedureService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
            _jxApplication = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
            _thirdPartyUserAccountRep = ResolveJxBackendService<IThirdPartyUserAccountRep>(DbConnectionTypes.Slave);
        }

        public PagedResultModel<TPGameMoneyInfoViewModel> GetMoneyInfoList(PlatformProduct product, SearchTransferType searchTransferType, SearchTPGameMoneyInfoParam param)
        {
            var returnModel = new PagedResultModel<TPGameMoneyInfoViewModel>(param)
            {
                ResultList = new List<TPGameMoneyInfoViewModel>()
            };

            ITPGameStoredProcedureRep rep = ResolveTPGameStoredProcedureRep(product);
            List<BaseTPGameMoneyInfo> baseMoneyInfos = null;

            if (searchTransferType == SearchTransferType.In)
            {
                PagedResultModel<TPGameMoneyInInfo> moneyInInfos = rep.GetMoneyInInfoList(param);
                baseMoneyInfos = moneyInInfos.ResultList.ToList<BaseTPGameMoneyInfo>();
                returnModel.TotalCount = moneyInInfos.TotalCount;
            }
            else if (searchTransferType == SearchTransferType.Out)
            {
                PagedResultModel<TPGameMoneyOutInfo> moneyOutInfos = rep.GetMoneyOutInfoList(param);
                baseMoneyInfos = moneyOutInfos.ResultList.ToList<BaseTPGameMoneyInfo>();
                returnModel.TotalCount = moneyOutInfos.TotalCount;
            }
            else
            {
                throw new ArgumentNullException();
            }

            returnModel.ResultList = new List<TPGameMoneyInfoViewModel>();

            baseMoneyInfos.ForEach(f =>
            {
                returnModel.ResultList.Add(GetMoneyInfoViewModel(f, product, searchTransferType));
            });

            return returnModel;
        }

        private TPGameMoneyInfoViewModel GetMoneyInfoViewModel(BaseTPGameMoneyInfo baseTPGameMoneyInfo, PlatformProduct product, SearchTransferType searchTransferType)
        {
            TPGameMoneyInfoViewModel viewModel = baseTPGameMoneyInfo.CastByJson<TPGameMoneyInfoViewModel>();
            viewModel.Id = baseTPGameMoneyInfo.GetMoneyID();
            viewModel.OrderTypeName = _platformProductService.GetName(product.Value) + searchTransferType.Name;

            if (searchTransferType == SearchTransferType.In)
            {
                viewModel.StatusName = TPGameMoneyInOrderStatus.GetName(baseTPGameMoneyInfo.Status);
            }
            else if (searchTransferType == SearchTransferType.Out)
            {
                viewModel.StatusName = TPGameMoneyOutOrderStatus.GetName(baseTPGameMoneyInfo.Status);
            }

            return viewModel;
        }

        public TPGameMoneyInfoViewModel GetMoneyInfo(PlatformProduct product, SearchTransferType searchTransferType, string moneyId)
        {
            ITPGameStoredProcedureRep tpGameRep = ResolveTPGameStoredProcedureRep(product);
            BaseTPGameMoneyInfo baseTPGameMoneyInfo = null;

            if (searchTransferType == SearchTransferType.In)
            {
                TPGameMoneyInInfo moneyInInfo = tpGameRep.GetTPGameMoneyInInfo(moneyId);
                baseTPGameMoneyInfo = moneyInInfo;
            }
            else if (searchTransferType == SearchTransferType.Out)
            {
                TPGameMoneyOutInfo moneyOutInfo = tpGameRep.GetTPGameMoneyOutInfo(moneyId);
                baseTPGameMoneyInfo = moneyOutInfo;
            }
            else
            {
                throw new ArgumentNullException();
            }

            return GetMoneyInfoViewModel(baseTPGameMoneyInfo, product, searchTransferType);
        }

        private ITPGameStoredProcedureRep ResolveTPGameStoredProcedureRep(PlatformProduct product)
        {
            return ResolveJxBackendService<ITPGameStoredProcedureRep>(product);
        }
    }
}