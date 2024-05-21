namespace JxBackendService.Model.Enums.Finance
{
    public class MiseTransferType : BaseIntValueModel<MiseTransferType>
    {
        private MiseTransferType()
        {
        }

        public static MiseTransferType In = new MiseTransferType() { Value = 1 };

        public static MiseTransferType Out = new MiseTransferType() { Value = 2 };
    }
}