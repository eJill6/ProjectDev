using System.Collections.Generic;

namespace Web.Models.Base
{
    public class AllPlayConfigModel
    {
        private static List<PlayConfig> Configs = new List<PlayConfig>();
        private static object s_obj = new object();
       

        public static List<PlayConfig> GetAll()
        {            
            if (Configs != null && Configs.Count > 0)
            {
                return Configs;
            }
            else
            {
                lock (s_obj)
                {                        
     //               var realtime = RealtimePlayConfig.GetConfig();
     //               var realtime_old = RealtimePlayConfig.GetOldConfig();
     //               var selectFive = SelectFivePlayConfig.GetConfig();
     //               var selectFive_old = SelectFivePlayConfig.GetOldConfig();
     //               var welfare3D = Welfare3DPlayConfig.GetConfig();
     //               var welfare3D_old = Welfare3DPlayConfig.GetOldConfig();
     //               var pk10 = PK10PlayConfig.GetConfig();
     //               var pk10_old = PK10PlayConfig.GetOldConfig();
					//var kuaiSan = KuaiSanConfig.GetConfig();
					//var kuaiSan_old = KuaiSanConfig.GetOldConfig();
					//Configs.AddRange(realtime_old);
     //               Configs.AddRange(realtime);
     //               Configs.AddRange(selectFive);
     //               Configs.AddRange(selectFive_old);
     //               Configs.AddRange(welfare3D);
     //               Configs.AddRange(welfare3D_old);
     //               Configs.AddRange(pk10);
     //               Configs.AddRange(pk10_old);
					//Configs.AddRange(kuaiSan);
					//Configs.AddRange(kuaiSan_old);
				}
            }

            return Configs;
        }
    }

    public enum LotteryType
    {
        SSC,
        SelectFive,
        PK10,
        Welfare3D,
        HappyPoker,
        KuaiSan
    }

    public class PlayConfig
    {
        public PlayConfig()
        {
            Fields = new List<Field>();
            ShowQuickSelect = true;
            ShowRenXuanPosition = false;
        }

        public LotteryType LotteryType { get; set; }
        public int DigitNums { get; set; }
        public int? CanInput { get; set; }        
        public int PlayTypeID { get; set; }
        public string CName { get; set; }
        public string PlayTypeName { get; set; }
        public int PlayTypeRadioID { get; set; }
        public string PlayTypeRadioName { get; set; }
        public List<Field> Fields { get; set; }
        public bool ShowQuickSelect { get; set; }
        /// <summary>
        /// 号码分隔符
        /// </summary>
        public string NumberSeprator { get; set; }
        /// <summary>
        /// 是否显示任选的位置
        /// </summary>
        public bool ShowRenXuanPosition { get; set; }
        /// <summary>
        /// 最少必选的位置数
        /// </summary>
        public int MinPositions { get; set; }

        /// <summary>
        /// 玩法（新旧）
        /// </summary>
        public int Type { get; set; }
    }

    public class Field
    {
        public Field()
        {
            this.ShowQuickSelect = true;
        }
        public string Prompt { get; set; }
        public string Nums { get; set; }
        public bool ShowQuickSelect { get; set; }
    }
}