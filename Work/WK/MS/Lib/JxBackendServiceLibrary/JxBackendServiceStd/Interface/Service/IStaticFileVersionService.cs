using JxBackendService.Model.ViewModel.Web;

namespace JxBackendService.Interface.Service
{
    public interface IStaticFileVersionService
    {
        string GetStaticFileVersion();

        void InitStaticFileVersionInfo(params StaticDirectoryInfo[] staticDirectoryInfos);
    }
}