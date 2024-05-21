using JxBackendService.Interface.Model.MiseLive.ViewModel;
using JxBackendService.Model.ReturnModel;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.MiseLive
{
    public interface ITPLiveStreamService
    {
        BaseReturnModel CrawlAnchors();

        List<IMiseLiveAnchor> GetAnchors();

        void StartNewSyncCachedAnchorsJob();
    }
}