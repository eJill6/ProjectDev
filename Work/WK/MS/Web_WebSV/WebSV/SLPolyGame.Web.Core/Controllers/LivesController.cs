using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.ViewModel;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.MSSeal.Models;

namespace SLPolyGame.Web.Core.Controllers
{
    /// <summary>
    /// 第三方直播信息
    /// </summary>
    public class LivesController : BaseApiController
    {
        private readonly Lazy<ITPLiveStreamService> _tpLiveStreamService;

        /// <summary>ctor</summary>
        public LivesController()
        {
            _tpLiveStreamService = DependencyUtil.ResolveJxBackendService<ITPLiveStreamService>(
                EnvLoginUser,
                DbConnectionTypes.Slave);
        }

        /// <summary>获取主播列表</summary>
        [HttpGet]
        public async Task<ResultModel<List<IMiseLiveAnchor>>> GetAnchors()
        {
            string ss = OBEBSharedAppSetting.NewAvatar;
            string ss1 = SharedAppSettings.WebCoreCDN;

            //這裡不包try-catch讓exception回到底層拋出回應並且做log與告警通知
            List<IMiseLiveAnchor> anchors = _tpLiveStreamService.Value.GetAnchors()
                .Select(anchor =>
                {
                    anchor.Description = OBEBSharedAppSetting.NewDescription.IsNullOrEmpty() ? "百家乐" : OBEBSharedAppSetting.NewDescription;
                    anchor.Avatar = (OBEBSharedAppSetting.NewAvatar.IsNullOrEmpty() || SharedAppSettings.WebCoreCDN.IsNullOrEmpty()) ? anchor.Avatar : $"{SharedAppSettings.WebCoreCDN}{OBEBSharedAppSetting.NewAvatar}";

                    return anchor;
                }).ToList();

            return await Task.FromResult(new ResultModel<List<IMiseLiveAnchor>> { Success = true, Data = anchors });
        }        
    }
}