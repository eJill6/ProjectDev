using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Interface.Service
{
    public interface ISqliteTokenService
    {
        string GetSqliteNextSearchToken(string lastSearchToken, RequestAndResponse dataMrequestAndResponseodel);
    }
}
