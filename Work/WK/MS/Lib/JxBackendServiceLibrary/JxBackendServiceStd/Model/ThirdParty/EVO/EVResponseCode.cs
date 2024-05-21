using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.EVO
{
    public class EVResponseCode : BaseIntValueModel<EVResponseCode>
    {
        private EVResponseCode() { }

        public static EVResponseCode Success = new EVResponseCode() { Value = 0 };
        public static EVResponseCode Error = new EVResponseCode() { Value = 999 };

        public static EVResponseCode AccountExist = new EVResponseCode() { Value = 40006 };
    }

    public class EVTransferSatatus : BaseIntValueModel<EVTransferSatatus>
    {
        private EVTransferSatatus() { }

        public static EVTransferSatatus Success = new EVTransferSatatus { Value = 1 };
        public static EVTransferSatatus Fail = new EVTransferSatatus { Value = 2 };
        public static EVTransferSatatus Unknown = new EVTransferSatatus { Value = 3 };

    }

}
