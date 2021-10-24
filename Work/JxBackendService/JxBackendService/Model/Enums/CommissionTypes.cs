using JxBackendService.Resource.Element;
using System.Linq;

namespace JxBackendService.Model.Enums
{
    //合併 M的 BonusTypeEnum 及 MSV的 CommissionRateType 、 CommissionTypeEnum
    //裡面的值都是直接搬過來，目前還沒有異動
    public class CommissionTypes : BaseIntValueModel<CommissionTypes>
    {
        private CommissionTypes() { }

        public string QueryCommissionType { get; private set; }

        public CommissionGroupType CommissionGroupType { get; private set; }

        public PlatformProduct Product { get; private set; }

        /// <summary> 无 </summary>
        public static readonly CommissionTypes No = new CommissionTypes()
        {
            Value = -1,
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.None)
        };

        /// <summary> 全部 </summary>
        public static readonly CommissionTypes All = new CommissionTypes()
        {
            Value = 0,
            ResourceType = typeof(CommonElement),
            ResourcePropertyName = nameof(CommonElement.All),
            QueryCommissionType = "All"
        };

        /// <summary> 彩票 </summary>
        public static readonly CommissionTypes Lottery = new CommissionTypes()
        {
            Value = 1,
            ResourceType = PlatformProduct.Lottery.ResourceType,
            ResourcePropertyName = PlatformProduct.Lottery.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.Lottery.Value,
            CommissionGroupType = CommissionGroupType.PlatformLottery,
            Product = PlatformProduct.Lottery,            
        };

        /// <summary> 真人娱乐 </summary>
        public static readonly CommissionTypes AG = new CommissionTypes()
        {
            Value = 2,
            ResourceType = PlatformProduct.AG.ResourceType,
            ResourcePropertyName = PlatformProduct.AG.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.AG.Value,
            CommissionGroupType = CommissionGroupType.Live,
            Product = PlatformProduct.AG,
        };

        /// <summary> 体育 </summary>
        public static readonly CommissionTypes Sport = new CommissionTypes()
        {
            Value = 3,
            ResourceType = PlatformProduct.Sport.ResourceType,
            ResourcePropertyName = PlatformProduct.Sport.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.Sport.Value,
            CommissionGroupType = CommissionGroupType.ESport,
            Product = PlatformProduct.Sport,
        };

        /// <summary> 电子游戏 </summary>
        public static readonly CommissionTypes Slot = new CommissionTypes()
        {
            Value = 4,
            ResourceType = PlatformProduct.PT.ResourceType,
            ResourcePropertyName = PlatformProduct.PT.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.PT.Value,
            CommissionGroupType = CommissionGroupType.Slot,
            Product = PlatformProduct.PT,
        };

        /// <summary> 真人AND老虎机 </summary>
        public static readonly CommissionTypes AgAndSlot = new CommissionTypes()
        {
            Value = 5,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.CommissionGroupType_Live_Backend),
            QueryCommissionType = CommissionGroupType.Live.Value,
            CommissionGroupType = CommissionGroupType.Live,
        };

        /// <summary> 棋牌 </summary>
        public static readonly CommissionTypes LC = new CommissionTypes()
        {
            Value = 6,
            ResourceType = PlatformProduct.LC.ResourceType,
            ResourcePropertyName = PlatformProduct.LC.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.LC.Value,
            CommissionGroupType = CommissionGroupType.BoardGame,
            Product = PlatformProduct.LC,
        };

        /// <summary> IM电竞 </summary>
        public static readonly CommissionTypes IM = new CommissionTypes()
        {
            Value = 7,
            ResourceType = PlatformProduct.IM.ResourceType,
            ResourcePropertyName = PlatformProduct.IM.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.IM.Value,
            CommissionGroupType = CommissionGroupType.ESport,
            Product = PlatformProduct.IM,
        };

        /// <summary> RG电竞 </summary>
        public static readonly CommissionTypes RG = new CommissionTypes()
        {
            Value = 8,
            ResourceType = PlatformProduct.RG.ResourceType,
            ResourcePropertyName = PlatformProduct.RG.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.RG.Value,
            CommissionGroupType = CommissionGroupType.ESport,
            Product = PlatformProduct.RG,
        };

        /// <summary> 体育电竞: IM + RG + Sport(沙巴) + IMSport </summary>
        public static readonly CommissionTypes ESport = new CommissionTypes()
        {
            Value = 9,
            ResourceType = CommissionGroupType.ESport.ResourceType,
            ResourcePropertyName = CommissionGroupType.ESport.ResourcePropertyName,
            QueryCommissionType = CommissionGroupType.ESport.Value,
            CommissionGroupType = CommissionGroupType.ESport,
        };

        /// <summary> IMPT电游 </summary>
        public static readonly CommissionTypes IMPT = new CommissionTypes()
        {
            Value = 10,
            ResourceType = PlatformProduct.IMPT.ResourceType,
            ResourcePropertyName = PlatformProduct.IMPT.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.IMPT.Value,
            CommissionGroupType = CommissionGroupType.Slot,
            Product = PlatformProduct.IMPT,
        };

        /// <summary> 老虎机电游: PT(JXPT) + IMPT </summary>
        public static readonly CommissionTypes SlotGame = new CommissionTypes()
        {
            Value = 11,
            ResourceType = CommissionGroupType.Slot.ResourceType,
            ResourcePropertyName = CommissionGroupType.Slot.ResourcePropertyName,
            QueryCommissionType = CommissionGroupType.Slot.Value,
            CommissionGroupType = CommissionGroupType.Slot,
        };

        /// <summary> PP电子 </summary>
        public static readonly CommissionTypes IMPP = new CommissionTypes()
        {
            Value = 12,
            ResourceType = PlatformProduct.IMPP.ResourceType,
            ResourcePropertyName = PlatformProduct.IMPP.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.IMPP.Value,
            CommissionGroupType = CommissionGroupType.Slot,
            Product = PlatformProduct.IMPP,
        };

        /// <summary> IM体育 </summary>
        public static readonly CommissionTypes IMSB = new CommissionTypes()
        {
            Value = 13,
            ResourceType = PlatformProduct.IMSport.ResourceType,
            ResourcePropertyName = PlatformProduct.IMSport.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.IMSport.Value,
            CommissionGroupType = CommissionGroupType.ESport,
            Product = PlatformProduct.IMSport,
        };

        /// <summary> 真人娱乐: AG + IMeBET </summary>
        public static readonly CommissionTypes Live = new CommissionTypes()
        {
            Value = 14,
            ResourceType = CommissionGroupType.Live.ResourceType,
            ResourcePropertyName = CommissionGroupType.Live.ResourcePropertyName,
            QueryCommissionType = CommissionGroupType.Live.Value,
            CommissionGroupType = CommissionGroupType.Live,
        };

        /// <summary> eBET真人 </summary>
        public static readonly CommissionTypes IMeBET = new CommissionTypes()
        {
            Value = 15,
            ResourceType = PlatformProduct.IMeBET.ResourceType,
            ResourcePropertyName = PlatformProduct.IMeBET.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.IMeBET.Value,
            CommissionGroupType = CommissionGroupType.Live,
            Product = PlatformProduct.IMeBET,
        };

        /// <summary> 棋牌: LC + IMBG </summary>
        public static readonly CommissionTypes BoardGame = new CommissionTypes()
        {
            Value = 16,
            ResourceType = CommissionGroupType.BoardGame.ResourceType,
            ResourcePropertyName = CommissionGroupType.BoardGame.ResourcePropertyName,
            QueryCommissionType = CommissionGroupType.BoardGame.Value,
            CommissionGroupType = CommissionGroupType.BoardGame,
        };

        /// <summary> IM棋牌 </summary>
        public static readonly CommissionTypes IMBG = new CommissionTypes()
        {
            Value = 17,
            ResourceType = PlatformProduct.IMBG.ResourceType,
            ResourcePropertyName = PlatformProduct.IMBG.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.IMBG.Value,
            CommissionGroupType = CommissionGroupType.BoardGame,
            Product = PlatformProduct.IMBG,
        };

        /// <summary> 双赢彩票 </summary>
        public static readonly CommissionTypes IMSG = new CommissionTypes()
        {
            Value = 18,
            ResourceType = PlatformProduct.IMSG.ResourceType,
            ResourcePropertyName = PlatformProduct.IMSG.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.IMSG.Value,
            CommissionGroupType = CommissionGroupType.OtherLottery,
            Product = PlatformProduct.IMSG,
        };

        /// <summary> VR彩票 </summary>
        public static readonly CommissionTypes IMVR = new CommissionTypes()
        {
            Value = 19,
            ResourceType = PlatformProduct.IMVR.ResourceType,
            ResourcePropertyName = PlatformProduct.IMVR.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.IMVR.Value,
            CommissionGroupType = CommissionGroupType.OtherLottery,
            Product = PlatformProduct.IMVR,
        };

        /// <summary> 其他彩票: 双赢彩票 + VR彩票 </summary>
        public static readonly CommissionTypes OtherLottery = new CommissionTypes()
        {
            Value = 20,
            ResourceType = CommissionGroupType.OtherLottery.ResourceType,
            ResourcePropertyName = CommissionGroupType.OtherLottery.ResourcePropertyName,
            QueryCommissionType = CommissionGroupType.OtherLottery.Value,
            CommissionGroupType = CommissionGroupType.OtherLottery,
        };

        /// <summary> 歐博真人 </summary>
        public static readonly CommissionTypes ABEB = new CommissionTypes()
        {
            Value = 21,
            ResourceType = PlatformProduct.ABEB.ResourceType,
            ResourcePropertyName = PlatformProduct.ABEB.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.ABEB.Value,
            CommissionGroupType = CommissionGroupType.Live,
            Product = PlatformProduct.ABEB,
        };

        /// <summary> PG電子 </summary>
        public static readonly CommissionTypes PGSL = new CommissionTypes()
        {
            Value = 22,
            ResourceType = PlatformProduct.PGSL.ResourceType,
            ResourcePropertyName = PlatformProduct.PGSL.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.PGSL.Value,
            CommissionGroupType = CommissionGroupType.Slot,
            Product = PlatformProduct.PGSL,
        };

        /// <summary> OB捕魚王 </summary>
        public static readonly CommissionTypes OBFI = new CommissionTypes()
        {
            Value = 23,
            ResourceType = PlatformProduct.OBFI.ResourceType,
            ResourcePropertyName = PlatformProduct.OBFI.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.OBFI.Value,
            CommissionGroupType = CommissionGroupType.Slot,
            Product = PlatformProduct.OBFI,
        };

        /// <summary> EVO真人 </summary>
        public static readonly CommissionTypes EVEB = new CommissionTypes()
        {
            Value = 24,
            ResourceType = PlatformProduct.EVEB.ResourceType,
            ResourcePropertyName = PlatformProduct.EVEB.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.EVEB.Value,
            CommissionGroupType = CommissionGroupType.Live,
            Product = PlatformProduct.EVEB,
        };

        /// <summary> BTI體育 </summary>
        public static readonly CommissionTypes BTIS = new CommissionTypes()
        {
            Value = 25,
            ResourceType = PlatformProduct.BTIS.ResourceType,
            ResourcePropertyName = PlatformProduct.BTIS.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.BTIS.Value,
            CommissionGroupType = CommissionGroupType.ESport,
            Product = PlatformProduct.BTIS,
        };

        /// <summary> OB體育 </summary>
        public static readonly CommissionTypes OBSP = new CommissionTypes()
        {
            Value = 26,
            ResourceType = PlatformProduct.OBSP.ResourceType,
            ResourcePropertyName = PlatformProduct.OBSP.ResourcePropertyName,
            QueryCommissionType = PlatformProduct.OBSP.Value,
            CommissionGroupType = CommissionGroupType.ESport,
            Product = PlatformProduct.OBSP,
        };

        /// <summary>
        /// 使用 CommissionType 反推 PlatformProduct
        /// </summary>
        public static PlatformProduct GetSinglePlatformProduct(int commissionType)
        {
            return GetAll().Where(x => x.Value == commissionType).Single().Product;
        }

        /// <summary>
        /// 使用產品查詢CommissionType
        /// </summary>
        public static int GetIntCommissionTypeByPlatformProduct(PlatformProduct platformProduct)
        {
            return GetAll().Where(x => x.Product == platformProduct).Single().Value;
        }

        public static CommissionTypes GetCommissionTypeByPlatformProduct(PlatformProduct platformProduct)
        {
            return GetAll().Where(x => x.Product == platformProduct).SingleOrDefault();
        }

        public static CommissionGroupType GetCommissionGroupTypeByPlatformProduct(PlatformProduct platformProduct)
        {
            if (platformProduct != null)
            {
                return GetAll().Where(x => x.Product == platformProduct).Single().CommissionGroupType;
            }

            return null;
        }

        /// <summary>
        /// 使用GroupType查詢第一筆CommissionType
        /// </summary>
        public static int GetCommissionTypeByCommissionGroupType(CommissionGroupType commissionGroupType)
        {
            return GetAll().Where(x => x.CommissionGroupType == commissionGroupType).First().Value;
        }

        /// <summary>
        /// 使用GroupType查詢CommissionType Group的那一筆
        /// </summary>
        public static int GetCommissionTypeGroupByCommissionGroupType(CommissionGroupType commissionGroupType)
        {
            return GetAll().Where(x => x.CommissionGroupType == commissionGroupType && (x.Product == null || x.Product == PlatformProduct.Lottery)).First().Value;
        }

        public static string GetProductValueByCommissionGroupType(CommissionGroupType commissionGroupType)
        {
            return GetAll().Where(x => x.CommissionGroupType == commissionGroupType).First().Product.Value;
        }
    }
}
