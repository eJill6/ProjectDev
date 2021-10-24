using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IReportTypeService
    {
        /// <summary>取得報表類型選單</summary>
        List<JxBackendSelectListItem> GetMenus();

        /// <summary>單一報表類型下的產品</summary>
        List<JxBackendSelectListItem> GetProductSelectItemsByReportType(int reportTypeValue);

        /// <summary>單一報表類型下的產品</summary>
        List<PlatformProduct> GetProductsByReportType(int reportTypeValue);

        ReportInnerSetting GetReportInnerSetting(int reportTypeValue);
    }
}