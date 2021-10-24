using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.BackSideWeb;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Finance
{
    public class GivePrizesService : BaseService, IGivePrizesService
    {
        private readonly IProfitLossTypeNameService _profitLossTypeNameService;
        private readonly IGivePrizeRep _givePrizeRep;
        private readonly IWalletTypeService _walletTypeService;
        private readonly IVIPUserInfoRep _vipUserInfoRep;

        public GivePrizesService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _profitLossTypeNameService = DependencyUtil.ResolveKeyed<IProfitLossTypeNameService>(Merchant);
            _walletTypeService = DependencyUtil.ResolveKeyed<IWalletTypeService>(Merchant);
            _givePrizeRep = ResolveJxBackendService<IGivePrizeRep>();
            _vipUserInfoRep = ResolveJxBackendService<IVIPUserInfoRep>();
        }

        public List<JxBackendSelectListItem<bool>> GetPrizeTypeItems(WalletType walletType)
        {
            List<ProfitLossTypeName> profitLossTypeNames = new List<ProfitLossTypeName>();

            if (walletType == WalletType.Center)
            {
                profitLossTypeNames = _profitLossTypeNameService.GetGivePrizeList();
            }
            else if (walletType == WalletType.Agent)
            {
                profitLossTypeNames = _profitLossTypeNameService.GetGivePrizeByAgent();
            }

            return profitLossTypeNames.Select(s => new JxBackendSelectListItem<bool>()
            {
                Value = s.Value,
                Text = s.Name,
                DataModel = s.IsGivePrizesNeedBankSelection
            }).ToList();
        }

        public BaseReturnModel SaveGivePrize(GivePrizesProcessParam saveParam)
        {
            ProfitLossTypeName profitLossTypeName = ProfitLossTypeName.GetSingle(saveParam.ProfitLossType);
            BaseBankType bankType = null;

            if (profitLossTypeName == ProfitLossTypeName.CZ)
            {
                bankType = new BaseBankType()
                {
                    BankTypeID = saveParam.BankTypeId.Value,
                    BankTypeName = saveParam.BankName
                };
            }

            LocalizationParam localizationParam = CreateGivePrizeMemoJsonParam(profitLossTypeName, saveParam.Memo);

            var givePrizesByCustomerTypeParam = new GivePrizesByCustomerTypeParam()
            {
                UserID = saveParam.UserId,
                WalletType = WalletType.GetSingle(saveParam.WalletTypeValue),
                PrizesMoney = saveParam.Money,
                FlowMultiple = saveParam.FlowMultiple,
                BankType = bankType,
                RefundTypeParam = profitLossTypeName.RefundTypeParam,
                ProfitLossType = profitLossTypeName,
                MemoJsonParam = localizationParam,
            };

            return _givePrizeRep.GivePrizesByCustomerType(givePrizesByCustomerTypeParam);
        }

        public LocalizationParam CreateGivePrizeMemoJsonParam(ProfitLossTypeName profitLossTypeName, string memo)
        {
            string resourcePropertyName = nameof(DBContentElement.GivePrizeMemo);
            var args = new List<string>() { profitLossTypeName.Name, memo };

            if (profitLossTypeName == ProfitLossTypeName.Prizes)
            {
                resourcePropertyName = nameof(DBContentElement.GivePrizeMemoByPrize);
                args.RemoveAt(0);
            }

            var localizationParam = new LocalizationParam()
            {
                LocalizationSentences = new List<LocalizationSentence>()
                {
                    new LocalizationSentence()
                    {
                        ResourcePropertyName = resourcePropertyName,
                        Args = args
                    }
                }
            };

            return localizationParam;
        }

        public GivePrizeInitData GetGivePrizeInitData(int userId)
        {
            var givePrizeInitData = new GivePrizeInitData()
            {
                IsFlowMultipleVisible = Merchant.CustomerType == CustomerTypes.Direct,
            };

            List<JxBackendSelectListItem> walletTypeItems = _walletTypeService.GetSelectListItems();

            //判斷是否有代理身分
            BaseReturnModel checkReturnModel = _vipUserInfoRep.CheckQualifiedForUser(userId, WalletType.Agent);

            if (!checkReturnModel.IsSuccess)
            {
                walletTypeItems.RemoveAll(r => r.Value == WalletType.Agent.Value.ToString());
            }

            if (walletTypeItems.Count > 1)
            {
                givePrizeInitData.IsWalletTypeVisible = true;
                walletTypeItems.AddBlankOption();
            }

            givePrizeInitData.WalletTypeItems = walletTypeItems.Select(s => new JxBackendSelectListItem<bool>()
            {
                Value = s.Value,
                Text = s.Text,
                DataModel = s.Value == WalletType.Center.Value.ToString() //代理錢包不顯示流水
            }).ToList();

            return givePrizeInitData;
        }
    }
}