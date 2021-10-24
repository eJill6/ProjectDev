using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{ 
    public class AwardTypes : BaseIntValueModel<AwardTypes>
    {
        public static AwardTypes None = new AwardTypes()
        {
            Value = 0,
            ResourcePropertyName = nameof(CommonElement.PrizeNone),
            ResourceType = typeof(CommonElement)
        };

        public static AwardTypes Win = new AwardTypes()
        {
            Value = 1,
            ResourcePropertyName = nameof(CommonElement.PrizeWin),
            ResourceType = typeof(CommonElement)
        };

        public static AwardTypes Loss = new AwardTypes()
        {
            Value = 2,
            ResourcePropertyName = nameof(CommonElement.PrizeLoss),
            ResourceType = typeof(CommonElement)
        };

        public static AwardTypes Cancel = new AwardTypes()
        {
            Value = 3,
            ResourcePropertyName = nameof(CommonElement.PrizeCancel),
            ResourceType = typeof(CommonElement)
        };

        public static AwardTypes CancelSys = new AwardTypes()
        {
            Value = 4,
            ResourcePropertyName = nameof(CommonElement.PrizeCancelSys),
            ResourceType = typeof(CommonElement)
        };

        public static AwardTypes CancelAdmin = new AwardTypes()
        {
            Value = 6,
            ResourcePropertyName = nameof(CommonElement.PrizeCanceAdmin),
            ResourceType = typeof(CommonElement)
        };
    }

    public class AwardTypesApiQueryTypes : BaseValueModel<int, AwardTypesApiQueryTypes>
    {
        private AwardTypesApiQueryTypes() { }
        public int[] StatusIds { get; set; }

        ///<summary>未開獎、已中獎、未中獎、已撤單</summary> 
        public static AwardTypesApiQueryTypes AwardForSelfProduct = new AwardTypesApiQueryTypes()
        {
            Value = 0,
            StatusIds = new int[] { AwardTypes.None.Value, AwardTypes.Win.Value, AwardTypes.Loss.Value, AwardTypes.Cancel.Value }
        };
        ///<summary>已中獎、未中獎</summary>
        public static AwardTypesApiQueryTypes AwardForThirdParty = new AwardTypesApiQueryTypes()
        {
            Value = 1,
            StatusIds = new int[] { AwardTypes.Win.Value, AwardTypes.Loss.Value }
        };
        ///<summary>只有PT(老虎機)是這三個選項，融舊，已中獎、未中獎、已撤單</summary>
        public static AwardTypesApiQueryTypes AwardForPT = new AwardTypesApiQueryTypes()
        {
            Value = 2,
            StatusIds = new int[] { AwardTypes.Win.Value, AwardTypes.Loss.Value, AwardTypes.Cancel.Value }
        };
    }

}
