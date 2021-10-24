using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Enums
{
    public class BaseReportInnerSettingService : IReportInnerSettingService
    {
        protected readonly ProfitLossReportTabTypes _profitLossReportTabTypes;

        public BaseReportInnerSettingService(ProfitLossReportTabTypes profitLossReportTabTypes)
        {
            _profitLossReportTabTypes = profitLossReportTabTypes;
        }

        public virtual ReportInnerSetting GetInnerSetting()
        {
            var innerMenus = new List<InnerMenu>
            {
                new InnerMenu()
                {
                    ActionName = "ProfitAndLoss",
                    MenuName = _profitLossReportTabTypes.ProfitLossInnerMenuName
                },

                new InnerMenu()
                {
                    ActionName = "PlayHistory",
                    MenuName = _profitLossReportTabTypes.BuyHistoryInnerMenuName
                }
            };

            return new ReportInnerSetting()
            {
                InnerMenus = innerMenus,
                IsHBVisible = false,
                IsYJVisible = false,
                FooterMemo = CommonElement.ThirdPartyGameLagMemo
            };
        }
    }

    public class LotteryReportInnerSettingService : BaseReportInnerSettingService
    {
        public LotteryReportInnerSettingService(ProfitLossReportTabTypes profitLossReportTabTypes) : base(profitLossReportTabTypes)
        {
        }

        public override ReportInnerSetting GetInnerSetting()
        {
            List<InnerMenu> innerMenus = base.GetInnerSetting().InnerMenus;
            innerMenus[1].MenuName = SelectItemElement.LotteryInnerMenu_PlayInfoHistory;

            //加上彩票追號
            innerMenus.Add(new InnerMenu()
            {
                ActionName = "AfterHistory",
                MenuName = SelectItemElement.LotteryInnerMenu_AfterHistory
            });

            return new ReportInnerSetting()
            {
                InnerMenus = innerMenus,
                IsHBVisible = true,
                IsYJVisible = true,
                FooterMemo = null
            };
        }
    }
}
