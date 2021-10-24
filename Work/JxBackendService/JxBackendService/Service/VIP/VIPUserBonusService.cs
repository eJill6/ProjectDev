using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.VIP;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.VIP;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.VIP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.BackSideWeb;
using JxBackendService.Model.ViewModel.VIP;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.VIP
{
    public class VIPUserBonusService : BaseService, IVIPUserBonusService
    {
        private readonly IVIPSettingService _vipSettingService;

        private readonly IVIPUserBonusRep _vipUserBonusRep;
        private readonly IUserInfoRep _userInfoRep;

        public VIPUserBonusService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser,
            dbConnectionType)
        {
            _vipSettingService = ResolveJxBackendService<IVIPSettingService>();

            _vipUserBonusRep = ResolveJxBackendService<IVIPUserBonusRep>();
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
        }

        public VIPUserBonusInitData GetBacksideWebInitData()
        {
            bool hasBlankOption = true;
            string defaultValue = string.Empty;
            string defaultDisplayText = SelectItemElement.SelectAll;

            // 禮金類型選項
            List<JxBackendSelectListItem> bonusTypesItems = VIPBonusType.GetSelectListItems(hasBlankOption, defaultValue, defaultDisplayText);

            // 領取狀態選項
            List<JxBackendSelectListItem> receivedStatusItems = ReceivedStatus.GetSelectListItems(hasBlankOption, defaultValue, defaultDisplayText);

            // VIP等級設定 抽成選項
            List<VIPLevelSetting> vipSettings = _vipSettingService.GetAll();

            List<JxBackendSelectListItem> vipLevelItems = vipSettings.Select(setting => new JxBackendSelectListItem(setting.VIPLevel.ToString(), setting.LevelName)).ToList();

            return new VIPUserBonusInitData()
            {
                BonusTypeItems = bonusTypesItems,
                VIPLevelItems = vipLevelItems.AddBlankOption(hasBlankOption, defaultValue, defaultDisplayText),
                ReceivedStatusItems = receivedStatusItems
            };
        }

        public PagedResultWithAdditionalData<VIPUserBonusModel, decimal> GetList(VIPUserBonusQueryParam param, BasePagingRequestParam pageParam)
        {
            if (!string.IsNullOrWhiteSpace(param.Username))
            {
                param.UserID = _userInfoRep.GetFrontSideUserId(param.Username);
            }

            PagedResultWithAdditionalData<VIPUserBonus, decimal> entityList = _vipUserBonusRep.GetEntityList(param, pageParam);

            List<int> userIdList = entityList.ResultList.Select(x => x.UserID).Distinct().ToList();
            Dictionary<int, string> usernameMapping = UserIdMapUsername(userIdList);

            List<int> vipLevelList = entityList.ResultList.Select(x => x.ReceivedVIPLevel).Distinct().ToList();
            Dictionary<int, string> vipNameMapping = VIPLevelMapLevelName(vipLevelList);

            // DTO
            PagedResultWithAdditionalData<VIPUserBonusModel, decimal> result =
                new PagedResultWithAdditionalData<VIPUserBonusModel, decimal>() { AdditionalData = entityList.AdditionalData };

            foreach (VIPUserBonus entity in entityList.ResultList)
            {
                result.ResultList.Add(new VIPUserBonusModel()
                {
                    Username = usernameMapping[entity.UserID],
                    VIPLevel = vipNameMapping[entity.ReceivedVIPLevel],
                    BonusType = VIPBonusType.GetName(entity.BonusType),
                    BonusMoney = entity.BonusMoney,
                    Memo = entity.MemoJson.ToLocalizationContent(),
                    ReceivedStatus = ReceivedStatus.GetName(entity.ReceivedStatus),
                    ReceivedDate = entity.ReceivedDate
                });
            }

            return result;
        }

        private Dictionary<int, string> UserIdMapUsername(List<int> userIdList)
        {
            List<string> usernameList = _userInfoRep.GetUserNames(userIdList);

            Dictionary<int, string> dict = userIdList.Zip(usernameList, (k, v) => new { k, v })
                                                    .ToDictionary(x => x.k, x => x.v);
            return dict;
        }

        private Dictionary<int, string> VIPLevelMapLevelName(List<int> vipLevelist)
        {
            List<string> vipSettings = _vipSettingService.GetAll()
                                                    .Where(setting => vipLevelist.Contains(setting.VIPLevel))
                                                    .Select(setting => setting.LevelName).ToList();

            Dictionary<int, string> dict = vipLevelist.Zip(vipSettings, (k, v) => new { k, v })
                                                    .ToDictionary(x => x.k, x => x.v);
            return dict;
        }

    }
}