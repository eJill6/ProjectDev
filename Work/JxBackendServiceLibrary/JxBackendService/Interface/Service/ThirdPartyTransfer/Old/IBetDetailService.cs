namespace JxBackendService.Interface.Service.ThirdPartyTransfer.Old
{
    public interface IBetDetailService<ApiParamType, ReturnType>
    {
        ReturnType GetRemoteBetDetail(ApiParamType model);
    }
}