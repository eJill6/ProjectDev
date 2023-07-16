using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.OB.OBEB
{
    public class OBEBResponseCode : BaseStringValueModel<OBEBResponseCode>
    {
        private OBEBResponseCode()
        { }

        public static OBEBResponseCode Success = new OBEBResponseCode() { Value = "200" };

        public static OBEBResponseCode AccountExist = new OBEBResponseCode() { Value = "20000" };
    }
}