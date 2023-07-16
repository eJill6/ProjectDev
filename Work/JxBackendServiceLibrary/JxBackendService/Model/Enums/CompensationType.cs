using System.Linq;

namespace JxBackendService.Model.Enums
{
    public class CompensationType : BaseStringValueModel<CompensationType>
    {
        private CompensationType()
        { }

        public bool IsMoneyIn { get; private set; }

        public static CompensationType TransferMoneyIn = new CompensationType()
        {
            Value = "TransferMoneyIn",
            IsMoneyIn = true
        };

        public static CompensationType TransferMoneyOut = new CompensationType()
        {
            Value = "TransferMoneyOut",
            IsMoneyIn = false
        };
    }
}