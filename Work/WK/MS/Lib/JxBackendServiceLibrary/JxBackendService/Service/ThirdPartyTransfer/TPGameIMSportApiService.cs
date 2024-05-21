using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMSportApiService : TPGameIMOneApiService
    {
        public TPGameIMSportApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public override PlatformProduct Product => PlatformProduct.IMSport;

        public override IIMOneAppSetting AppSetting => _gameAppSettingService.GetIMSportAppSetting();

        protected override PlatformHandicap ConvertToPlatformHandicap(string handicap)
        {
            IMOneHandicap imOneHandicap = IMOneHandicap.GetSingle(handicap);

            if (imOneHandicap == null)
            {
                return null;
            }

            return imOneHandicap.PlatformHandicap;
        }
    }
}
