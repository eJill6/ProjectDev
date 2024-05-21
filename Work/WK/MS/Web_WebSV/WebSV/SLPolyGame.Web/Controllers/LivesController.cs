using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.ViewModel;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using Microsoft.Extensions.Logging;
using SLPolyGame.Web.Controllers.Base;
using SLPolyGame.Web.MSSeal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace SLPolyGame.Web.Controllers
{
    /// <summary>
    /// 第三方直播信息
    /// </summary>
    public class LivesController : BaseApiController
    {
        private readonly ITPLiveStreamService _tpLiveStreamService;

        /// <summary>ctor</summary>
        public LivesController()
        {
            _tpLiveStreamService = DependencyUtil.ResolveJxBackendService<ITPLiveStreamService>(
                EnvLoginUser,
                DbConnectionTypes.Slave);
        }

        /// <summary>
        /// 获取主播列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResultModel<List<IMiseLiveAnchor>> GetAnchors()
        {
            //這裡不包try-catch讓exception回到底層拋出回應並且做log與告警通知
            List<IMiseLiveAnchor> anchros = _tpLiveStreamService.GetAnchors()
                .Select(anchor =>
                {
                    anchor.Description = OBEBSharedAppSetting.NewDescription.IsNullOrEmpty() ? "百家乐" : OBEBSharedAppSetting.NewDescription;
                    anchor.Avatar = OBEBSharedAppSetting.NewAvatar.IsNullOrEmpty() ? anchor.Avatar : OBEBSharedAppSetting.NewAvatar;
                    return anchor;
                }).ToList();

            return new ResultModel<List<IMiseLiveAnchor>> { Success = true, Data = anchros };
        }
    }
}