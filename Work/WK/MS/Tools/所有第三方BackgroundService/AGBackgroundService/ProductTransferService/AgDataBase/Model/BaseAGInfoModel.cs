using JxBackendService.Model.ThirdParty.Base;

namespace ProductTransferService.AgDataBase.Model
{
    public abstract class BaseAGInfoModel : BaseRemoteBetLog
    {
        public abstract bool IsIgnoreAddProfitLoss { get; }

        public string platformType { get; set; }    //平台类型

        public string PlatformTypeName
        {
            get
            {
                if (AGConstParams.PlatformTypes.ContainsKey(platformType))
                {
                    return AGConstParams.PlatformTypes[platformType];
                }

                return null;
            }
        }
    }
}