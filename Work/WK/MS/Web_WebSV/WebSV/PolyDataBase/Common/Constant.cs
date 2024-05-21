using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel;

namespace SLPolyGame.Web.Common
{
	public class Constant
	{
		public static Hashtable Foot_ht = new Hashtable();
		/// <summary>
		/// 时时彩基础奖金
		/// </summary>
		public static decimal SSC_BaseScale = (decimal)1.80;
		/// <summary>
		/// 福彩体彩基础奖金
		/// </summary>
		public static decimal FC_BaseScale = (decimal)1.75;
		/// <summary>
		/// 最高返点
		/// </summary>
		public static decimal MaxRebatePro = 0.077M;

		/// <summary>
		/// 前端投注最高返点
		/// </summary>
		public static decimal FontMaxRebatePro = 0.078M;

		/// <summary>
		/// 最高用户等级返点
		/// </summary>
		public static decimal MaxUserRebatePro = 0.07M;

		public enum LotteryType
		{
			CQSSC = 1,
			JXSSC = 2,
			FC3D = 3,
			TCPL3 = 4,
			GD115 = 6,
			CQ115 = 9,
			SD115 = 11,
			BJPK = 12,
			HSSSC = 13,
			HS115 = 14,
			XJSSC = 15,
			HSSFC = 16,
			HSPK = 17,
			JSKS = 18,
			HSSEC_MMC = 19,
			HSSEC_PK10 = 20,
			TAIJINSSC = 21,
			DEPK = 26,
			QQSSC = 27,
			ITALYPK = 28,
			ITALYSSC = 29,
			HLJSSC = 30,
            FRKS = 31,//法国快三
            LKAPK10 = 32,
            QQ5 = 33,
            VNSPK10 = 34,    //威尼斯飞艇
            QQRCPK10 = 35,   //腾讯赛车分分彩
            QQRC5PK10 = 36,    //腾讯赛车5分彩
            MQQ = 37, //QQ分分彩
            MQQ5 = 38, //QQ5分彩
            FHQQ = 39, //鳳凰騰訊分分彩
            WeixinQQ = 40,  //微信分分彩
            Jx11x5 = 41, // 江西11選5
            Js11x5 = 42, // 江蘇11選5
            VietSSC = 43, //河內分分彩
            ChichuQQ = 46, //奇趣腾讯分分彩
        }

		public static class LotteryTypeName
		{
			public static string CQSSC = "重庆时时彩";
			public static string JXSSC = "江西时时彩";
			public static string FC3D = "福彩3D";
			public static string TCPL3 = "体彩排列三";
			public static string GD115 = "广东十一选五";
			public static string CQ115 = "重庆十一选五";
			public static string SD115 = "山东十一选五";
			public static string BJPK = "北京PK拾";
			public static string HSSSC = "{0}时时彩";
			public static string HS115 = "{0} 11选5";
			public static string XJSSC = "新疆时时彩";
			public static string HSSFC = "{0} 3分彩";
			public static string HSPK = "{0} PK拾";
			public static string JSKS = "江苏快三";
			public static string HSSEC_MMC = "{0}秒秒彩";
			public static string HSSEC_PK10 = "PK拾秒秒彩";
			public static string TAIJINSSC = "天津时时彩";
			public static string DEPK = "德国PK拾";
			public static string QQSSC = "腾讯分分彩";
			public static string ITALYPK = "意大利PK拾";
			public static string ITALYSSC = "意大利分分彩";
			public static string HLJSSC = "黑龙江时时彩";
            public static string FRKS = "法国快三";
            public static string LKAPK = "幸运飞艇";
            public static string QQ5 = "腾讯5分彩";
            public static string VNSPK = "威尼斯飞艇";
            public static string QQRCPK = "腾讯赛车分分彩";
            public static string QQRC5PK = "腾讯赛车5分彩";
            public static string VietSSC = "河內分分彩";
            public static string ChichuQQ = "奇趣腾讯分分彩";
        }

		#region 專家-黑龍江
		public enum SSC_HLJPro_PlayTypeRadio
		{

			#region 五星
			wx_zhixuanfushi = 2723,
			wx_zhixuandanshi = 2724,
			wx_wxzuhe = 2634,
			wx_zx120 = 2635,
			wx_zx60 = 2636,
			wx_zx30 = 2637,
			wx_zx20 = 2638,
			wx_zx10 = 2639,
			wx_zx5 = 2640,
			wx_yifanfengshun = 2641,
			wx_haoshichengshuang = 2642,
			wx_sanxingbaoxi = 2643,
			wx_sijifacai = 2644,
			#endregion

			#region 四星
			sx_zhixuanfushi = 2725,
			sx_zhixuandanshi = 2726,
			sx_sixinzuhe = 2645,
			sx_sixinzuxuan = 2646,
			sx_zuxuan12 = 2647,
			sx_zuxuan6 = 2648,
			sx_zuxuan4 = 2649,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 2602,
			hsanx_zhixuandanshi = 2603,
			hsanx_housanzuhe = 2650,
			hsanx_zhixuanhezhi = 2651,
			hsanx_zhixuankuadu = 2652,
			hsanx_zusanfushi = 2742,
			hsanx_zusandanshi = 2653,
			hsanx_zuliufushi = 2741,
			hsanx_zuliudanshi = 2654,
			hsanx_hunhezuxuan = 2655,
			hsanx_zuxuanhezhi = 2656,
			hsanx_zuxuanbaodan = 2657,
			hsanx_hezhiweihao = 2658,
			hsanx_teshuhao = 2732,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 2600,
			zsanx_zhixuandanshi = 2601,
			zsanx_zhixuanhezhi = 2660,
			zsanx_zusanfushi = 2661,
			zsanx_zusandanshi = 2662,
			zsanx_zuliufushi = 2740,
			zsanx_zuliudanshi = 2663,
			zsanx_hunhezuxuan = 2739,
			zsanx_zuxuanhezhi = 2664,
			zsanx_housanzuhe = 2665,
			zsanx_zhixuankuadu = 2666,
			zsanx_zuxuanbaodan = 2667,
			zsanx_hezhiweihao = 2668,
			zsanx_teshuhao = 2727,
			#endregion

			#region 前三
			qsanx_zhixuanfushi = 2598,
			qsanx_zhixuandanshi = 2599,
			qsanx_housanzuhe = 2670,
			qsanx_zhixuanhezhi = 2671,
			qsanx_zhixuankuadu = 2672,
			qsanx_zusanfushi = 2738,
			qsanx_zusandanshi = 2673,
			qsanx_zuliufushi = 2737,
			qsanx_zuliudanshi = 2674,
			qsanx_hunhezuxuan = 2675,
			qsanx_zuxuanhezhi = 2676,
			qsanx_zuxuanbaodan = 2677,
			qsanx_hezhiweihao = 2678,
			qsanx_teshuhao = 2718,
			#endregion

			#region 后二
			her_zhixuanfushi = 2743,
			her_zhixuandanshi = 2744,
			her_zhixuanhezhi = 2745,
			her_zhixuankuadu = 2746,
			her_zuxuanfushi = 2747,
			her_zuxuandanshi = 2748,
			her_zuxuanhezhi = 2784,
			her_zuxuanbaodan = 2785,
			#endregion

			#region 前二
			qer_zhixuanfushi = 2743,
			qer_zhixuandanshi = 2744,
			qer_zhixuanhezhi = 2745,
			qer_zhixuankuadu = 2746,
			qer_zuxuanfushi = 2747,
			qer_zuxuandanshi = 2748,
			qer_zuxuanhezhi = 2784,
			qer_zuxuanbaodan = 2785,
			#endregion

			#region 定位胆
			dingweidan = 2686,
			#endregion

			#region 不定位
			hsan_yima = 2687,
			qsan_yima = 2688,
			hsan_erma = 2689,
			qsan_erma = 2690,
			sx_yima = 2691,
			sx_erma = 2692,
			wx_erma = 2693,
			wx_sanma = 2694,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 2755,
			her_daxiaodanshuang = 2756,
			qsan_daxiaodanshuang = 2757,
			hsan_daxiaodanshuang = 2758,
			zhdxds = 2695, // 总和大小单双
			#endregion

			#region 任二
			r2_zhixuanfushi = 2696,
			r2_zhixuandanshi = 2697,
			r2_zhixuanhezhi = 2698,
			r2_zuxuanfushi = 2699,
			r2_zuxuandanshi = 2670,
			r2_zuxuanhezhi = 2671,
			#endregion

			#region 任三
			r3_zhixuanfushi = 2702,
			r3_zhixuandanshi = 2703,
			r3_zhixuanhezhi = 2704,
			r3_zusanfushi = 2705,
			r3_zusandanshi = 2706,
			r3_zuliufushi = 2707,
			r3_zuliudanshi = 2708,
			r3_hunhezuxuan = 2709,
			r3_zuxuanhezhi = 2710,
			#endregion

			#region 任四
			r4_zhixuanfushi = 2711,
			r4_zhixuandanshi = 2712,
			r4_zuxuan24 = 2713,
			r4_zuxuan12 = 2714,
			r4_zuxuan6 = 2715,
			r4_zuxuan4 = 2716,
			#endregion

			#region 龙虎和
			lhh = 2717,
			#endregion
		}
		#endregion

		public enum SSC_CQ_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 897,
			wx_zhixuandanshi = 898,
			wx_wxzuhe = 899,
			wx_zx120 = 900,
			wx_zx60 = 901,
			wx_zx30 = 902,
			wx_zx20 = 903,
			wx_zx10 = 904,
			wx_zx5 = 905,
			wx_yifanfengshun = 906,
			wx_haoshichengshuang = 907,
			wx_sanxingbaoxi = 908,
			wx_sijifacai = 909,
			#endregion

			#region 四星
			sx_zhixuanfushi = 910,
			sx_zhixuandanshi = 911,
			sx_sixinzuhe = 912,
			sx_sixinzuxuan = 913,
			sx_zuxuan12 = 914,
			sx_zuxuan6 = 915,
			sx_zuxuan4 = 916,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 917,
			hsanx_zhixuandanshi = 918,
			hsanx_housanzuhe = 919,
			hsanx_zhixuanhezhi = 920,
			hsanx_zhixuankuadu = 921,
			hsanx_zusanfushi = 922,
			hsanx_zusandanshi = 923,
			hsanx_zuliufushi = 924,
			hsanx_zuliudanshi = 925,
			hsanx_hunhezuxuan = 926,
			hsanx_zuxuanhezhi = 927,
			hsanx_zuxuanbaodan = 928,
			hsanx_hezhiweihao = 929,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 930,
			zsanx_zhixuandanshi = 931,
			zsanx_zhixuanhezhi = 932,
			zsanx_zusanfushi = 933,
			zsanx_zusandanshi = 934,
			zsanx_zuliufushi = 935,
			zsanx_zuliudanshi = 936,
			zsanx_hunhezuxuan = 937,
			zsanx_zuxuanhezhi = 938,

			zsanx_housanzuhe = 2120,
			zsanx_zhixuankuadu = 2121,
			zsanx_zuxuanbaodan = 2123,
			zsanx_hezhiweihao = 2124,
			zsanx_teshuhao = 2125,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 939,
			qsanx_zhixuandanshi = 940,
			qsanx_housanzuhe = 941,
			qsanx_zhixuanhezhi = 942,
			qsanx_zhixuankuadu = 943,
			qsanx_zusanfushi = 944,
			qsanx_zusandanshi = 945,
			qsanx_zuliufushi = 946,
			qsanx_zuliudanshi = 947,
			qsanx_hunhezuxuan = 948,
			qsanx_zuxuanhezhi = 949,
			qsanx_zuxuanbaodan = 950,
			qsanx_hezhiweihao = 951,
			qsanx_teshuhao = 952,
			#endregion

			#region 后二
			her_zhixuanfushi = 953,
			her_zhixuandanshi = 954,
			her_zhixuanhezhi = 955,
			her_zhixuankuadu = 956,
			her_zuxuanfushi = 957,
			her_zuxuandanshi = 958,
			her_zuxuanhezhi = 959,
			her_zuxuanbaodan = 960,
			#endregion

			#region 前二
			qer_zhixuanfushi = 961,
			qer_zhixuandanshi = 962,
			qer_zhixuanhezhi = 963,
			qer_zhixuankuadu = 964,
			qer_zuxuanfushi = 965,
			qer_zuxuandanshi = 966,
			qer_zuxuanhezhi = 967,
			qer_zuxuanbaodan = 968,
			#endregion

			#region 定位胆
			dingweidan = 969,
			#endregion

			#region 不定位
			hsan_yima = 970,
			qsan_yima = 971,
			hsan_erma = 972,
			qsan_erma = 973,
			sx_yima = 974,
			sx_erma = 975,
			wx_erma = 976,
			wx_sanma = 977,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 978,
			her_daxiaodanshuang = 979,
			qsan_daxiaodanshuang = 980,
			hsan_daxiaodanshuang = 981,
			#endregion

			#region 任二
			r2_zhixuanfushi = 982,
			r2_zhixuandanshi = 983,
			r2_zhixuanhezhi = 984,
			r2_zuxuanfushi = 985,
			r2_zuxuandanshi = 986,
			r2_zuxuanhezhi = 987,
			#endregion

			#region 任三
			r3_zhixuanfushi = 988,
			r3_zhixuandanshi = 989,
			r3_zhixuanhezhi = 990,
			r3_zusanfushi = 991,
			r3_zusandanshi = 992,
			r3_zuliufushi = 993,
			r3_zuliudanshi = 994,
			r3_hunhezuxuan = 995,
			r3_zuxuanhezhi = 996,
			#endregion

			#region 任四
			r4_zhixuanfushi = 997,
			r4_zhixuandanshi = 998,
			r4_zuxuan24 = 999,
			r4_zuxuan12 = 1000,
			r4_zuxuan6 = 1001,
			r4_zuxuan4 = 1002,
			#endregion

			#region 龙虎和
			lhh = 1003,
			#endregion

			#region 总和大小单双
			zhdxds = 1004,
			#endregion

			#region 特殊号
			tsh = 1005
			#endregion

		}

		public enum FC_3D_PlayTypeRadio
		{
			x3_zhixuanfushi = 1006,
			x3_zhixuandanshi = 1007,
			x3_zhixuanhezhi = 1008,
			x3_zusanfushi = 1009,
			x3_zusandanshi = 1010,
			x3_zuliufushi = 1011,
			x3_zuliudanshi = 1012,
			x3_hunhezuxuan = 1013,
			x3_zuxuanhezhi = 1014,

			x2_qzhixuanfushi = 1015,
			x2_qzhixuandanshi = 1016,
			x2_hzhixuanfushi = 1017,
			x2_hzhixuandanshi = 1018,

			x2_qzuxuanfushi = 1019,
			x2_qzuxuandanshi = 1020,
			x2_hzuxuanfushi = 1021,
			x2_hzuxuandanshi = 1022,

			x3_dingweidan = 1023,

			x1_budingwei = 1024,
			x2_budingwei = 1025,

			x2_qdaxiaodanshaung = 1026,
			x2_hdaxiaodanshaung = 1027

		}

		public enum SF_GD_PlayTypeRadio
		{
			#region 三码
			x3_zhixuanfushi = 1028,
			x3_zhixuandanshi = 1029,
			x3_zuxuanfushi = 1030,
			x3_zuxuandanshi = 1031,
			x3_zuxuandantuo = 1032,
			#endregion

			#region 二码
			x2_zhixuanfushi = 1033,
			x2_zhixuandanshi = 1034,
			x2_zuxuanfushi = 1035,
			x2_zuxuandanshi = 1036,
			x2_zuxuandantuo = 1037,
			#endregion

			#region 不定位
			x3_budingwei = 1038,
			#endregion

			#region 定位胆
			x3_dingweidan = 1039,
			#endregion

			#region 趣味
			dingdanshaung = 1040,
			caizhongwei = 1041,
			#endregion

			#region 任选复式
			renfu_1x1 = 1042,
			renfu_2x2 = 1043,
			renfu_3x3 = 1044,
			renfu_4x4 = 1045,
			renfu_5x5 = 1046,
			renfu_6x5 = 1047,
			renfu_7x5 = 1048,
			renfu_8x5 = 1049,
			#endregion

			#region 任选单式
			rendan_1x1 = 1050,
			rendan_2x2 = 1051,
			rendan_3x3 = 1052,
			rendan_4x4 = 1053,
			rendan_5x5 = 1054,
			rendan_6x5 = 1055,
			rendan_7x5 = 1056,
			rendan_8x5 = 1057,
			#endregion

			#region 任选胆拖
			rendt_2x2 = 1058,
			rendt_3x3 = 1059,
			rendt_4x4 = 1060,
			rendt_5x5 = 1061,
			rendt_6x5 = 1062,
			rendt_7x5 = 1063,
			rendt_8x5 = 1064,
			#endregion

		}

		public enum PK_BJ_PlayTypeRadio
		{
			lm = 1065,
			dwd = 1066,
			gyh = 1067,
			x2_fushi = 1068,
			x2_danshi = 1069,
			x3_fushi = 1070,
			x3_danshi = 1071,
			x4_fushi = 1072,
			x4_danshi = 1073,
			x5_fushi = 1074,
			x5_danshi = 1075,
			x6_fushi = 1076,
			x6_danshi = 1077,
			x1_fushi = 2072,
			lh_1vs10 = 2075,
			lh_2vs9 = 2076,
			lh_3vs8 = 2077,
			lh_4vs7 = 2078,
			lh_5vs6 = 2079,
			dxds_x1 = 2090,
			dxds_x2 = 2091,
			dxds_x3 = 2092,
			dxds_x4 = 2093,
			dxds_x5 = 2094,
			dxds_x6 = 2095,
			dxds_x7 = 2096,
			dxds_x8 = 2097,
			dxds_x9 = 2098,
			dxds_x10 = 2099
		}

		public enum PK_De_PlayTypeRadio
		{
			lm = 2194,
			dwd = 2195,
			gyh = 2196,
			x2_fushi = 2197,
			x2_danshi = 2198,
			x3_fushi = 2199,
			x3_danshi = 2200,
			x4_fushi = 2201,
			x4_danshi = 2202,
			x5_fushi = 2203,
			x5_danshi = 2204,
			x6_fushi = 2205,
			x6_danshi = 2206,
			x1_fushi = 2207,
			lh_1vs10 = 2208,
			lh_2vs9 = 2209,
			lh_3vs8 = 2210,
			lh_4vs7 = 2211,
			lh_5vs6 = 2212,
			dxds_x1 = 2213,
			dxds_x2 = 2214,
			dxds_x3 = 2215,
			dxds_x4 = 2216,
			dxds_x5 = 2217,
			dxds_x6 = 2218,
			dxds_x7 = 2219,
			dxds_x8 = 2220,
			dxds_x9 = 2221,
			dxds_x10 = 2222
		}

        public enum PK_LKA_PlayTypeRadio
        {
            lm = 2831,
            dwd = 2832,
            gyh = 2833,
            x2_fushi = 2834,
            x2_danshi = 2835,
            x3_fushi = 2836,
            x3_danshi = 2837,
            x4_fushi = 2838,
            x4_danshi = 2839,
            x5_fushi = 2840,
            x5_danshi = 2841,
            x6_fushi = 2842,
            x6_danshi = 2843,
            x1_fushi = 2844,
            lh_1vs10 = 2845,
            lh_2vs9 = 2846,
            lh_3vs8 = 2847,
            lh_4vs7 = 2848,
            lh_5vs6 = 2849,
            dxds_x1 = 2850,
            dxds_x2 = 2851,
            dxds_x3 = 2852,
            dxds_x4 = 2853,
            dxds_x5 = 2854,
            dxds_x6 = 2855,
            dxds_x7 = 2856,
            dxds_x8 = 2857,
            dxds_x9 = 2858,
            dxds_x10 = 2859
        }

        public enum PK_VNS_PlayTypeRadio
        {
            lm = 3044,
            dwd = 3045,
            gyh = 3046,
            x2_fushi = 3047,
            x2_danshi = 3048,
            x3_fushi = 3049,
            x3_danshi = 3050,
            x4_fushi = 3051,
            x4_danshi = 3052,
            x5_fushi = 3053,
            x5_danshi = 3054,
            x6_fushi = 3055,
            x6_danshi = 3056,
            x1_fushi = 3057,
            lh_1vs10 = 3058,
            lh_2vs9 = 3059,
            lh_3vs8 = 3060,
            lh_4vs7 = 3061,
            lh_5vs6 = 3062,
            dxds_x1 = 3063,
            dxds_x2 = 3064,
            dxds_x3 = 3065,
            dxds_x4 = 3066,
            dxds_x5 = 3067,
            dxds_x6 = 3068,
            dxds_x7 = 3069,
            dxds_x8 = 3070,
            dxds_x9 = 3071,
            dxds_x10 = 3072
        }

        public enum PK_QQRC_PlayTypeRadio
        {
            lm = 3101,
            dwd = 3102,
            gyh = 3103,
            x2_fushi = 3104,
            x2_danshi = 3105,
            x3_fushi = 3106,
            x3_danshi = 3107,
            x4_fushi = 3108,
            x4_danshi = 3109,
            x5_fushi = 3110,
            x5_danshi = 3111,
            x6_fushi = 3112,
            x6_danshi = 3113,
            x1_fushi = 3114,
            lh_1vs10 = 3115,
            lh_2vs9 = 3116,
            lh_3vs8 = 3117,
            lh_4vs7 = 3118,
            lh_5vs6 = 3119,
            dxds_x1 = 3120,
            dxds_x2 = 3121,
            dxds_x3 = 3122,
            dxds_x4 = 3123,
            dxds_x5 = 3124,
            dxds_x6 = 3125,
            dxds_x7 = 3126,
            dxds_x8 = 3127,
            dxds_x9 = 3128,
            dxds_x10 = 3129
        }

        public enum PK_QQRC5_PlayTypeRadio
        {
            lm = 3158,
            dwd = 3159,
            gyh = 3160,
            x2_fushi = 3161,
            x2_danshi = 3162,
            x3_fushi = 3163,
            x3_danshi = 3164,
            x4_fushi = 3165,
            x4_danshi = 3166,
            x5_fushi = 3167,
            x5_danshi = 3168,
            x6_fushi = 3169,
            x6_danshi = 3170,
            x1_fushi = 3171,
            lh_1vs10 = 3172,
            lh_2vs9 = 3173,
            lh_3vs8 = 3174,
            lh_4vs7 = 3175,
            lh_5vs6 = 3176,
            dxds_x1 = 3177,
            dxds_x2 = 3178,
            dxds_x3 = 3179,
            dxds_x4 = 3180,
            dxds_x5 = 3181,
            dxds_x6 = 3182,
            dxds_x7 = 3183,
            dxds_x8 = 3184,
            dxds_x9 = 3185,
            dxds_x10 = 3186
        }

        public enum PK_Italy_PlayTypeRadio
		{
			lm = 2407,
			dwd = 2408,
			gyh = 2409,
			x2_fushi = 2410,
			x2_danshi = 2411,
			x3_fushi = 2412,
			x3_danshi = 2413,
			x4_fushi = 2414,
			x4_danshi = 2415,
			x5_fushi = 2416,
			x5_danshi = 2417,
			x6_fushi = 2418,
			x6_danshi = 2419,
			x1_fushi = 2420,
			lh_1vs10 = 2421,
			lh_2vs9 = 2422,
			lh_3vs8 = 2423,
			lh_4vs7 = 2424,
			lh_5vs6 = 2425,
			dxds_x1 = 2426,
			dxds_x2 = 2427,
			dxds_x3 = 2428,
			dxds_x4 = 2429,
			dxds_x5 = 2430,
			dxds_x6 = 2431,
			dxds_x7 = 2432,
			dxds_x8 = 2433,
			dxds_x9 = 2434,
			dxds_x10 = 2435
		}

		public enum FC_PL_PlayTypeRadio
		{
			x3_zhixuanfushi = 1078,
			x3_zhixuandanshi = 1079,
			x3_zhixuanhezhi = 1080,
			x3_zusanfushi = 1081,
			x3_zusandanshi = 1082,
			x3_zuliufushi = 1083,
			x3_zuliudanshi = 1084,
			x3_hunhezuxuan = 1085,
			x3_zuxuanhezhi = 1086,

			x2_qzhixuanfushi = 1087,
			x2_qzhixuandanshi = 1088,
			x2_hzhixuanfushi = 1089,
			x2_hzhixuandanshi = 1090,

			x2_qzuxuanfushi = 1091,
			x2_qzuxuandanshi = 1092,
			x2_hzuxuanfushi = 1093,
			x2_hzuxuandanshi = 1094,

			x3_dingweidan = 1095,

			x1_budingwei = 1096,
			x2_budingwei = 1097,

			x2_qdaxiaodanshaung = 1098,
			x2_hdaxiaodanshaung = 1099

		}

		public enum SF_SD_PlayTypeRadio
		{
			#region 三码
			x3_zhixuanfushi = 1100,
			x3_zhixuandanshi = 1101,
			x3_zuxuanfushi = 1102,
			x3_zuxuandanshi = 1103,
			x3_zuxuandantuo = 1104,
			#endregion

			#region 二码
			x2_zhixuanfushi = 1105,
			x2_zhixuandanshi = 1106,
			x2_zuxuanfushi = 1107,
			x2_zuxuandanshi = 1108,
			x2_zuxuandantuo = 1109,
			#endregion

			#region 不定位
			x3_budingwei = 1110,
			#endregion

			#region 定位胆
			x3_dingweidan = 1111,
			#endregion

			#region 趣味
			dingdanshaung = 1112,
			caizhongwei = 1113,
			#endregion

			#region 任选复式
			renfu_1x1 = 1114,
			renfu_2x2 = 1115,
			renfu_3x3 = 1116,
			renfu_4x4 = 1117,
			renfu_5x5 = 1118,
			renfu_6x5 = 1119,
			renfu_7x5 = 1120,
			renfu_8x5 = 1121,
			#endregion

			#region 任选单式
			rendan_1x1 = 1122,
			rendan_2x2 = 1123,
			rendan_3x3 = 1124,
			rendan_4x4 = 1125,
			rendan_5x5 = 1126,
			rendan_6x5 = 1127,
			rendan_7x5 = 1128,
			rendan_8x5 = 1129,
			#endregion

			#region 任选胆拖
			rendt_2x2 = 1130,
			rendt_3x3 = 1131,
			rendt_4x4 = 1132,
			rendt_5x5 = 1133,
			rendt_6x5 = 1134,
			rendt_7x5 = 1135,
			rendt_8x5 = 1136,
			#endregion

		}

		public enum SSC_HS_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 1137,
			wx_zhixuandanshi = 1138,
			wx_wxzuhe = 1139,
			wx_zx120 = 1140,
			wx_zx60 = 1141,
			wx_zx30 = 1142,
			wx_zx20 = 1143,
			wx_zx10 = 1144,
			wx_zx5 = 1145,
			wx_yifanfengshun = 1146,
			wx_haoshichengshuang = 1147,
			wx_sanxingbaoxi = 1148,
			wx_sijifacai = 1149,
			#endregion

			#region 四星
			sx_zhixuanfushi = 1150,
			sx_zhixuandanshi = 1151,
			sx_sixinzuhe = 1152,
			sx_sixinzuxuan = 1153,
			sx_zuxuan12 = 1154,
			sx_zuxuan6 = 1155,
			sx_zuxuan4 = 1156,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 1157,
			hsanx_zhixuandanshi = 1158,
			hsanx_housanzuhe = 1159,
			hsanx_zhixuanhezhi = 1160,
			hsanx_zhixuankuadu = 1161,
			hsanx_zusanfushi = 1162,
			hsanx_zusandanshi = 1163,
			hsanx_zuliufushi = 1164,
			hsanx_zuliudanshi = 1165,
			hsanx_hunhezuxuan = 1166,
			hsanx_zuxuanhezhi = 1167,
			hsanx_zuxuanbaodan = 1168,
			hsanx_hezhiweihao = 1169,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 1170,
			zsanx_zhixuandanshi = 1171,
			zsanx_zhixuanhezhi = 1172,
			zsanx_zusanfushi = 1173,
			zsanx_zusandanshi = 1174,
			zsanx_zuliufushi = 1175,
			zsanx_zuliudanshi = 1176,
			zsanx_hunhezuxuan = 1177,
			zsanx_zuxuanhezhi = 1178,

			zsanx_housanzuhe = 2126,
			zsanx_zhixuankuadu = 2127,
			zsanx_zuxuanbaodan = 2128,
			zsanx_hezhiweihao = 2129,
			zsanx_teshuhao = 2130,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 1179,
			qsanx_zhixuandanshi = 1180,
			qsanx_housanzuhe = 1181,
			qsanx_zhixuanhezhi = 1182,
			qsanx_zhixuankuadu = 1183,
			qsanx_zusanfushi = 1184,
			qsanx_zusandanshi = 1185,
			qsanx_zuliufushi = 1186,
			qsanx_zuliudanshi = 1187,
			qsanx_hunhezuxuan = 1188,
			qsanx_zuxuanhezhi = 1189,
			qsanx_zuxuanbaodan = 1190,
			qsanx_hezhiweihao = 1191,
			qsanx_teshuhao = 1192,
			#endregion

			#region 后二
			her_zhixuanfushi = 1193,
			her_zhixuandanshi = 1194,
			her_zhixuanhezhi = 1195,
			her_zhixuankuadu = 1196,
			her_zuxuanfushi = 1197,
			her_zuxuandanshi = 1198,
			her_zuxuanhezhi = 1199,
			her_zuxuanbaodan = 1200,
			#endregion

			#region 前二
			qer_zhixuanfushi = 1201,
			qer_zhixuandanshi = 1202,
			qer_zhixuanhezhi = 1203,
			qer_zhixuankuadu = 1204,
			qer_zuxuanfushi = 1205,
			qer_zuxuandanshi = 1206,
			qer_zuxuanhezhi = 1207,
			qer_zuxuanbaodan = 1208,
			#endregion

			#region 定位胆
			dingweidan = 1209,
			#endregion

			#region 不定位
			hsan_yima = 1210,
			qsan_yima = 1211,
			hsan_erma = 1212,
			qsan_erma = 1213,
			sx_yima = 1214,
			sx_erma = 1215,
			wx_erma = 1216,
			wx_sanma = 1217,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 1218,
			her_daxiaodanshuang = 1219,
			qsan_daxiaodanshuang = 1220,
			hsan_daxiaodanshuang = 1221,
			#endregion

			#region 任二
			r2_zhixuanfushi = 1222,
			r2_zhixuandanshi = 1223,
			r2_zhixuanhezhi = 1224,
			r2_zuxuanfushi = 1225,
			r2_zuxuandanshi = 1226,
			r2_zuxuanhezhi = 1227,
			#endregion

			#region 任三
			r3_zhixuanfushi = 1228,
			r3_zhixuandanshi = 1229,
			r3_zhixuanhezhi = 1230,
			r3_zusanfushi = 1231,
			r3_zusandanshi = 1232,
			r3_zuliufushi = 1233,
			r3_zuliudanshi = 1234,
			r3_hunhezuxuan = 1235,
			r3_zuxuanhezhi = 1236,
			#endregion

			#region 任四
			r4_zhixuanfushi = 1237,
			r4_zhixuandanshi = 1238,
			r4_zuxuan24 = 1239,
			r4_zuxuan12 = 1240,
			r4_zuxuan6 = 1241,
			r4_zuxuan4 = 1242,
			#endregion

			#region 龙虎和
			lhh = 1243,
			#endregion

			#region 总和大小单双
			zhdxds = 1244,
			#endregion

			#region 特殊号
			tsh = 1245
			#endregion

		}

		public enum SF_HS_PlayTypeRadio
		{
			#region 三码
			x3_zhixuanfushi = 1246,
			x3_zhixuandanshi = 1247,
			x3_zuxuanfushi = 1248,
			x3_zuxuandanshi = 1249,
			x3_zuxuandantuo = 1250,
			#endregion

			#region 二码
			x2_zhixuanfushi = 1251,
			x2_zhixuandanshi = 1252,
			x2_zuxuanfushi = 1253,
			x2_zuxuandanshi = 1254,
			x2_zuxuandantuo = 1255,
			#endregion

			#region 不定位
			x3_budingwei = 1256,
			#endregion

			#region 定位胆
			x3_dingweidan = 1257,
			#endregion

			#region 趣味
			dingdanshaung = 1258,
			caizhongwei = 1259,
			#endregion

			#region 任选复式
			renfu_1x1 = 1260,
			renfu_2x2 = 1261,
			renfu_3x3 = 1262,
			renfu_4x4 = 1263,
			renfu_5x5 = 1264,
			renfu_6x5 = 1265,
			renfu_7x5 = 1266,
			renfu_8x5 = 1267,
			#endregion

			#region 任选单式
			rendan_1x1 = 1268,
			rendan_2x2 = 1269,
			rendan_3x3 = 1270,
			rendan_4x4 = 1271,
			rendan_5x5 = 1272,
			rendan_6x5 = 1273,
			rendan_7x5 = 1274,
			rendan_8x5 = 1275,
			#endregion

			#region 任选胆拖
			rendt_2x2 = 1276,
			rendt_3x3 = 1277,
			rendt_4x4 = 1278,
			rendt_5x5 = 1279,
			rendt_6x5 = 1280,
			rendt_7x5 = 1281,
			rendt_8x5 = 1282,
			#endregion

		}

		public enum SSC_XJ_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 1283,
			wx_zhixuandanshi = 1284,
			wx_wxzuhe = 1285,
			wx_zx120 = 1286,
			wx_zx60 = 1287,
			wx_zx30 = 1288,
			wx_zx20 = 1289,
			wx_zx10 = 1290,
			wx_zx5 = 1291,
			wx_yifanfengshun = 1292,
			wx_haoshichengshuang = 1293,
			wx_sanxingbaoxi = 1294,
			wx_sijifacai = 1295,
			#endregion

			#region 四星
			sx_zhixuanfushi = 1296,
			sx_zhixuandanshi = 1297,
			sx_sixinzuhe = 1298,
			sx_sixinzuxuan = 1299,
			sx_zuxuan12 = 1300,
			sx_zuxuan6 = 1301,
			sx_zuxuan4 = 1302,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 1303,
			hsanx_zhixuandanshi = 1304,
			hsanx_housanzuhe = 1305,
			hsanx_zhixuanhezhi = 1306,
			hsanx_zhixuankuadu = 1307,
			hsanx_zusanfushi = 1308,
			hsanx_zusandanshi = 1309,
			hsanx_zuliufushi = 1310,
			hsanx_zuliudanshi = 1311,
			hsanx_hunhezuxuan = 1312,
			hsanx_zuxuanhezhi = 1313,
			hsanx_zuxuanbaodan = 1314,
			hsanx_hezhiweihao = 1315,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 1316,
			zsanx_zhixuandanshi = 1317,
			zsanx_zhixuanhezhi = 1318,
			zsanx_zusanfushi = 1319,
			zsanx_zusandanshi = 1320,
			zsanx_zuliufushi = 1321,
			zsanx_zuliudanshi = 1322,
			zsanx_hunhezuxuan = 1323,
			zsanx_zuxuanhezhi = 1324,

			zsanx_housanzuhe = 2131,
			zsanx_zhixuankuadu = 2132,
			zsanx_zuxuanbaodan = 2133,
			zsanx_hezhiweihao = 2134,
			zsanx_teshuhao = 2135,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 1325,
			qsanx_zhixuandanshi = 1326,
			qsanx_housanzuhe = 1327,
			qsanx_zhixuanhezhi = 1328,
			qsanx_zhixuankuadu = 1329,
			qsanx_zusanfushi = 1330,
			qsanx_zusandanshi = 1331,
			qsanx_zuliufushi = 1332,
			qsanx_zuliudanshi = 1333,
			qsanx_hunhezuxuan = 1334,
			qsanx_zuxuanhezhi = 1335,
			qsanx_zuxuanbaodan = 1336,
			qsanx_hezhiweihao = 1337,
			qsanx_teshuhao = 1338,
			#endregion

			#region 后二
			her_zhixuanfushi = 1339,
			her_zhixuandanshi = 1340,
			her_zhixuanhezhi = 1341,
			her_zhixuankuadu = 1342,
			her_zuxuanfushi = 1343,
			her_zuxuandanshi = 1344,
			her_zuxuanhezhi = 1345,
			her_zuxuanbaodan = 1346,
			#endregion

			#region 前二
			qer_zhixuanfushi = 1347,
			qer_zhixuandanshi = 1348,
			qer_zhixuanhezhi = 1349,
			qer_zhixuankuadu = 1350,
			qer_zuxuanfushi = 1351,
			qer_zuxuandanshi = 1352,
			qer_zuxuanhezhi = 1353,
			qer_zuxuanbaodan = 1354,
			#endregion

			#region 定位胆
			dingweidan = 1355,
			#endregion

			#region 不定位
			hsan_yima = 1356,
			qsan_yima = 1357,
			hsan_erma = 1358,
			qsan_erma = 1359,
			sx_yima = 1360,
			sx_erma = 1361,
			wx_erma = 1362,
			wx_sanma = 1363,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 1364,
			her_daxiaodanshuang = 1365,
			qsan_daxiaodanshuang = 1366,
			hsan_daxiaodanshuang = 1367,
			#endregion

			#region 任二
			r2_zhixuanfushi = 1368,
			r2_zhixuandanshi = 1369,
			r2_zhixuanhezhi = 1370,
			r2_zuxuanfushi = 1371,
			r2_zuxuandanshi = 1372,
			r2_zuxuanhezhi = 1373,
			#endregion

			#region 任三
			r3_zhixuanfushi = 1374,
			r3_zhixuandanshi = 1375,
			r3_zhixuanhezhi = 1376,
			r3_zusanfushi = 1377,
			r3_zusandanshi = 1378,
			r3_zuliufushi = 1379,
			r3_zuliudanshi = 1380,
			r3_hunhezuxuan = 1381,
			r3_zuxuanhezhi = 1382,
			#endregion

			#region 任四
			r4_zhixuanfushi = 1383,
			r4_zhixuandanshi = 1384,
			r4_zuxuan24 = 1385,
			r4_zuxuan12 = 1386,
			r4_zuxuan6 = 1387,
			r4_zuxuan4 = 1388,
			#endregion

			#region 龙虎和
			lhh = 1389,
			#endregion

			#region 总和大小单双
			zhdxds = 1390,
			#endregion

			#region 特殊号
			tsh = 1391
			#endregion

		}

		public enum SSC_SF_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 1392,
			wx_zhixuandanshi = 1393,
			wx_wxzuhe = 1394,
			wx_zx120 = 1395,
			wx_zx60 = 1396,
			wx_zx30 = 1397,
			wx_zx20 = 1398,
			wx_zx10 = 1399,
			wx_zx5 = 1400,
			wx_yifanfengshun = 1401,
			wx_haoshichengshuang = 1402,
			wx_sanxingbaoxi = 1403,
			wx_sijifacai = 1404,
			#endregion

			#region 四星
			sx_zhixuanfushi = 1405,
			sx_zhixuandanshi = 1406,
			sx_sixinzuhe = 1407,
			sx_sixinzuxuan = 1408,
			sx_zuxuan12 = 1409,
			sx_zuxuan6 = 1410,
			sx_zuxuan4 = 1411,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 1412,
			hsanx_zhixuandanshi = 1413,
			hsanx_housanzuhe = 1414,
			hsanx_zhixuanhezhi = 1415,
			hsanx_zhixuankuadu = 1416,
			hsanx_zusanfushi = 1417,
			hsanx_zusandanshi = 1418,
			hsanx_zuliufushi = 1419,
			hsanx_zuliudanshi = 1420,
			hsanx_hunhezuxuan = 1421,
			hsanx_zuxuanhezhi = 1422,
			hsanx_zuxuanbaodan = 1423,
			hsanx_hezhiweihao = 1424,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 1425,
			zsanx_zhixuandanshi = 1426,
			zsanx_zhixuanhezhi = 1427,
			zsanx_zusanfushi = 1428,
			zsanx_zusandanshi = 1429,
			zsanx_zuliufushi = 1430,
			zsanx_zuliudanshi = 1431,
			zsanx_hunhezuxuan = 1432,
			zsanx_zuxuanhezhi = 1433,

			zsanx_housanzuhe = 2136,
			zsanx_zhixuankuadu = 2137,
			zsanx_zuxuanbaodan = 2138,
			zsanx_hezhiweihao = 2139,
			zsanx_teshuhao = 2140,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 1434,
			qsanx_zhixuandanshi = 1435,
			qsanx_housanzuhe = 1436,
			qsanx_zhixuanhezhi = 1437,
			qsanx_zhixuankuadu = 1438,
			qsanx_zusanfushi = 1439,
			qsanx_zusandanshi = 1440,
			qsanx_zuliufushi = 1441,
			qsanx_zuliudanshi = 1442,
			qsanx_hunhezuxuan = 1443,
			qsanx_zuxuanhezhi = 1444,
			qsanx_zuxuanbaodan = 1445,
			qsanx_hezhiweihao = 1446,
			qsanx_teshuhao = 1447,
			#endregion

			#region 后二
			her_zhixuanfushi = 1448,
			her_zhixuandanshi = 1449,
			her_zhixuanhezhi = 1450,
			her_zhixuankuadu = 1451,
			her_zuxuanfushi = 1452,
			her_zuxuandanshi = 1453,
			her_zuxuanhezhi = 1454,
			her_zuxuanbaodan = 1455,
			#endregion

			#region 前二
			qer_zhixuanfushi = 1456,
			qer_zhixuandanshi = 1457,
			qer_zhixuanhezhi = 1458,
			qer_zhixuankuadu = 1459,
			qer_zuxuanfushi = 1460,
			qer_zuxuandanshi = 1461,
			qer_zuxuanhezhi = 1462,
			qer_zuxuanbaodan = 1463,
			#endregion

			#region 定位胆
			dingweidan = 1464,
			#endregion

			#region 不定位
			hsan_yima = 1465,
			qsan_yima = 1466,
			hsan_erma = 1467,
			qsan_erma = 1468,
			sx_yima = 1469,
			sx_erma = 1470,
			wx_erma = 1471,
			wx_sanma = 1472,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 1473,
			her_daxiaodanshuang = 1474,
			qsan_daxiaodanshuang = 1475,
			hsan_daxiaodanshuang = 1476,
			#endregion

			#region 任二
			r2_zhixuanfushi = 1477,
			r2_zhixuandanshi = 1478,
			r2_zhixuanhezhi = 1479,
			r2_zuxuanfushi = 1480,
			r2_zuxuandanshi = 1481,
			r2_zuxuanhezhi = 1482,
			#endregion

			#region 任三
			r3_zhixuanfushi = 1483,
			r3_zhixuandanshi = 1484,
			r3_zhixuanhezhi = 1485,
			r3_zusanfushi = 1486,
			r3_zusandanshi = 1487,
			r3_zuliufushi = 1488,
			r3_zuliudanshi = 1489,
			r3_hunhezuxuan = 1490,
			r3_zuxuanhezhi = 1491,
			#endregion

			#region 任四
			r4_zhixuanfushi = 1492,
			r4_zhixuandanshi = 1493,
			r4_zuxuan24 = 1494,
			r4_zuxuan12 = 1495,
			r4_zuxuan6 = 1496,
			r4_zuxuan4 = 1497,
			#endregion

			#region 龙虎和
			lhh = 1498,
			#endregion

			#region 总和大小单双
			zhdxds = 1499,
			#endregion

			#region 特殊号
			tsh = 1500
			#endregion

		}

		public enum PK_HS_PlayTypeRadio
		{
			lm = 1501,
			dwd = 1502,
			gyh = 1503,
			x2_fushi = 1504,
			x2_danshi = 1505,
			x3_fushi = 1506,
			x3_danshi = 1507,
			x4_fushi = 1508,
			x4_danshi = 1509,
			x5_fushi = 1510,
			x5_danshi = 1511,
			x6_fushi = 1512,
			x6_danshi = 1513,
			x1_fushi = 2073,
			lh_1vs10 = 2080,
			lh_2vs9 = 2081,
			lh_3vs8 = 2082,
			lh_4vs7 = 2083,
			lh_5vs6 = 2084,
			dxds_x1 = 2100,
			dxds_x2 = 2101,
			dxds_x3 = 2102,
			dxds_x4 = 2103,
			dxds_x5 = 2104,
			dxds_x6 = 2105,
			dxds_x7 = 2106,
			dxds_x8 = 2107,
			dxds_x9 = 2108,
			dxds_x10 = 2109
		}

		public enum SSC_MMC_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 1514,
			wx_zhixuandanshi = 1515,
			wx_wxzuhe = 1516,
			wx_zx120 = 1517,
			wx_zx60 = 1518,
			wx_zx30 = 1519,
			wx_zx20 = 1520,
			wx_zx10 = 1521,
			wx_zx5 = 1522,
			wx_yifanfengshun = 1523,
			wx_haoshichengshuang = 1524,
			wx_sanxingbaoxi = 1525,
			wx_sijifacai = 1526,
			#endregion

			#region 四星
			sx_zhixuanfushi = 1527,
			sx_zhixuandanshi = 1528,
			sx_sixinzuhe = 1529,
			sx_sixinzuxuan = 1530,
			sx_zuxuan12 = 1531,
			sx_zuxuan6 = 1532,
			sx_zuxuan4 = 1533,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 1534,
			hsanx_zhixuandanshi = 1535,
			hsanx_housanzuhe = 1536,
			hsanx_zhixuanhezhi = 1537,
			hsanx_zhixuankuadu = 1538,
			hsanx_zusanfushi = 1539,
			hsanx_zusandanshi = 1540,
			hsanx_zuliufushi = 1541,
			hsanx_zuliudanshi = 1542,
			hsanx_hunhezuxuan = 1543,
			hsanx_zuxuanhezhi = 1544,
			hsanx_zuxuanbaodan = 1545,
			hsanx_hezhiweihao = 1546,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 1547,
			zsanx_zhixuandanshi = 1548,
			zsanx_zhixuanhezhi = 1549,
			zsanx_zusanfushi = 1550,
			zsanx_zusandanshi = 1551,
			zsanx_zuliufushi = 1552,
			zsanx_zuliudanshi = 1553,
			zsanx_hunhezuxuan = 1554,
			zsanx_zuxuanhezhi = 1555,

			zsanx_housanzuhe = 2141,
			zsanx_zhixuankuadu = 2142,
			zsanx_zuxuanbaodan = 2143,
			zsanx_hezhiweihao = 2144,
			zsanx_teshuhao = 2145,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 1556,
			qsanx_zhixuandanshi = 1557,
			qsanx_housanzuhe = 1558,
			qsanx_zhixuanhezhi = 1559,
			qsanx_zhixuankuadu = 1560,
			qsanx_zusanfushi = 1561,
			qsanx_zusandanshi = 1562,
			qsanx_zuliufushi = 1563,
			qsanx_zuliudanshi = 1564,
			qsanx_hunhezuxuan = 1565,
			qsanx_zuxuanhezhi = 1566,
			qsanx_zuxuanbaodan = 1567,
			qsanx_hezhiweihao = 1568,
			qsanx_teshuhao = 1569,
			#endregion

			#region 后二
			her_zhixuanfushi = 1570,
			her_zhixuandanshi = 1571,
			her_zhixuanhezhi = 1572,
			her_zhixuankuadu = 1573,
			her_zuxuanfushi = 1574,
			her_zuxuandanshi = 1575,
			her_zuxuanhezhi = 1576,
			her_zuxuanbaodan = 1577,
			#endregion

			#region 前二
			qer_zhixuanfushi = 1578,
			qer_zhixuandanshi = 1579,
			qer_zhixuanhezhi = 1580,
			qer_zhixuankuadu = 1581,
			qer_zuxuanfushi = 1582,
			qer_zuxuandanshi = 1583,
			qer_zuxuanhezhi = 1584,
			qer_zuxuanbaodan = 1585,
			#endregion

			#region 定位胆
			dingweidan = 1586,
			#endregion

			#region 不定位
			hsan_yima = 1587,
			qsan_yima = 1588,
			hsan_erma = 1589,
			qsan_erma = 1590,
			sx_yima = 1591,
			sx_erma = 1592,
			wx_erma = 1593,
			wx_sanma = 1594,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 1595,
			her_daxiaodanshuang = 1596,
			qsan_daxiaodanshuang = 1597,
			hsan_daxiaodanshuang = 1598,
			#endregion

			#region 任二
			r2_zhixuanfushi = 1599,
			r2_zhixuandanshi = 1600,
			r2_zhixuanhezhi = 1601,
			r2_zuxuanfushi = 1602,
			r2_zuxuandanshi = 1603,
			r2_zuxuanhezhi = 1604,
			#endregion

			#region 任三
			r3_zhixuanfushi = 1605,
			r3_zhixuandanshi = 1606,
			r3_zhixuanhezhi = 1607,
			r3_zusanfushi = 1608,
			r3_zusandanshi = 1609,
			r3_zuliufushi = 1610,
			r3_zuliudanshi = 1611,
			r3_hunhezuxuan = 1612,
			r3_zuxuanhezhi = 1613,
			#endregion

			#region 任四
			r4_zhixuanfushi = 1614,
			r4_zhixuandanshi = 1615,
			r4_zuxuan24 = 1616,
			r4_zuxuan12 = 1617,
			r4_zuxuan6 = 1618,
			r4_zuxuan4 = 1619,
			#endregion

			#region 龙虎和
			lhh = 1620,
			#endregion

			#region 总和大小单双
			zhdxds = 1621,
			#endregion

			#region 特殊号
			tsh = 1622
			#endregion

		}

		public enum PK_MM_PlayTypeRadio
		{
			lm = 1623,
			dwd = 1624,
			gyh = 1625,
			x2_fushi = 1626,
			x2_danshi = 1627,
			x3_fushi = 1628,
			x3_danshi = 1629,
			x4_fushi = 1630,
			x4_danshi = 1631,
			x5_fushi = 1632,
			x5_danshi = 1633,
			x6_fushi = 1634,
			x6_danshi = 1635,
			x1_fushi = 2074,
			lh_1vs10 = 2085,
			lh_2vs9 = 2086,
			lh_3vs8 = 2087,
			lh_4vs7 = 2088,
			lh_5vs6 = 2089,
			dxds_x1 = 2110,
			dxds_x2 = 2111,
			dxds_x3 = 2112,
			dxds_x4 = 2113,
			dxds_x5 = 2114,
			dxds_x6 = 2115,
			dxds_x7 = 2116,
			dxds_x8 = 2117,
			dxds_x9 = 2118,
			dxds_x10 = 2119
		}

		public enum SSC_TJ_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 1636,
			wx_zhixuandanshi = 1637,
			wx_wxzuhe = 1638,
			wx_zx120 = 1639,
			wx_zx60 = 1640,
			wx_zx30 = 1641,
			wx_zx20 = 1642,
			wx_zx10 = 1643,
			wx_zx5 = 1644,
			wx_yifanfengshun = 1645,
			wx_haoshichengshuang = 1646,
			wx_sanxingbaoxi = 1647,
			wx_sijifacai = 1648,
			#endregion

			#region 四星
			sx_zhixuanfushi = 1649,
			sx_zhixuandanshi = 1650,
			sx_sixinzuhe = 1651,
			sx_sixinzuxuan = 1652,
			sx_zuxuan12 = 1653,
			sx_zuxuan6 = 1654,
			sx_zuxuan4 = 1655,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 1656,
			hsanx_zhixuandanshi = 1657,
			hsanx_housanzuhe = 1658,
			hsanx_zhixuanhezhi = 1659,
			hsanx_zhixuankuadu = 1660,
			hsanx_zusanfushi = 1661,
			hsanx_zusandanshi = 1662,
			hsanx_zuliufushi = 1663,
			hsanx_zuliudanshi = 1664,
			hsanx_hunhezuxuan = 1665,
			hsanx_zuxuanhezhi = 1666,
			hsanx_zuxuanbaodan = 1667,
			hsanx_hezhiweihao = 1668,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 1669,
			zsanx_zhixuandanshi = 1670,
			zsanx_zhixuanhezhi = 1671,
			zsanx_zusanfushi = 1672,
			zsanx_zusandanshi = 1673,
			zsanx_zuliufushi = 1674,
			zsanx_zuliudanshi = 1675,
			zsanx_hunhezuxuan = 1676,
			zsanx_zuxuanhezhi = 1677,

			zsanx_housanzuhe = 2146,
			zsanx_zhixuankuadu = 2147,
			zsanx_zuxuanbaodan = 2148,
			zsanx_hezhiweihao = 2149,
			zsanx_teshuhao = 2150,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 1678,
			qsanx_zhixuandanshi = 1679,
			qsanx_housanzuhe = 1680,
			qsanx_zhixuanhezhi = 1681,
			qsanx_zhixuankuadu = 1682,
			qsanx_zusanfushi = 1683,
			qsanx_zusandanshi = 1684,
			qsanx_zuliufushi = 1685,
			qsanx_zuliudanshi = 1686,
			qsanx_hunhezuxuan = 1687,
			qsanx_zuxuanhezhi = 1688,
			qsanx_zuxuanbaodan = 1689,
			qsanx_hezhiweihao = 1690,
			qsanx_teshuhao = 1691,
			#endregion

			#region 后二
			her_zhixuanfushi = 1692,
			her_zhixuandanshi = 1693,
			her_zhixuanhezhi = 1694,
			her_zhixuankuadu = 1695,
			her_zuxuanfushi = 1696,
			her_zuxuandanshi = 1697,
			her_zuxuanhezhi = 1698,
			her_zuxuanbaodan = 1699,
			#endregion

			#region 前二
			qer_zhixuanfushi = 1700,
			qer_zhixuandanshi = 1701,
			qer_zhixuanhezhi = 1702,
			qer_zhixuankuadu = 1703,
			qer_zuxuanfushi = 1704,
			qer_zuxuandanshi = 1705,
			qer_zuxuanhezhi = 1706,
			qer_zuxuanbaodan = 1707,
			#endregion

			#region 定位胆
			dingweidan = 1708,
			#endregion

			#region 不定位
			hsan_yima = 1709,
			qsan_yima = 1710,
			hsan_erma = 1711,
			qsan_erma = 1712,
			sx_yima = 1713,
			sx_erma = 1714,
			wx_erma = 1715,
			wx_sanma = 1716,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 1717,
			her_daxiaodanshuang = 1718,
			qsan_daxiaodanshuang = 1719,
			hsan_daxiaodanshuang = 1720,
			#endregion

			#region 任二
			r2_zhixuanfushi = 1721,
			r2_zhixuandanshi = 1722,
			r2_zhixuanhezhi = 1723,
			r2_zuxuanfushi = 1724,
			r2_zuxuandanshi = 1725,
			r2_zuxuanhezhi = 1726,
			#endregion

			#region 任三
			r3_zhixuanfushi = 1727,
			r3_zhixuandanshi = 1728,
			r3_zhixuanhezhi = 1729,
			r3_zusanfushi = 1730,
			r3_zusandanshi = 1731,
			r3_zuliufushi = 1732,
			r3_zuliudanshi = 1733,
			r3_hunhezuxuan = 1734,
			r3_zuxuanhezhi = 1735,
			#endregion

			#region 任四
			r4_zhixuanfushi = 1736,
			r4_zhixuandanshi = 1737,
			r4_zuxuan24 = 1738,
			r4_zuxuan12 = 1739,
			r4_zuxuan6 = 1740,
			r4_zuxuan4 = 1741,
			#endregion

			#region 龙虎和
			lhh = 1742,
			#endregion

			#region 总和大小单双
			zhdxds = 1743,
			#endregion

			#region 特殊号
			tsh = 1744
			#endregion

		}

		public enum SSC_BINGGO_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 1745,
			wx_zhixuandanshi = 1746,
			wx_wxzuhe = 1747,
			wx_zx120 = 1748,
			wx_zx60 = 1749,
			wx_zx30 = 1750,
			wx_zx20 = 1751,
			wx_zx10 = 1752,
			wx_zx5 = 1753,
			wx_yifanfengshun = 1754,
			wx_haoshichengshuang = 1755,
			wx_sanxingbaoxi = 1756,
			wx_sijifacai = 1757,
			#endregion

			#region 四星
			sx_zhixuanfushi = 1758,
			sx_zhixuandanshi = 1759,
			sx_sixinzuhe = 1760,
			sx_sixinzuxuan = 1761,
			sx_zuxuan12 = 1762,
			sx_zuxuan6 = 1763,
			sx_zuxuan4 = 1764,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 1765,
			hsanx_zhixuandanshi = 1766,
			hsanx_housanzuhe = 1767,
			hsanx_zhixuanhezhi = 1768,
			hsanx_zhixuankuadu = 1769,
			hsanx_zusanfushi = 1770,
			hsanx_zusandanshi = 1771,
			hsanx_zuliufushi = 1772,
			hsanx_zuliudanshi = 1773,
			hsanx_hunhezuxuan = 1774,
			hsanx_zuxuanhezhi = 1775,
			hsanx_zuxuanbaodan = 1776,
			hsanx_hezhiweihao = 1777,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 1778,
			zsanx_zhixuandanshi = 1779,
			zsanx_zhixuanhezhi = 1780,
			zsanx_zusanfushi = 1781,
			zsanx_zusandanshi = 1782,
			zsanx_zuliufushi = 1783,
			zsanx_zuliudanshi = 1784,
			zsanx_hunhezuxuan = 1785,
			zsanx_zuxuanhezhi = 1786,

			zsanx_housanzuhe = 2151,
			zsanx_zhixuankuadu = 2152,
			zsanx_zuxuanbaodan = 2153,
			zsanx_hezhiweihao = 2154,
			zsanx_teshuhao = 2155,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 1787,
			qsanx_zhixuandanshi = 1788,
			qsanx_housanzuhe = 1789,
			qsanx_zhixuanhezhi = 1790,
			qsanx_zhixuankuadu = 1791,
			qsanx_zusanfushi = 1792,
			qsanx_zusandanshi = 1793,
			qsanx_zuliufushi = 1794,
			qsanx_zuliudanshi = 1795,
			qsanx_hunhezuxuan = 1796,
			qsanx_zuxuanhezhi = 1797,
			qsanx_zuxuanbaodan = 1798,
			qsanx_hezhiweihao = 1799,
			qsanx_teshuhao = 1800,
			#endregion

			#region 后二
			her_zhixuanfushi = 1801,
			her_zhixuandanshi = 1802,
			her_zhixuanhezhi = 1803,
			her_zhixuankuadu = 1804,
			her_zuxuanfushi = 1805,
			her_zuxuandanshi = 1806,
			her_zuxuanhezhi = 1807,
			her_zuxuanbaodan = 1808,
			#endregion

			#region 前二
			qer_zhixuanfushi = 1809,
			qer_zhixuandanshi = 1810,
			qer_zhixuanhezhi = 1811,
			qer_zhixuankuadu = 1812,
			qer_zuxuanfushi = 1813,
			qer_zuxuandanshi = 1814,
			qer_zuxuanhezhi = 1815,
			qer_zuxuanbaodan = 1816,
			#endregion

			#region 定位胆
			dingweidan = 1817,
			#endregion

			#region 不定位
			hsan_yima = 1818,
			qsan_yima = 1819,
			hsan_erma = 1820,
			qsan_erma = 1821,
			sx_yima = 1822,
			sx_erma = 1823,
			wx_erma = 1824,
			wx_sanma = 1825,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 1826,
			her_daxiaodanshuang = 1827,
			qsan_daxiaodanshuang = 1828,
			hsan_daxiaodanshuang = 1829,
			#endregion

			#region 任二
			r2_zhixuanfushi = 1830,
			r2_zhixuandanshi = 1831,
			r2_zhixuanhezhi = 1832,
			r2_zuxuanfushi = 1833,
			r2_zuxuandanshi = 1834,
			r2_zuxuanhezhi = 1835,
			#endregion

			#region 任三
			r3_zhixuanfushi = 1836,
			r3_zhixuandanshi = 1837,
			r3_zhixuanhezhi = 1838,
			r3_zusanfushi = 1839,
			r3_zusandanshi = 1840,
			r3_zuliufushi = 1841,
			r3_zuliudanshi = 1842,
			r3_hunhezuxuan = 1843,
			r3_zuxuanhezhi = 1844,
			#endregion

			#region 任四
			r4_zhixuanfushi = 1845,
			r4_zhixuandanshi = 1846,
			r4_zuxuan24 = 1847,
			r4_zuxuan12 = 1848,
			r4_zuxuan6 = 1849,
			r4_zuxuan4 = 1850,
			#endregion

			#region 龙虎和
			lhh = 1851,
			#endregion

			#region 总和大小单双
			zhdxds = 1852,
			#endregion

			#region 特殊号
			tsh = 1853
			#endregion

		}

		public enum SSC_KENO_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 1854,
			wx_zhixuandanshi = 1855,
			wx_wxzuhe = 1856,
			wx_zx120 = 1857,
			wx_zx60 = 1858,
			wx_zx30 = 1859,
			wx_zx20 = 1860,
			wx_zx10 = 1861,
			wx_zx5 = 1862,
			wx_yifanfengshun = 1863,
			wx_haoshichengshuang = 1864,
			wx_sanxingbaoxi = 1865,
			wx_sijifacai = 1866,
			#endregion

			#region 四星
			sx_zhixuanfushi = 1867,
			sx_zhixuandanshi = 1868,
			sx_sixinzuhe = 1869,
			sx_sixinzuxuan = 1870,
			sx_zuxuan12 = 1871,
			sx_zuxuan6 = 1872,
			sx_zuxuan4 = 1873,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 1874,
			hsanx_zhixuandanshi = 1875,
			hsanx_housanzuhe = 1876,
			hsanx_zhixuanhezhi = 1877,
			hsanx_zhixuankuadu = 1878,
			hsanx_zusanfushi = 1879,
			hsanx_zusandanshi = 1880,
			hsanx_zuliufushi = 1881,
			hsanx_zuliudanshi = 1882,
			hsanx_hunhezuxuan = 1883,
			hsanx_zuxuanhezhi = 1884,
			hsanx_zuxuanbaodan = 1885,
			hsanx_hezhiweihao = 1886,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 1887,
			zsanx_zhixuandanshi = 1888,
			zsanx_zhixuanhezhi = 1889,
			zsanx_zusanfushi = 1890,
			zsanx_zusandanshi = 1891,
			zsanx_zuliufushi = 1892,
			zsanx_zuliudanshi = 1893,
			zsanx_hunhezuxuan = 1894,
			zsanx_zuxuanhezhi = 1895,

			zsanx_housanzuhe = 2156,
			zsanx_zhixuankuadu = 2157,
			zsanx_zuxuanbaodan = 2158,
			zsanx_hezhiweihao = 2159,
			zsanx_teshuhao = 2160,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 1896,
			qsanx_zhixuandanshi = 1897,
			qsanx_housanzuhe = 1898,
			qsanx_zhixuanhezhi = 1899,
			qsanx_zhixuankuadu = 1900,
			qsanx_zusanfushi = 1901,
			qsanx_zusandanshi = 1902,
			qsanx_zuliufushi = 1903,
			qsanx_zuliudanshi = 1904,
			qsanx_hunhezuxuan = 1905,
			qsanx_zuxuanhezhi = 1906,
			qsanx_zuxuanbaodan = 1907,
			qsanx_hezhiweihao = 1908,
			qsanx_teshuhao = 1909,
			#endregion

			#region 后二
			her_zhixuanfushi = 1910,
			her_zhixuandanshi = 1911,
			her_zhixuanhezhi = 1912,
			her_zhixuankuadu = 1913,
			her_zuxuanfushi = 1914,
			her_zuxuandanshi = 1915,
			her_zuxuanhezhi = 1916,
			her_zuxuanbaodan = 1917,
			#endregion

			#region 前二
			qer_zhixuanfushi = 1918,
			qer_zhixuandanshi = 1919,
			qer_zhixuanhezhi = 1920,
			qer_zhixuankuadu = 1921,
			qer_zuxuanfushi = 1922,
			qer_zuxuandanshi = 1923,
			qer_zuxuanhezhi = 1924,
			qer_zuxuanbaodan = 1925,
			#endregion

			#region 定位胆
			dingweidan = 1926,
			#endregion

			#region 不定位
			hsan_yima = 1927,
			qsan_yima = 1928,
			hsan_erma = 1929,
			qsan_erma = 1930,
			sx_yima = 1931,
			sx_erma = 1932,
			wx_erma = 1933,
			wx_sanma = 1934,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 1935,
			her_daxiaodanshuang = 1936,
			qsan_daxiaodanshuang = 1937,
			hsan_daxiaodanshuang = 1938,
			#endregion

			#region 任二
			r2_zhixuanfushi = 1939,
			r2_zhixuandanshi = 1940,
			r2_zhixuanhezhi = 1941,
			r2_zuxuanfushi = 1942,
			r2_zuxuandanshi = 1943,
			r2_zuxuanhezhi = 1944,
			#endregion

			#region 任三
			r3_zhixuanfushi = 1945,
			r3_zhixuandanshi = 1946,
			r3_zhixuanhezhi = 1947,
			r3_zusanfushi = 1948,
			r3_zusandanshi = 1949,
			r3_zuliufushi = 1950,
			r3_zuliudanshi = 1951,
			r3_hunhezuxuan = 1952,
			r3_zuxuanhezhi = 1953,
			#endregion

			#region 任四
			r4_zhixuanfushi = 1954,
			r4_zhixuandanshi = 1955,
			r4_zuxuan24 = 1956,
			r4_zuxuan12 = 1957,
			r4_zuxuan6 = 1958,
			r4_zuxuan4 = 1959,
			#endregion

			#region 龙虎和
			lhh = 1960,
			#endregion

			#region 总和大小单双
			zhdxds = 1961,
			#endregion

			#region 特殊号
			tsh = 1962
			#endregion

		}

		public enum SSC_KOR_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 1963,
			wx_zhixuandanshi = 1964,
			wx_wxzuhe = 1965,
			wx_zx120 = 1966,
			wx_zx60 = 1967,
			wx_zx30 = 1968,
			wx_zx20 = 1969,
			wx_zx10 = 1970,
			wx_zx5 = 1971,
			wx_yifanfengshun = 1972,
			wx_haoshichengshuang = 1973,
			wx_sanxingbaoxi = 1974,
			wx_sijifacai = 1975,
			#endregion

			#region 四星
			sx_zhixuanfushi = 1976,
			sx_zhixuandanshi = 1977,
			sx_sixinzuhe = 1978,
			sx_sixinzuxuan = 1979,
			sx_zuxuan12 = 1980,
			sx_zuxuan6 = 1981,
			sx_zuxuan4 = 1982,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 1983,
			hsanx_zhixuandanshi = 1984,
			hsanx_housanzuhe = 1985,
			hsanx_zhixuanhezhi = 1986,
			hsanx_zhixuankuadu = 1987,
			hsanx_zusanfushi = 1988,
			hsanx_zusandanshi = 1989,
			hsanx_zuliufushi = 1990,
			hsanx_zuliudanshi = 1991,
			hsanx_hunhezuxuan = 1992,
			hsanx_zuxuanhezhi = 1993,
			hsanx_zuxuanbaodan = 1994,
			hsanx_hezhiweihao = 1995,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 1996,
			zsanx_zhixuandanshi = 1997,
			zsanx_zhixuanhezhi = 1998,
			zsanx_zusanfushi = 1999,
			zsanx_zusandanshi = 2000,
			zsanx_zuliufushi = 2001,
			zsanx_zuliudanshi = 2002,
			zsanx_hunhezuxuan = 2003,
			zsanx_zuxuanhezhi = 2004,

			zsanx_housanzuhe = 2161,
			zsanx_zhixuankuadu = 2162,
			zsanx_zuxuanbaodan = 2163,
			zsanx_hezhiweihao = 2164,
			zsanx_teshuhao = 2165,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 2005,
			qsanx_zhixuandanshi = 2006,
			qsanx_housanzuhe = 2007,
			qsanx_zhixuanhezhi = 2008,
			qsanx_zhixuankuadu = 2009,
			qsanx_zusanfushi = 2010,
			qsanx_zusandanshi = 2011,
			qsanx_zuliufushi = 2012,
			qsanx_zuliudanshi = 2013,
			qsanx_hunhezuxuan = 2014,
			qsanx_zuxuanhezhi = 2015,
			qsanx_zuxuanbaodan = 2016,
			qsanx_hezhiweihao = 2017,
			qsanx_teshuhao = 2018,
			#endregion

			#region 后二
			her_zhixuanfushi = 2019,
			her_zhixuandanshi = 2020,
			her_zhixuanhezhi = 2021,
			her_zhixuankuadu = 2022,
			her_zuxuanfushi = 2023,
			her_zuxuandanshi = 2024,
			her_zuxuanhezhi = 2025,
			her_zuxuanbaodan = 2026,
			#endregion

			#region 前二
			qer_zhixuanfushi = 2027,
			qer_zhixuandanshi = 2028,
			qer_zhixuanhezhi = 2029,
			qer_zhixuankuadu = 2030,
			qer_zuxuanfushi = 2031,
			qer_zuxuandanshi = 2032,
			qer_zuxuanhezhi = 2033,
			qer_zuxuanbaodan = 2034,
			#endregion

			#region 定位胆
			dingweidan = 2035,
			#endregion

			#region 不定位
			hsan_yima = 2036,
			qsan_yima = 2037,
			hsan_erma = 2038,
			qsan_erma = 2039,
			sx_yima = 2040,
			sx_erma = 2041,
			wx_erma = 2042,
			wx_sanma = 2043,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 2044,
			her_daxiaodanshuang = 2045,
			qsan_daxiaodanshuang = 2046,
			hsan_daxiaodanshuang = 2047,
			#endregion

			#region 任二
			r2_zhixuanfushi = 2048,
			r2_zhixuandanshi = 2049,
			r2_zhixuanhezhi = 2050,
			r2_zuxuanfushi = 2051,
			r2_zuxuandanshi = 2052,
			r2_zuxuanhezhi = 2053,
			#endregion

			#region 任三
			r3_zhixuanfushi = 2054,
			r3_zhixuandanshi = 2055,
			r3_zhixuanhezhi = 2056,
			r3_zusanfushi = 2057,
			r3_zusandanshi = 2058,
			r3_zuliufushi = 2059,
			r3_zuliudanshi = 2060,
			r3_hunhezuxuan = 2061,
			r3_zuxuanhezhi = 2062,
			#endregion

			#region 任四
			r4_zhixuanfushi = 2063,
			r4_zhixuandanshi = 2064,
			r4_zuxuan24 = 2065,
			r4_zuxuan12 = 2066,
			r4_zuxuan6 = 2067,
			r4_zuxuan4 = 2068,
			#endregion

			#region 龙虎和
			lhh = 2069,
			#endregion

			#region 总和大小单双
			zhdxds = 2070,
			#endregion

			#region 特殊号
			tsh = 2071
			#endregion

		}

		public enum SSC_QQ_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 2265,
			wx_zhixuandanshi = 2266,
			wx_wxzuhe = 2267,
			wx_zx120 = 2268,
			wx_zx60 = 2269,
			wx_zx30 = 2270,
			wx_zx20 = 2271,
			wx_zx10 = 2272,
			wx_zx5 = 2273,
			wx_yifanfengshun = 2274,
			wx_haoshichengshuang = 2275,
			wx_sanxingbaoxi = 2276,
			wx_sijifacai = 2277,
			#endregion

			#region 四星
			sx_zhixuanfushi = 2278,
			sx_zhixuandanshi = 2279,
			sx_sixinzuhe = 2280,
			sx_sixinzuxuan = 2281,
			sx_zuxuan12 = 2282,
			sx_zuxuan6 = 2283,
			sx_zuxuan4 = 2284,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 2285,
			hsanx_zhixuandanshi = 2286,
			hsanx_housanzuhe = 2287,
			hsanx_zhixuanhezhi = 2288,
			hsanx_zhixuankuadu = 2289,
			hsanx_zusanfushi = 2290,
			hsanx_zusandanshi = 2291,
			hsanx_zuliufushi = 2292,
			hsanx_zuliudanshi = 2293,
			hsanx_hunhezuxuan = 2294,
			hsanx_zuxuanhezhi = 2295,
			hsanx_zuxuanbaodan = 2296,
			hsanx_hezhiweihao = 2297,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 2298,
			zsanx_zhixuandanshi = 2299,
			zsanx_zhixuanhezhi = 2300,
			zsanx_zusanfushi = 2301,
			zsanx_zusandanshi = 2302,
			zsanx_zuliufushi = 2303,
			zsanx_zuliudanshi = 2304,
			zsanx_hunhezuxuan = 2305,
			zsanx_zuxuanhezhi = 2306,

			zsanx_housanzuhe = 2374,
			zsanx_zhixuankuadu = 2375,
			zsanx_zuxuanbaodan = 2376,
			zsanx_hezhiweihao = 2377,
			zsanx_teshuhao = 2378,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 2307,
			qsanx_zhixuandanshi = 2308,
			qsanx_housanzuhe = 2309,
			qsanx_zhixuanhezhi = 2310,
			qsanx_zhixuankuadu = 2311,
			qsanx_zusanfushi = 2312,
			qsanx_zusandanshi = 2313,
			qsanx_zuliufushi = 2314,
			qsanx_zuliudanshi = 2315,
			qsanx_hunhezuxuan = 2316,
			qsanx_zuxuanhezhi = 2317,
			qsanx_zuxuanbaodan = 2318,
			qsanx_hezhiweihao = 2319,
			qsanx_teshuhao = 2320,
			#endregion

			#region 后二
			her_zhixuanfushi = 2321,
			her_zhixuandanshi = 2322,
			her_zhixuanhezhi = 2323,
			her_zhixuankuadu = 2324,
			her_zuxuanfushi = 2325,
			her_zuxuandanshi = 2326,
			her_zuxuanhezhi = 2327,
			her_zuxuanbaodan = 2328,
			#endregion

			#region 前二
			qer_zhixuanfushi = 2329,
			qer_zhixuandanshi = 2330,
			qer_zhixuanhezhi = 2331,
			qer_zhixuankuadu = 2332,
			qer_zuxuanfushi = 2333,
			qer_zuxuandanshi = 2334,
			qer_zuxuanhezhi = 2335,
			qer_zuxuanbaodan = 2336,
			#endregion

			#region 定位胆
			dingweidan = 2337,
			#endregion

			#region 不定位
			hsan_yima = 2338,
			qsan_yima = 2339,
			hsan_erma = 2340,
			qsan_erma = 2341,
			sx_yima = 2342,
			sx_erma = 2343,
			wx_erma = 2344,
			wx_sanma = 2345,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 2346,
			her_daxiaodanshuang = 2347,
			qsan_daxiaodanshuang = 2348,
			hsan_daxiaodanshuang = 2349,
			#endregion

			#region 任二
			r2_zhixuanfushi = 2350,
			r2_zhixuandanshi = 2351,
			r2_zhixuanhezhi = 2352,
			r2_zuxuanfushi = 2353,
			r2_zuxuandanshi = 2354,
			r2_zuxuanhezhi = 2355,
			#endregion

			#region 任三
			r3_zhixuanfushi = 2356,
			r3_zhixuandanshi = 2357,
			r3_zhixuanhezhi = 2358,
			r3_zusanfushi = 2359,
			r3_zusandanshi = 2360,
			r3_zuliufushi = 2361,
			r3_zuliudanshi = 2362,
			r3_hunhezuxuan = 2363,
			r3_zuxuanhezhi = 2364,
			#endregion

			#region 任四
			r4_zhixuanfushi = 2365,
			r4_zhixuandanshi = 2366,
			r4_zuxuan24 = 2367,
			r4_zuxuan12 = 2368,
			r4_zuxuan6 = 2369,
			r4_zuxuan4 = 2370,
			#endregion

			#region 龙虎和
			lhh = 2371,
			#endregion

			#region 总和大小单双
			zhdxds = 2372,
			#endregion

			#region 特殊号
			tsh = 2373
			#endregion

		}

        public enum SSC_QQ5_PlayTypeRadio
        {
            #region 五星
            wx_zhixuanfushi = 2902,
            wx_zhixuandanshi = 2903,
            wx_wxzuhe = 2904,
            wx_zx120 = 2905,
            wx_zx60 = 2906,
            wx_zx30 = 2907,
            wx_zx20 = 2908,
            wx_zx10 = 2909,
            wx_zx5 = 2910,
            wx_yifanfengshun = 2911,
            wx_haoshichengshuang = 2912,
            wx_sanxingbaoxi = 2913,
            wx_sijifacai = 2914,
            #endregion

            #region 四星
            sx_zhixuanfushi = 2915,
            sx_zhixuandanshi = 2916,
            sx_sixinzuhe = 2917,
            sx_sixinzuxuan = 2918,
            sx_zuxuan12 = 2919,
            sx_zuxuan6 = 2920,
            sx_zuxuan4 = 2921,
            #endregion

            #region 后三
            hsanx_zhixuanfushi = 2922,
            hsanx_zhixuandanshi = 2923,
            hsanx_housanzuhe = 2924,
            hsanx_zhixuanhezhi = 2925,
            hsanx_zhixuankuadu = 2926,
            hsanx_zusanfushi = 2927,
            hsanx_zusandanshi = 2928,
            hsanx_zuliufushi = 2929,
            hsanx_zuliudanshi = 2930,
            hsanx_hunhezuxuan = 2931,
            hsanx_zuxuanhezhi = 2932,
            hsanx_zuxuanbaodan = 2933,
            hsanx_hezhiweihao = 2934,
            #endregion

            #region 中三
            zsanx_zhixuanfushi = 2935,
            zsanx_zhixuandanshi = 2936,
            zsanx_zhixuanhezhi = 2937,
            zsanx_zusanfushi = 2938,
            zsanx_zusandanshi = 2939,
            zsanx_zuliufushi = 2940,
            zsanx_zuliudanshi = 2941,
            zsanx_hunhezuxuan = 2942,
            zsanx_zuxuanhezhi = 2943,

            zsanx_housanzuhe = 3011,
            zsanx_zhixuankuadu = 3012,
            zsanx_zuxuanbaodan = 3013,
            zsanx_hezhiweihao = 3014,
            zsanx_teshuhao = 3015,
            #endregion

            #region 前三
            qsanx_zhixuanfushi = 2944,
            qsanx_zhixuandanshi = 2945,
            qsanx_housanzuhe = 2946,
            qsanx_zhixuanhezhi = 2947,
            qsanx_zhixuankuadu = 2948,
            qsanx_zusanfushi = 2949,
            qsanx_zusandanshi = 2950,
            qsanx_zuliufushi = 2951,
            qsanx_zuliudanshi = 2952,
            qsanx_hunhezuxuan = 2953,
            qsanx_zuxuanhezhi = 2954,
            qsanx_zuxuanbaodan = 2955,
            qsanx_hezhiweihao = 2956,
            qsanx_teshuhao = 2957,
            #endregion

            #region 后二
            her_zhixuanfushi = 2958,
            her_zhixuandanshi = 2959,
            her_zhixuanhezhi = 2960,
            her_zhixuankuadu = 2961,
            her_zuxuanfushi = 2962,
            her_zuxuandanshi = 2963,
            her_zuxuanhezhi = 2964,
            her_zuxuanbaodan = 2965,
            #endregion

            #region 前二
            qer_zhixuanfushi = 2966,
            qer_zhixuandanshi = 2967,
            qer_zhixuanhezhi = 2968,
            qer_zhixuankuadu = 2969,
            qer_zuxuanfushi = 2970,
            qer_zuxuandanshi = 2971,
            qer_zuxuanhezhi = 2972,
            qer_zuxuanbaodan = 2973,
            #endregion

            #region 定位胆
            dingweidan = 2974,
            #endregion

            #region 不定位
            hsan_yima = 2975,
            qsan_yima = 2976,
            hsan_erma = 2977,
            qsan_erma = 2978,
            sx_yima = 2979,
            sx_erma = 2980,
            wx_erma = 2981,
            wx_sanma = 2982,
            #endregion

            #region 大小单双
            qer_daxiaodanshuang = 2983,
            her_daxiaodanshuang = 2984,
            qsan_daxiaodanshuang = 2985,
            hsan_daxiaodanshuang = 2986,
            #endregion

            #region 任二
            r2_zhixuanfushi = 2987,
            r2_zhixuandanshi = 2988,
            r2_zhixuanhezhi = 2989,
            r2_zuxuanfushi = 2990,
            r2_zuxuandanshi = 2991,
            r2_zuxuanhezhi = 2992,
            #endregion

            #region 任三
            r3_zhixuanfushi = 2993,
            r3_zhixuandanshi = 2994,
            r3_zhixuanhezhi = 2995,
            r3_zusanfushi = 2996,
            r3_zusandanshi = 2997,
            r3_zuliufushi = 2998,
            r3_zuliudanshi = 2999,
            r3_hunhezuxuan = 3000,
            r3_zuxuanhezhi = 3001,
            #endregion

            #region 任四
            r4_zhixuanfushi = 3002,
            r4_zhixuandanshi = 3003,
            r4_zuxuan24 = 3004,
            r4_zuxuan12 = 3005,
            r4_zuxuan6 = 3006,
            r4_zuxuan4 = 3007,
            #endregion

            #region 龙虎和
            lhh = 3008,
            #endregion

            #region 总和大小单双
            zhdxds = 3009,
            #endregion

            #region 特殊号
            tsh = 3010
            #endregion

        }

        public enum SSC_WeixinQQ_PlayTypeRadio
        {
            #region 五星
            wx_zhixuanfushi = 3697, //直选复式
            wx_zhixuandanshi = 3698, //直选单式
            wx_wxzuhe = 3699, //五星组合
            wx_zx120 = 3700, //组选120
            wx_zx60 = 3701, //组选60
            wx_zx30 = 3702, //组选30
            wx_zx20 = 3703, //组选20
            wx_zx10 = 3704, //组选10
            wx_zx5 = 3705, //组选5
            wx_yifanfengshun = 3706, //一帆风顺
            wx_haoshichengshuang = 3707, //好事成双
            wx_sanxingbaoxi = 3708, //三星报喜
            wx_sijifacai = 3709, //四季发财
            #endregion

            #region 四星
            sx_zhixuanfushi = 3710, //直选复式
            sx_zhixuandanshi = 3711, //直选单式
            sx_sixinzuhe = 3712, //四星组合
            sx_sixinzuxuan = 3713, //组选24
            sx_zuxuan12 = 3714, //组选12
            sx_zuxuan6 = 3715, //组选6
            sx_zuxuan4 = 3716, //组选4
            #endregion

            #region 后三
            hsanx_zhixuanfushi = 3717, //直选复式
            hsanx_zhixuandanshi = 3718, //直选单式
            hsanx_housanzuhe = 3719, //后三组合
            hsanx_zhixuanhezhi = 3720, //直选和值
            hsanx_zhixuankuadu = 3721, //直选跨度
            hsanx_zusanfushi = 3722, //组三复式
            hsanx_zusandanshi = 3723, //组三单式
            hsanx_zuliufushi = 3724, //组六复式
            hsanx_zuliudanshi = 3725, //组六单式
            hsanx_hunhezuxuan = 3726, //混合组选
            hsanx_zuxuanhezhi = 3727, //组选和值
            hsanx_zuxuanbaodan = 3728, //组选包胆
            hsanx_hezhiweihao = 3729, //和值尾数
            #endregion

            #region 中三
            zsanx_zhixuanfushi = 3730, //直选复式
            zsanx_zhixuandanshi = 3731, //直选单式
            zsanx_zhixuanhezhi = 3732, //直选和值
            zsanx_zusanfushi = 3733, //组三复式
            zsanx_zusandanshi = 3734, //组三单式
            zsanx_zuliufushi = 3735, //组六复式
            zsanx_zuliudanshi = 3736, //组六单式
            zsanx_hunhezuxuan = 3737, //混合组选
            zsanx_zuxuanhezhi = 3738, //组选和值

            //zsanx_housanzuhe = 3271,
            //zsanx_zhixuankuadu = 3272,
            //zsanx_zuxuanbaodan = 3273,
            //zsanx_hezhiweihao = 3274,
            //zsanx_teshuhao = 3275,

            #endregion

            #region 前三
            qsanx_zhixuanfushi = 3739, //直选复式
            qsanx_zhixuandanshi = 3740, //直选单式
            qsanx_qiansanzuhe = 3741, //前三组合
            qsanx_zhixuanhezhi = 3742, //直选和值
            qsanx_zhixuankuadu = 3743, //直选跨度
            qsanx_zusanfushi = 3744, //组三复式
            qsanx_zusandanshi = 3745, //组三单式
            qsanx_zuliufushi = 3746, //组六复式
            qsanx_zuliudanshi = 3747, //组六单式
            qsanx_hunhezuxuan = 3748, //混合组选
            qsanx_zuxuanhezhi = 3749, //组选和值
            qsanx_zuxuanbaodan = 3750, //组选包胆
            qsanx_hezhiweihao = 3751, //和值尾数
            qsanx_teshuhao = 3752, //特殊号
            #endregion

            #region 后二
            her_zhixuanfushi = 3753, //直选复式
            her_zhixuandanshi = 3754, //直选单式
            her_zhixuanhezhi = 3755, //直选和值
            her_zhixuankuadu = 3756, //直选跨度
            her_zuxuanfushi = 3757, //组选复式
            her_zuxuandanshi = 3758, //组选单式
            her_zuxuanhezhi = 3759, //组选和值
            her_zuxuanbaodan = 3760, //组选包胆
            #endregion

            #region 前二
            qer_zhixuanfushi = 3761, //直选复式
            qer_zhixuandanshi = 3762, //直选单式
            qer_zhixuanhezhi = 3763, //直选和值
            qer_zhixuankuadu = 3764, //直选跨度
            qer_zuxuanfushi = 3765, //组选复式
            qer_zuxuandanshi = 3766, //组选单式
            qer_zuxuanhezhi = 3767, //组选和值
            qer_zuxuanbaodan = 3768, //组选包胆
            #endregion

            #region 定位胆
            dingweidan = 3769, //定位胆
            #endregion

            #region 不定位
            hsan_yima = 3770, //后三一码
            qsan_yima = 3771, //前三一码
            hsan_erma = 3772, //后三二码
            qsan_erma = 3773, //前三二码
            sx_yima = 3774, //四星一码
            sx_erma = 3775, //四星二码
            wx_erma = 3776, //五星二码
            wx_sanma = 3777, //五星三码
            #endregion

            #region 大小单双
            qer_daxiaodanshuang = 3778, //前二大小单双
            her_daxiaodanshuang = 3779, //后二大小单双
            qsan_daxiaodanshuang = 3780, //前三大小单双
            hsan_daxiaodanshuang = 3781, //后三大小单双
            #endregion

            #region 任二
            r2_zhixuanfushi = 3782, //直选复式
            r2_zhixuandanshi = 3783, //直选单式
            r2_zhixuanhezhi = 3784, //直选和值
            r2_zuxuanfushi = 3785, //组选复式
            r2_zuxuandanshi = 3786, //组选单式
            r2_zuxuanhezhi = 3787, //组选和值
            #endregion

            #region 任三
            r3_zhixuanfushi = 3788, //直选复式
            r3_zhixuandanshi = 3789, //直选单式
            r3_zhixuanhezhi = 3790, //直选和值
            r3_zusanfushi = 3791, //组三复式
            r3_zusandanshi = 3792, //组三单式
            r3_zuliufushi = 3793, //组六复式
            r3_zuliudanshi = 3794, //组六单式
            r3_hunhezuxuan = 3795, //混合组选
            r3_zuxuanhezhi = 3796, //组选和值
            #endregion

            #region 任四
            r4_zhixuanfushi = 3797, //直选复式
            r4_zhixuandanshi = 3798, //直选单式
            r4_zuxuan24 = 3799, //组选24
            r4_zuxuan12 = 3800, //组选12
            r4_zuxuan6 = 3801, //组选6
            r4_zuxuan4 = 3802, //组选4
            #endregion

            #region 龙虎和
            lhh = 3803, //龙虎和
            #endregion

            #region 总和大小单双
            zhdxds = 3804, //总和大小单双
            #endregion

            #region 后三 特殊号
            tsh = 3805, //特殊号
            #endregion

            #region 中三
            zsanx_housanzuhe = 3806, //中三组合
            zsanx_zhixuankuadu = 3807, //直选跨度
            zsanx_zuxuanbaodan = 3808, //组选包胆
            zsanx_hezhiweihao = 3809, //和值尾数
            zsanx_teshuhao = 3810 //特殊号
            #endregion
        }

        public enum SSC_Italy_PlayTypeRadio
		{
			#region 五星
			wx_zhixuanfushi = 2478,
			wx_zhixuandanshi = 2479,
			wx_wxzuhe = 2480,
			wx_zx120 = 2481,
			wx_zx60 = 2482,
			wx_zx30 = 2483,
			wx_zx20 = 2484,
			wx_zx10 = 2485,
			wx_zx5 = 2486,
			wx_yifanfengshun = 2487,
			wx_haoshichengshuang = 2488,
			wx_sanxingbaoxi = 2489,
			wx_sijifacai = 2490,
			#endregion

			#region 四星
			sx_zhixuanfushi = 2491,
			sx_zhixuandanshi = 2492,
			sx_sixinzuhe = 2493,
			sx_sixinzuxuan = 2494,
			sx_zuxuan12 = 2495,
			sx_zuxuan6 = 2496,
			sx_zuxuan4 = 2497,
			#endregion

			#region 后三
			hsanx_zhixuanfushi = 2498,
			hsanx_zhixuandanshi = 2499,
			hsanx_housanzuhe = 2500,
			hsanx_zhixuanhezhi = 2501,
			hsanx_zhixuankuadu = 2502,
			hsanx_zusanfushi = 2503,
			hsanx_zusandanshi = 2504,
			hsanx_zuliufushi = 2505,
			hsanx_zuliudanshi = 2506,
			hsanx_hunhezuxuan = 2507,
			hsanx_zuxuanhezhi = 2508,
			hsanx_zuxuanbaodan = 2509,
			hsanx_hezhiweihao = 2510,
			#endregion

			#region 特殊号
			tsh = 2511,
			#endregion

			#region 中三
			zsanx_zhixuanfushi = 2512,
			zsanx_zhixuandanshi = 2513,
			zsanx_zhixuanhezhi = 2514,
			zsanx_zusanfushi = 2515,
			zsanx_zusandanshi = 2516,
			zsanx_zuliufushi = 2517,
			zsanx_zuliudanshi = 2518,
			zsanx_hunhezuxuan = 2519,
			zsanx_zuxuanhezhi = 2520,

			zsanx_housanzuhe = 2521,
			zsanx_zhixuankuadu = 2522,
			zsanx_zuxuanbaodan = 2523,
			zsanx_hezhiweihao = 2524,
			zsanx_teshuhao = 2525,

			#endregion

			#region 前三
			qsanx_zhixuanfushi = 2526,
			qsanx_zhixuandanshi = 2527,
			qsanx_housanzuhe = 2528,
			qsanx_zhixuanhezhi = 2529,
			qsanx_zhixuankuadu = 2530,
			qsanx_zusanfushi = 2531,
			qsanx_zusandanshi = 2532,
			qsanx_zuliufushi = 2533,
			qsanx_zuliudanshi = 2534,
			qsanx_hunhezuxuan = 2535,
			qsanx_zuxuanhezhi = 2536,
			qsanx_zuxuanbaodan = 2537,
			qsanx_hezhiweihao = 2538,
			qsanx_teshuhao = 2539,
			#endregion

			#region 后二
			her_zhixuanfushi = 2540,
			her_zhixuandanshi = 2541,
			her_zhixuanhezhi = 2542,
			her_zhixuankuadu = 2543,
			her_zuxuanfushi = 2544,
			her_zuxuandanshi = 2545,
			her_zuxuanhezhi = 2546,
			her_zuxuanbaodan = 2547,
			#endregion

			#region 前二
			qer_zhixuanfushi = 2548,
			qer_zhixuandanshi = 2549,
			qer_zhixuanhezhi = 2550,
			qer_zhixuankuadu = 2551,
			qer_zuxuanfushi = 2552,
			qer_zuxuandanshi = 2553,
			qer_zuxuanhezhi = 2554,
			qer_zuxuanbaodan = 2555,
			#endregion

			#region 定位胆
			dingweidan = 2556,
			#endregion

			#region 不定位
			hsan_yima = 2557,
			qsan_yima = 2558,
			hsan_erma = 2559,
			qsan_erma = 2560,
			sx_yima = 2561,
			sx_erma = 2562,
			wx_erma = 2563,
			wx_sanma = 2564,
			#endregion

			#region 大小单双
			qer_daxiaodanshuang = 2565,
			her_daxiaodanshuang = 2566,
			qsan_daxiaodanshuang = 2567,
			hsan_daxiaodanshuang = 2568,
			#endregion

			#region 总和大小单双
			zhdxds = 2569,
			#endregion

			#region 任二
			r2_zhixuanfushi = 2570,
			r2_zhixuandanshi = 2571,
			r2_zhixuanhezhi = 2572,
			r2_zuxuanfushi = 2573,
			r2_zuxuandanshi = 2574,
			r2_zuxuanhezhi = 2575,
			#endregion

			#region 任三
			r3_zhixuanfushi = 2576,
			r3_zhixuandanshi = 2577,
			r3_zhixuanhezhi = 2578,
			r3_zusanfushi = 2579,
			r3_zusandanshi = 2580,
			r3_zuliufushi = 2581,
			r3_zuliudanshi = 2582,
			r3_hunhezuxuan = 2583,
			r3_zuxuanhezhi = 2584,
			#endregion

			#region 任四
			r4_zhixuanfushi = 2585,
			r4_zhixuandanshi = 2586,
			r4_zuxuan24 = 2587,
			r4_zuxuan12 = 2588,
			r4_zuxuan6 = 2589,
			r4_zuxuan4 = 2590,
			#endregion

			#region 龙虎和
			lhh = 2591,
			#endregion



		}

        public enum SSC_MQQ_PlayTypeRadio
        {
            #region 五星
            wx_zhixuanfushi = 3229,
            wx_zhixuandanshi = 3230,
            wx_wxzuhe = 3231,
            wx_zx120 = 3232,
            wx_zx60 = 3233,
            wx_zx30 = 3234,
            wx_zx20 = 3235,
            wx_zx10 = 3236,
            wx_zx5 = 3237,
            wx_yifanfengshun = 3238,
            wx_haoshichengshuang = 3239,
            wx_sanxingbaoxi = 3240,
            wx_sijifacai = 3241,
            #endregion

            #region 四星
            sx_zhixuanfushi = 3242,
            sx_zhixuandanshi = 3243,
            sx_sixinzuhe = 3244,
            sx_sixinzuxuan = 3245,
            sx_zuxuan12 = 3246,
            sx_zuxuan6 = 3247,
            sx_zuxuan4 = 3248,
            #endregion

            #region 后三
            hsanx_zhixuanfushi = 3249,
            hsanx_zhixuandanshi = 3250,
            hsanx_housanzuhe = 3251,
            hsanx_zhixuanhezhi = 3252,
            hsanx_zhixuankuadu = 3253,
            hsanx_zusanfushi = 3254,
            hsanx_zusandanshi = 3255,
            hsanx_zuliufushi = 3256,
            hsanx_zuliudanshi = 3257,
            hsanx_hunhezuxuan = 3258,
            hsanx_zuxuanhezhi = 3259,
            hsanx_zuxuanbaodan = 3260,
            hsanx_hezhiweihao = 3261,
            #endregion

            #region 中三
            zsanx_zhixuanfushi = 3262,
            zsanx_zhixuandanshi = 3263,
            zsanx_zhixuanhezhi = 3264,
            zsanx_zusanfushi = 3265,
            zsanx_zusandanshi = 3266,
            zsanx_zuliufushi = 3267,
            zsanx_zuliudanshi = 3268,
            zsanx_hunhezuxuan = 3269,
            zsanx_zuxuanhezhi = 3270,

            zsanx_housanzuhe = 3338,
            zsanx_zhixuankuadu = 3339,
            zsanx_zuxuanbaodan = 3340,
            zsanx_hezhiweihao = 3341,
            zsanx_teshuhao = 3342,

            #endregion

            #region 前三
            qsanx_zhixuanfushi = 3271,
            qsanx_zhixuandanshi = 3272,
            qsanx_qiansanzuhe = 3273,
            qsanx_zhixuanhezhi = 3274,
            qsanx_zhixuankuadu = 3275,
            qsanx_zusanfushi = 3276,
            qsanx_zusandanshi = 3277,
            qsanx_zuliufushi = 3278,
            qsanx_zuliudanshi = 3279,
            qsanx_hunhezuxuan = 3280,
            qsanx_zuxuanhezhi = 3281,
            qsanx_zuxuanbaodan = 3282,
            qsanx_hezhiweihao = 3283,
            qsanx_teshuhao = 3284,
            #endregion

            #region 后二
            her_zhixuanfushi = 3285,
            her_zhixuandanshi = 3286,
            her_zhixuanhezhi = 3287,
            her_zhixuankuadu = 3288,
            her_zuxuanfushi = 3289,
            her_zuxuandanshi = 3290,
            her_zuxuanhezhi = 3291,
            her_zuxuanbaodan = 3292,
            #endregion

            #region 前二
            qer_zhixuanfushi = 3293,
            qer_zhixuandanshi = 3294,
            qer_zhixuanhezhi = 3295,
            qer_zhixuankuadu = 3296,
            qer_zuxuanfushi = 3297,
            qer_zuxuandanshi = 3298,
            qer_zuxuanhezhi = 3299,
            qer_zuxuanbaodan = 3300,
            #endregion

            #region 定位胆
            dingweidan = 3301,
            #endregion

            #region 不定位
            hsan_yima = 3302,
            qsan_yima = 3303,
            hsan_erma = 3304,
            qsan_erma = 3305,
            sx_yima = 3306,
            sx_erma = 3307,
            wx_erma = 3308,
            wx_sanma = 3309,
            #endregion

            #region 大小单双
            qer_daxiaodanshuang = 3310,
            her_daxiaodanshuang = 3311,
            qsan_daxiaodanshuang = 3312,
            hsan_daxiaodanshuang = 3313,
            #endregion

            #region 任二
            r2_zhixuanfushi = 3314,
            r2_zhixuandanshi = 3315,
            r2_zhixuanhezhi = 3316,
            r2_zuxuanfushi = 3317,
            r2_zuxuandanshi = 3318,
            r2_zuxuanhezhi = 3319,
            #endregion

            #region 任三
            r3_zhixuanfushi = 3320,
            r3_zhixuandanshi = 3321,
            r3_zhixuanhezhi = 3322,
            r3_zusanfushi = 3323,
            r3_zusandanshi = 3324,
            r3_zuliufushi = 3325,
            r3_zuliudanshi = 3326,
            r3_hunhezuxuan = 3327,
            r3_zuxuanhezhi = 3328,
            #endregion

            #region 任四
            r4_zhixuanfushi = 3329,
            r4_zhixuandanshi = 3330,
            r4_zuxuan24 = 3331,
            r4_zuxuan12 = 3332,
            r4_zuxuan6 = 3333,
            r4_zuxuan4 = 3334,
            #endregion

            #region 龙虎和
            lhh = 3335,
            #endregion

            #region 总和大小单双
            zhdxds = 3336,
            #endregion

            #region 特殊号
            tsh = 3337
            #endregion
        }

        public enum SSC_MQQ5_PlayTypeRadio
        {
            #region 五星
            wx_zhixuanfushi = 3385,
            wx_zhixuandanshi = 3386,
            wx_wxzuhe = 3387,
            wx_zx120 = 3388,
            wx_zx60 = 3389,
            wx_zx30 = 3390,
            wx_zx20 = 3391,
            wx_zx10 = 3392,
            wx_zx5 = 3393,
            wx_yifanfengshun = 3394,
            wx_haoshichengshuang = 3395,
            wx_sanxingbaoxi = 3396,
            wx_sijifacai = 3397,
            #endregion

            #region 四星
            sx_zhixuanfushi = 3398,
            sx_zhixuandanshi = 3399,
            sx_sixinzuhe = 3400,
            sx_sixinzuxuan = 3401,
            sx_zuxuan12 = 3402,
            sx_zuxuan6 = 3403,
            sx_zuxuan4 = 3404,
            #endregion

            #region 后三
            hsanx_zhixuanfushi = 3405,
            hsanx_zhixuandanshi = 3406,
            hsanx_housanzuhe = 3407,
            hsanx_zhixuanhezhi = 3408,
            hsanx_zhixuankuadu = 3409,
            hsanx_zusanfushi = 3410,
            hsanx_zusandanshi = 3411,
            hsanx_zuliufushi = 3412,
            hsanx_zuliudanshi = 3413,
            hsanx_hunhezuxuan = 3414,
            hsanx_zuxuanhezhi = 3415,
            hsanx_zuxuanbaodan = 3416,
            hsanx_hezhiweihao = 3417,
            #endregion

            #region 中三
            zsanx_zhixuanfushi = 3418,
            zsanx_zhixuandanshi = 3419,
            zsanx_zhixuanhezhi = 3420,
            zsanx_zusanfushi = 3421,
            zsanx_zusandanshi = 3422,
            zsanx_zuliufushi = 3423,
            zsanx_zuliudanshi = 3424,
            zsanx_hunhezuxuan = 3425,
            zsanx_zuxuanhezhi = 3426,

            zsanx_housanzuhe = 3494,
            zsanx_zhixuankuadu = 3495,
            zsanx_zuxuanbaodan = 3496,
            zsanx_hezhiweihao = 3497,
            zsanx_teshuhao = 3498,

            #endregion

            #region 前三
            qsanx_zhixuanfushi = 3427,
            qsanx_zhixuandanshi = 3428,
            qsanx_qiansanzuhe = 3429,
            qsanx_zhixuanhezhi = 3430,
            qsanx_zhixuankuadu = 3431,
            qsanx_zusanfushi = 3432,
            qsanx_zusandanshi = 3433,
            qsanx_zuliufushi = 3434,
            qsanx_zuliudanshi = 3435,
            qsanx_hunhezuxuan = 3436,
            qsanx_zuxuanhezhi = 3437,
            qsanx_zuxuanbaodan = 3438,
            qsanx_hezhiweihao = 3439,
            qsanx_teshuhao = 3440,
            #endregion

            #region 后二
            her_zhixuanfushi = 3441,
            her_zhixuandanshi = 3442,
            her_zhixuanhezhi = 3443,
            her_zhixuankuadu = 3444,
            her_zuxuanfushi = 3445,
            her_zuxuandanshi = 3446,
            her_zuxuanhezhi = 3447,
            her_zuxuanbaodan = 3448,
            #endregion

            #region 前二
            qer_zhixuanfushi = 3449,
            qer_zhixuandanshi = 3450,
            qer_zhixuanhezhi = 3451,
            qer_zhixuankuadu = 3452,
            qer_zuxuanfushi = 3453,
            qer_zuxuandanshi = 3454,
            qer_zuxuanhezhi = 3455,
            qer_zuxuanbaodan = 3456,
            #endregion

            #region 定位胆
            dingweidan = 3457,
            #endregion

            #region 不定位
            hsan_yima = 3458,
            qsan_yima = 3459,
            hsan_erma = 3460,
            qsan_erma = 3461,
            sx_yima = 3462,
            sx_erma = 3463,
            wx_erma = 3464,
            wx_sanma = 3465,
            #endregion

            #region 大小单双
            qer_daxiaodanshuang = 3466,
            her_daxiaodanshuang = 3467,
            qsan_daxiaodanshuang = 3468,
            hsan_daxiaodanshuang = 3469,
            #endregion

            #region 任二
            r2_zhixuanfushi = 3470,
            r2_zhixuandanshi = 3471,
            r2_zhixuanhezhi = 3472,
            r2_zuxuanfushi = 3473,
            r2_zuxuandanshi = 3474,
            r2_zuxuanhezhi = 3475,
            #endregion

            #region 任三
            r3_zhixuanfushi = 3476,
            r3_zhixuandanshi = 3477,
            r3_zhixuanhezhi = 3478,
            r3_zusanfushi = 3479,
            r3_zusandanshi = 3480,
            r3_zuliufushi = 3481,
            r3_zuliudanshi = 3482,
            r3_hunhezuxuan = 3483,
            r3_zuxuanhezhi = 3484,
            #endregion

            #region 任四
            r4_zhixuanfushi = 3485,
            r4_zhixuandanshi = 3486,
            r4_zuxuan24 = 3487,
            r4_zuxuan12 = 3488,
            r4_zuxuan6 = 3489,
            r4_zuxuan4 = 3490,
            #endregion

            #region 龙虎和
            lhh = 3491,
            #endregion

            #region 总和大小单双
            zhdxds = 3492,
            #endregion

            #region 特殊号
            tsh = 3493
            #endregion
        }


        public enum SSC_FHQQ_PlayTypeRadio
        {
            #region 五星
            wx_zhixuanfushi = 3541,
            wx_zhixuandanshi = 3542,
            wx_wxzuhe = 3543,
            wx_zx120 = 3544,
            wx_zx60 = 3545,
            wx_zx30 = 3546,
            wx_zx20 = 3547,
            wx_zx10 = 3548,
            wx_zx5 = 3549,
            wx_yifanfengshun = 3550,
            wx_haoshichengshuang = 3551,
            wx_sanxingbaoxi = 3552,
            wx_sijifacai = 3553,
            #endregion

            #region 四星
            sx_zhixuanfushi = 3554,
            sx_zhixuandanshi = 3555,
            sx_sixinzuhe = 3556,
            sx_sixinzuxuan = 3557,
            sx_zuxuan12 = 3558,
            sx_zuxuan6 = 3559,
            sx_zuxuan4 = 3560,
            #endregion

            #region 后三
            hsanx_zhixuanfushi = 3561,
            hsanx_zhixuandanshi = 3562,
            hsanx_housanzuhe = 3563,
            hsanx_zhixuanhezhi = 3564,
            hsanx_zhixuankuadu = 3565,
            hsanx_zusanfushi = 3566,
            hsanx_zusandanshi = 3567,
            hsanx_zuliufushi = 3568,
            hsanx_zuliudanshi = 3569,
            hsanx_hunhezuxuan = 3570,
            hsanx_zuxuanhezhi = 3571,
            hsanx_zuxuanbaodan = 3572,
            hsanx_hezhiweihao = 3573,
            #endregion

            #region 中三
            zsanx_zhixuanfushi = 3574,
            zsanx_zhixuandanshi = 3575,
            zsanx_zhixuanhezhi = 3576,
            zsanx_zusanfushi = 3577,
            zsanx_zusandanshi = 3578,
            zsanx_zuliufushi = 3579,
            zsanx_zuliudanshi = 3580,
            zsanx_hunhezuxuan = 3581,
            zsanx_zuxuanhezhi = 3582,
            zsanx_housanzuhe = 3650,
            zsanx_zhixuankuadu = 3651,
            zsanx_zuxuanbaodan = 3652,
            zsanx_hezhiweihao = 3653,
            zsanx_teshuhao = 3654,
            #endregion

            #region 前三
            qsanx_zhixuanfushi = 3583,
            qsanx_zhixuandanshi = 3584,
            qsanx_qiansanzuhe = 3585,
            qsanx_zhixuanhezhi = 3586,
            qsanx_zhixuankuadu = 3587,
            qsanx_zusanfushi = 3588,
            qsanx_zusandanshi = 3589,
            qsanx_zuliufushi = 3590,
            qsanx_zuliudanshi = 3591,
            qsanx_hunhezuxuan = 3592,
            qsanx_zuxuanhezhi = 3593,
            qsanx_zuxuanbaodan = 3594,
            qsanx_hezhiweihao = 3595,
            qsanx_teshuhao = 3596,
            #endregion

            #region 后二
            her_zhixuanfushi = 3597,
            her_zhixuandanshi = 3598,
            her_zhixuanhezhi = 3599,
            her_zhixuankuadu = 3600,
            her_zuxuanfushi = 3601,
            her_zuxuandanshi = 3602,
            her_zuxuanhezhi = 3603,
            her_zuxuanbaodan = 3604,
            #endregion

            #region 前二
            qer_zhixuanfushi = 3605,
            qer_zhixuandanshi = 3606,
            qer_zhixuanhezhi = 3607,
            qer_zhixuankuadu = 3608,
            qer_zuxuanfushi = 3609,
            qer_zuxuandanshi = 3610,
            qer_zuxuanhezhi = 3611,
            qer_zuxuanbaodan = 3612,
            #endregion

            #region 定位胆
            dingweidan = 3613,
            #endregion

            #region 不定位
            hsan_yima = 3614,
            qsan_yima = 3615,
            hsan_erma = 3616,
            qsan_erma = 3617,
            sx_yima = 3618,
            sx_erma = 3619,
            wx_erma = 3620,
            wx_sanma = 3621,
            #endregion

            #region 大小单双
            qer_daxiaodanshuang = 3622,
            her_daxiaodanshuang = 3623,
            qsan_daxiaodanshuang = 3624,
            hsan_daxiaodanshuang = 3625,
            #endregion

            #region 任二
            r2_zhixuanfushi = 3626,
            r2_zhixuandanshi = 3627,
            r2_zhixuanhezhi = 3628,
            r2_zuxuanfushi = 3629,
            r2_zuxuandanshi = 3630,
            r2_zuxuanhezhi = 3631,
            #endregion

            #region 任三
            r3_zhixuanfushi = 3632,
            r3_zhixuandanshi = 3633,
            r3_zhixuanhezhi = 3634,
            r3_zusanfushi = 3635,
            r3_zusandanshi = 3636,
            r3_zuliufushi = 3637,
            r3_zuliudanshi = 3638,
            r3_hunhezuxuan = 3639,
            r3_zuxuanhezhi = 3640,
            #endregion

            #region 任四
            r4_zhixuanfushi = 3641,
            r4_zhixuandanshi = 3642,
            r4_zuxuan24 = 3643,
            r4_zuxuan12 = 3644,
            r4_zuxuan6 = 3645,
            r4_zuxuan4 = 3646,
            #endregion

            #region 龙虎和
            lhh = 3647,
            #endregion

            #region 总和大小单双
            zhdxds = 3648,
            #endregion

            #region 特殊号
            tsh = 3649,
            #endregion
        }

        public enum Jx11x5_PlayTypeRadio
        {
            #region 三码

            x3_zhixuanfushi = 3845,
            x3_zhixuandanshi = 3846,
            x3_zuxuanfushi = 3847,
            x3_zuxuandanshi = 3848,
            x3_zuxuandantuo = 3849,

            #endregion

            #region 二码

            x2_zhixuanfushi = 3850,
            x2_zhixuandanshi = 3851,
            x2_zuxuanfushi = 3852,
            x2_zuxuandanshi = 3853,
            x2_zuxuandantuo = 3854,

            #endregion

            #region 不定位

            x3_budingwei = 3855,

            #endregion

            #region 定位胆

            x3_dingweidan = 3856,

            #endregion

            #region 趣味

            dingdanshaung = 3857,
            caizhongwei = 3858,

            #endregion

            #region 任选复式

            renfu_1x1 = 3859,
            renfu_2x2 = 3860,
            renfu_3x3 = 3861,
            renfu_4x4 = 3862,
            renfu_5x5 = 3863,
            renfu_6x5 = 3864,
            renfu_7x5 = 3865,
            renfu_8x5 = 3866,

            #endregion

            #region 任选单式

            rendan_1x1 = 3867,
            rendan_2x2 = 3868,
            rendan_3x3 = 3869,
            rendan_4x4 = 3870,
            rendan_5x5 = 3871,
            rendan_6x5 = 3872,
            rendan_7x5 = 3873,
            rendan_8x5 = 3874,

            #endregion

            #region 任选胆拖

            rendt_2x2 = 3875,
            rendt_3x3 = 3876,
            rendt_4x4 = 3877,
            rendt_5x5 = 3878,
            rendt_6x5 = 3879,
            rendt_7x5 = 3880,
            rendt_8x5 = 3881,

            #endregion
        }

        public enum Js11x5_PlayTypeRadio
        {
            #region 三码

            x3_zhixuanfushi = 3916,
            x3_zhixuandanshi = 3917,
            x3_zuxuanfushi = 3918,
            x3_zuxuandanshi = 3919,
            x3_zuxuandantuo = 3920,

            #endregion

            #region 二码

            x2_zhixuanfushi = 3921,
            x2_zhixuandanshi = 3922,
            x2_zuxuanfushi = 3923,
            x2_zuxuandanshi = 3924,
            x2_zuxuandantuo = 3925,

            #endregion

            #region 不定位

            x3_budingwei = 3926,

            #endregion

            #region 定位胆

            x3_dingweidan = 3927,

            #endregion

            #region 趣味

            dingdanshaung = 3928,
            caizhongwei = 3929,

            #endregion

            #region 任选复式

            renfu_1x1 = 3930,
            renfu_2x2 = 3931,
            renfu_3x3 = 3932,
            renfu_4x4 = 3933,
            renfu_5x5 = 3934,
            renfu_6x5 = 3935,
            renfu_7x5 = 3936,
            renfu_8x5 = 3937,

            #endregion

            #region 任选单式

            rendan_1x1 = 3938,
            rendan_2x2 = 3939,
            rendan_3x3 = 3940,
            rendan_4x4 = 3941,
            rendan_5x5 = 3942,
            rendan_6x5 = 3943,
            rendan_7x5 = 3944,
            rendan_8x5 = 3945,

            #endregion

            #region 任选胆拖

            rendt_2x2 = 3946,
            rendt_3x3 = 3947,
            rendt_4x4 = 3948,
            rendt_5x5 = 3949,
            rendt_6x5 = 3950,
            rendt_7x5 = 3951,
            rendt_8x5 = 3952,

            #endregion
        }

        //usertype = 2的playtyperadioid
        public enum SSC_ChichuQQ_PlayTypeRadio
        {
            #region 五星
            wx_zhixuanfushi = 4369, //直选复式
            wx_zhixuandanshi = 4370, //直选单式
            wx_wxzuhe = 4371, //五星组合
            wx_zx120 = 4372, //组选120
            wx_zx60 = 4373, //组选60
            wx_zx30 = 4374, //组选30
            wx_zx20 = 4375, //组选20
            wx_zx10 = 4376, //组选10
            wx_zx5 = 4377, //组选5
            wx_yifanfengshun = 4378, //一帆风顺
            wx_haoshichengshuang = 4379, //好事成双
            wx_sanxingbaoxi = 4380, //三星报喜
            wx_sijifacai = 4381, //四季发财
            #endregion

            #region 四星
            sx_zhixuanfushi = 4382, //直选复式
            sx_zhixuandanshi = 4383, //直选单式
            sx_sixinzuhe = 4384, //四星组合
            sx_sixinzuxuan = 4385, //组选24
            sx_zuxuan12 = 4386, //组选12
            sx_zuxuan6 = 4387, //组选6
            sx_zuxuan4 = 4388, //组选4
            #endregion

            #region 后三
            hsanx_zhixuanfushi = 4389, //直选复式
            hsanx_zhixuandanshi = 4390, //直选单式
            hsanx_housanzuhe = 4391, //后三组合
            hsanx_zhixuanhezhi = 4392, //直选和值
            hsanx_zhixuankuadu = 4393, //直选跨度
            hsanx_zusanfushi = 4394, //组三复式
            hsanx_zusandanshi = 4395, //组三单式
            hsanx_zuliufushi = 4396, //组六复式
            hsanx_zuliudanshi = 4397, //组六单式
            hsanx_hunhezuxuan = 4398, //混合组选
            hsanx_zuxuanhezhi = 4399, //组选和值
            hsanx_zuxuanbaodan = 4400, //组选包胆
            hsanx_hezhiweihao = 4401, //和值尾数
            #endregion

            #region 中三
            zsanx_zhixuanfushi = 4402, //直选复式
            zsanx_zhixuandanshi = 4403, //直选单式
            zsanx_zhixuanhezhi = 4404, //直选和值
            zsanx_zusanfushi = 4405, //组三复式
            zsanx_zusandanshi = 4406, //组三单式
            zsanx_zuliufushi = 4407, //组六复式
            zsanx_zuliudanshi = 4408, //组六单式
            zsanx_hunhezuxuan = 4409, //混合组选
            zsanx_zuxuanhezhi = 4410, //组选和值
            #endregion

            #region 前三
            qsanx_zhixuanfushi = 4411, //直选复式
            qsanx_zhixuandanshi = 4412, //直选单式
            qsanx_qiansanzuhe = 4413, //前三组合
            qsanx_zhixuanhezhi = 4414, //直选和值
            qsanx_zhixuankuadu = 4415, //直选跨度
            qsanx_zusanfushi = 4416, //组三复式
            qsanx_zusandanshi = 4417, //组三单式
            qsanx_zuliufushi = 4418, //组六复式
            qsanx_zuliudanshi = 4419, //组六单式
            qsanx_hunhezuxuan = 4420, //混合组选
            qsanx_zuxuanhezhi = 4421, //组选和值
            qsanx_zuxuanbaodan = 4422, //组选包胆
            qsanx_hezhiweihao = 4423, //和值尾数
            qsanx_teshuhao = 4424, //特殊号
            #endregion

            #region 后二
            her_zhixuanfushi = 4425, //直选复式
            her_zhixuandanshi = 4426, //直选单式
            her_zhixuanhezhi = 4427, //直选和值
            her_zhixuankuadu = 4428, //直选跨度
            her_zuxuanfushi = 4429, //组选复式
            her_zuxuandanshi = 4430, //组选单式
            her_zuxuanhezhi = 4431, //组选和值
            her_zuxuanbaodan = 4432, //组选包胆
            #endregion

            #region 前二
            qer_zhixuanfushi = 4433, //直选复式
            qer_zhixuandanshi = 4434, //直选单式
            qer_zhixuanhezhi = 4435, //直选和值
            qer_zhixuankuadu = 4436, //直选跨度
            qer_zuxuanfushi = 4437, //组选复式
            qer_zuxuandanshi = 4438, //组选单式
            qer_zuxuanhezhi = 4439, //组选和值
            qer_zuxuanbaodan = 4440, //组选包胆
            #endregion

            #region 定位胆
            dingweidan = 4441, //定位胆
            #endregion

            #region 不定位
            hsan_yima = 4442, //后三一码
            qsan_yima = 4443, //前三一码
            hsan_erma = 4444, //后三二码
            qsan_erma = 4445, //前三二码
            sx_yima = 4446, //四星一码
            sx_erma = 4447, //四星二码
            wx_erma = 4448, //五星二码
            wx_sanma = 4449, //五星三码
            #endregion

            #region 大小单双
            qer_daxiaodanshuang = 4450, //前二大小单双
            her_daxiaodanshuang = 4451, //后二大小单双
            qsan_daxiaodanshuang = 4452, //前三大小单双
            hsan_daxiaodanshuang = 4453, //后三大小单双
            #endregion

            #region 任二
            r2_zhixuanfushi = 4454, //直选复式
            r2_zhixuandanshi = 4455, //直选单式
            r2_zhixuanhezhi = 4456, //直选和值
            r2_zuxuanfushi = 4457, //组选复式
            r2_zuxuandanshi = 4458, //组选单式
            r2_zuxuanhezhi = 4459, //组选和值
            #endregion

            #region 任三
            r3_zhixuanfushi = 4460, //直选复式
            r3_zhixuandanshi = 4461, //直选单式
            r3_zhixuanhezhi = 4462, //直选和值
            r3_zusanfushi = 4463, //组三复式
            r3_zusandanshi = 4464, //组三单式
            r3_zuliufushi = 4465, //组六复式
            r3_zuliudanshi = 4466, //组六单式
            r3_hunhezuxuan = 4467, //混合组选
            r3_zuxuanhezhi = 4468, //组选和值
            #endregion

            #region 任四
            r4_zhixuanfushi = 4469, //直选复式
            r4_zhixuandanshi = 4470, //直选单式
            r4_zuxuan24 = 4471, //组选24
            r4_zuxuan12 = 4472, //组选12
            r4_zuxuan6 = 4473, //组选6
            r4_zuxuan4 = 4474, //组选4
            #endregion

            #region 龙虎和
            lhh = 4475, //龙虎和
            #endregion

            #region 总和大小单双
            zhdxds = 4476, //总和大小单双
            #endregion

            #region 后三 特殊号
            tsh = 4477, //特殊号
            #endregion

            #region 中三
            zsanx_housanzuhe = 4478, //中三组合
            zsanx_zhixuankuadu = 4479, //直选跨度
            zsanx_zuxuanbaodan = 4480, //组选包胆
            zsanx_hezhiweihao = 4481, //和值尾数
            zsanx_teshuhao = 4482 //特殊号
            #endregion
        }

        #region  *******OLD*****
        #region PlAYTYPE ENUM
        public enum SSC_CQ_PlayType
		{
			CQ_3xZhix = 1,//三星直选
			CQ_3xZux = 2,//三星组选
			CQ_2xZhix = 3,//二星直选
			CQ_2xZux = 4,//二星组选
			CQ_Bdw = 6,//不定位胆
			CQ_Dw = 9,//定位胆
			CQ_Rx = 41,//任选
			CQ_ZXHH = 52,//组选混合单式
			CQ_WX = 56,//五星
			CQ_SX = 57,//四星
			CQ_DXDS = 114//大小单双
		}
		public enum SSC_QQ_PlayType
		{
			QQ_3xZhix = 848,//三星直选
			QQ_3xZux = 849,//三星组选
			QQ_2xZhix = 850,//二星直选
			QQ_2xZux = 851,//二星组选
			QQ_Bdw = 852,//不定位胆
			QQ_Dw = 853,//定位胆
			QQ_Rx = 854,//任选
			QQ_ZXHH = 855,//组选混合单式
			QQ_WX = 856,//五星
			QQ_SX = 857,//四星
			QQ_DXDS = 858//大小单双
		}
        public enum SSC_QQ5_PlayType
        {
            QQ5_3xZhix = 978,//三星直选
            QQ5_3xZux = 979,//三星组选
            QQ5_2xZhix = 980,//二星直选
            QQ5_2xZux = 981,//二星组选
            QQ5_Bdw = 982,//不定位胆
            QQ5_Dw = 983,//定位胆
            QQ5_Rx = 984,//任选
            QQ5_ZXHH = 985,//组选混合单式
            QQ5_WX = 986,//五星
            QQ5_SX = 987,//四星
            QQ5_DXDS = 988,//大小单双
        }

        public enum SSC_Italy_PlayType
		{
			Italy_3xZhix = 895,//三星直选
			Italy_3xZux = 896,//三星组选
			Italy_2xZhix = 897,//二星直选
			Italy_2xZux = 898,//二星组选
			Italy_Bdw = 899,//不定位胆
			Italy_Dw = 900,//定位胆
			Italy_Rx = 901,//任选
			Italy_ZXHH = 902,//组选混合单式
			Italy_WX = 903,//五星
			Italy_SX = 904,//四星
			Italy_DXDS = 905//大小单双
		}
		public enum SSC_Sinkiang_PlayType
		{
			Sinkiang_3xZhix = 60,//三星直选
			Sinkiang_3xZux = 61,//三星组选
			Sinkiang_2xZhix = 62,//二星直选
			Sinkiang_2xZux = 63,//二星组选
			Sinkiang_Bdw = 64,//不定位胆
			Sinkiang_Dw = 65,//定位胆
			Sinkiang_Rx = 68,//任选
			Sinkiang_ZXHH = 69,//组选混合单式
			Sinkiang_WX = 66,//五星
			Sinkiang_SX = 67,//四星
			Sinkiang_DXDS = 116//大小单双
		}
		public enum SSC_JX_PlayType
		{
			JX_3xZhix = 10,//三星直选
			JX_3xZux = 11,//三星组选
			JX_2xZhix = 12,//二星直选
			JX_2xZux = 13,//二星组选
			JX_Bdw = 17,//不定位胆
			JX_Dw = 18,//定位胆
			JX_Rx = 42,//任选
			JX_ZXHH = 53,//组选混合单式
			JX_WX = 58,//五星
			JX_SX = 59,//四星
			JX_DXDS = 115,//大小单双
		}

		public enum SSC_FC3D_PlayType
		{
			FC3D_3xZhix = 19,//三星直选
			FC3D_3xZux = 20,//三星组选
			FC3D_2xZhix = 21,//二星直选
			FC3D_Bdw = 22,//不定位胆
			FC3D_Dw = 25,//定位胆
			FC3D_ZXHH = 54//组选混合单式
		}

		public enum SSC_TC3_PlayType
		{
			TC3_3xZhix = 26,//三星直选
			TC3_3xZux = 27,//三星组选
			TC3_2xZhix = 28,//二星直选
			TC3_Bdw = 29,//不定位胆
			TC3_Dw = 30,//定位胆
			TC3_ZXHH = 55//组选混合单式
		}
		public enum SSC_Ele_Five_PlayType
		{
			SSC_Ele_Five_3xZhix = 43,//三星直选
			SSC_Ele_Five_3xZux = 44,//三星组选
			SSC_Ele_Five_2xZhix = 45,//二星直选
			SSC_Ele_Five_2xZux = 46,//二星组选
			SSC_Ele_Bdw = 47,//不定位胆
			SSC_Ele_Dw = 48,//定位胆
			SSC_Ele_RxFs = 50,//任选复式
			SSC_Ele_RxDt = 51,//任选胆拖
			SSC_Ele_RxDs = 196//任选单式
		}
		public enum CQ_Ele_Five_PlayType
		{
			CQ_Ele_Five_3xZhix = 70,//三星直选
			CQ_Ele_Five_3xZux = 71,//三星组选
			CQ_Ele_Five_2xZhix = 72,//二星直选
			CQ_Ele_Five_2xZux = 73,//二星组选
			CQ_Ele_Bdw = 74,//不定位胆
			CQ_Ele_Dw = 75,//定位胆
			CQ_Ele_RxFs = 76,//任选复式
			CQ_Ele_RxDt = 77//任选胆拖
		}

		public enum SD_Ele_Five_PlayType
		{
			SD_Ele_Five_3xZhix = 88,//三星直选
			SD_Ele_Five_3xZux = 89,//三星组选
			SD_Ele_Five_2xZhix = 90,//二星直选
			SD_Ele_Five_2xZux = 91,//二星组选
			SD_Ele_Bdw = 92,//不定位胆
			SD_Ele_Dw = 93,//定位胆
			SD_Ele_RxFs = 94,//任选复式
			SD_Ele_RxDt = 95,//任选胆拖
			SD_Ele_RxDs = 197,//任选单式
		}

		public enum SSC_TJ_PlayType
		{
			TJ_3xZhix = 80,//三星直选
			TJ_3xZux = 81,//三星组选
			TJ_2xZhix = 82,//二星直选
			TJ_2xZux = 83,//二星组选
			TJ_Bdw = 84,//不定位胆
			TJ_Dw = 85,//定位胆
			TJ_Rx = 86,//任选
			TJ_ZXHH = 87,//组选混合单式
			TJ_WX = 78,//五星
			TJ_SX = 79,//四星
			TJ_DXDS = 117//大小单双
		}

		public enum BJ_PK_PlayType
		{
			BJ_PK_Q1 = 96,//前一
			BJ_PK_Q2 = 97,//前二
			BJ_PK_Q3 = 98,//前三
			BJ_PK_Dw = 99,//定位胆
			BJ_PK_Q4 = 108,//前四
			BJ_PK_Q5 = 109,//前五
			BJ_PK_LH = 110,//龙虎
			BJ_PK_BS = 111,//大小
			BJ_PK_SD = 112,//单双
			BJ_PK_SV = 113,//和值
			BJ_PK_Q6 = 193,//前六
		}

		public enum De_PK_PlayType
		{
			De_PK_Q1 = 414,//前一
			De_PK_Q2 = 415,//前二
			De_PK_Q3 = 416,//前三
			De_PK_Dw = 417,//定位胆
			De_PK_Q4 = 418,//前四
			De_PK_Q5 = 419,//前五
			De_PK_LH = 420,//龙虎
			De_PK_BS = 421,//大小
			De_PK_SD = 422,//单双
			De_PK_SV = 423,//和值
			De_PK_Q6 = 424,//前六
		}

        public enum LKA_PK_PlayType
        {
            PK_Q1 = 956,//前一
            PK_Q2 = 957,//前二
            PK_Q3 = 958,//前三
            PK_Dw = 959,//定位胆
            PK_Q4 = 960,//前四
            PK_Q5 = 961,//前五
            PK_LH = 962,//龙虎
            PK_BS = 963,//大小
            PK_SD = 964,//单双
            PK_SV = 965,//和值
            PK_Q6 = 966,//前六
        }

        public enum VNS_PK_PlayType
        {
            VNS_PK_Q1 = 1003,//前一
            VNS_PK_Q2 = 1004,//前二
            VNS_PK_Q3 = 1005,//前三
            VNS_PK_Dw = 1006,//定位胆
            VNS_PK_Q4 = 1007,//前四
            VNS_PK_Q5 = 1008,//前五
            VNS_PK_LH = 1009,//龙虎
            VNS_PK_BS = 1010,//大小
            VNS_PK_SD = 1011,//单双
            VNS_PK_SV = 1012,//和值
            VNS_PK_Q6 = 1013 //前六
        }

        public enum QQRC_PK_PlayType
        {
            QQRC_PK_Q1 = 1025,//前一
            QQRC_PK_Q2 = 1026,//前二
            QQRC_PK_Q3 = 1027,//前三
            QQRC_PK_Dw = 1028,//定位胆
            QQRC_PK_Q4 = 1029,//前四
            QQRC_PK_Q5 = 1030,//前五
            QQRC_PK_LH = 1031,//龙虎
            QQRC_PK_BS = 1032,//大小
            QQRC_PK_SD = 1033,//单双
            QQRC_PK_SV = 1034,//和值
            QQRC_PK_Q6 = 1035 //前六
        }

        public enum QQRC5_PK_PlayType
        {
            QQRC5_PK_Q1 = 1047,//前一
            QQRC5_PK_Q2 = 1048,//前二
            QQRC5_PK_Q3 = 1049,//前三
            QQRC5_PK_Dw = 1050,//定位胆
            QQRC5_PK_Q4 = 1051,//前四
            QQRC5_PK_Q5 = 1052,//前五
            QQRC5_PK_LH = 1053,//龙虎
            QQRC5_PK_BS = 1054,//大小
            QQRC5_PK_SD = 1055,//单双
            QQRC5_PK_SV = 1056,//和值
            QQRC5_PK_Q6 = 1057 //前六
        }

        public enum Italy_PK_PlayType
		{
			Italy_PK_Q1 = 873,//前一
			Italy_PK_Q2 = 874,//前二
			Italy_PK_Q3 = 875,//前三
			Italy_PK_Dw = 876,//定位胆
			Italy_PK_Q4 = 877,//前四
			Italy_PK_Q5 = 878,//前五
			Italy_PK_LH = 879,//龙虎
			Italy_PK_BS = 880,//大小
			Italy_PK_SD = 881,//单双
			Italy_PK_SV = 882,//和值
			Italy_PK_Q6 = 883,//前六
		}

		public enum HJ_Ele_Five_PlayType
		{
			HJ_Ele_Five_3xZhix = 100,//三星直选
			HJ_Ele_Five_3xZux = 101,//三星组选
			HJ_Ele_Five_2xZhix = 102,//二星直选
			HJ_Ele_Five_2xZux = 103,//二星组选
			HJ_Ele_Bdw = 104,//不定位胆
			HJ_Ele_Dw = 105,//定位胆
			HJ_Ele_RxFs = 106,//任选复式
			HJ_Ele_RxDt = 107,//任选胆拖
			HJ_Ele_RxDs = 198//任选复式
		}

		public enum HJ_SFC_PlayType
		{
			HJ_SFC_3xZhix = 118,//三星直选
			HJ_SFC_3xZux = 119,//三星组选
			HJ_SFC_2xZhix = 120,//二星直选
			HJ_SFC_2xZux = 121,//二星组选
			HJ_SFC_Bdw = 122,//不定位胆
			HJ_SFC_Dw = 123,//定位胆
			HJ_SFC_Rx = 124,//任选
			HJ_SFC_ZXHH = 125,//组选混合单式
			HJ_SFC_WX = 126,//五星
			HJ_SFC_SX = 127,//四星
			HJ_SFC_DXDS = 128//大小单双
		}

		public enum HJ_PK_PlayType
		{

			HJ_PK_Q1 = 129,//前一
			HJ_PK_Q2 = 130,//前二
			HJ_PK_Q3 = 131,//前三
			HJ_PK_Dw = 132,//定位胆
			HJ_PK_Q4 = 133,//前四
			HJ_PK_Q5 = 134,//前五
			HJ_PK_LH = 135,//龙虎
			HJ_PK_BS = 136,//大小
			HJ_PK_SD = 137,//单双
			HJ_PK_SV = 138,//和值
			HJ_PK_Q6 = 194,//前六
		}

		public enum JSKS_PlayType
		{
			JSKS_3LTX = 139,
			JSKS_3BTH = 140,
			JSKS_3THDX = 141,
			JSKS_3THTX = 142,
			JSKS_2THDX = 143,
			JSKS_2THFX = 144,
			JSKS_2BTX = 145,
			JSKS_HZ = 146,
			JSKS_DX = 147,
			JSKS_DS = 148,
			JSKS_C1G = 149
		}
        /// <summary>
        /// 法國快三玩法
        /// </summary>
        public enum FRKS_PlayType
        {
            /// <summary>
            /// 三连号通选
            /// </summary>
            FRKS_3LTX = 945,
            /// <summary>
            /// 三不同号
            /// </summary>
            FRKS_3BTH = 946,
            /// <summary>
            /// 三同号单选
            /// </summary>
            FRKS_3THDX = 947,
            /// <summary>
            /// 三同号通选
            /// </summary>
            FRKS_3THTX = 948,
            /// <summary>
            /// 二同号单选
            /// </summary>
            FRKS_2THDX = 949,
            /// <summary>
            /// 二同号复选
            /// </summary>
            FRKS_2THFX = 950,
            /// <summary>
            /// 二不同号
            /// </summary>
            FRKS_2BTX = 951,
            /// <summary>
            /// 和值
            /// </summary>
            FRKS_HZ = 952,
            /// <summary>
            /// 大小
            /// </summary>
            FRKS_DX = 953,
            /// <summary>
            /// 单双
            /// </summary>
            FRKS_DS = 954,
            /// <summary>
            /// 猜一个号
            /// </summary>
            FRKS_C1G = 955
        }

        public enum SSC_HSSEC_PlayType
		{
			HSSEC_3xZhix = 152,//三星直选
			HSSEC_3xZux = 153,//三星组选
			HSSEC_2xZhix = 154,//二星直选
			HSSEC_2xZux = 155,//二星组选
			HSSEC_Bdw = 156,//不定位胆
			HSSEC_Dw = 157,//定位胆
			HSSEC_Rx = 158,//任选
			HSSEC_ZXHH = 159,//组选混合单式
			HSSEC_WX = 150,//五星
			HSSEC_SX = 151,//四星
			HSSEC_DXDS = 160//大小单双
		}

		public enum HJ_PKHSSEC_PlayType
		{

			HJ_PKHSSEC_Q1 = 161,//前一
			HJ_PKHSSEC_Q2 = 162,//前二
			HJ_PKHSSEC_Q3 = 163,//前三
			HJ_PKHSSEC_Dw = 164,//定位胆
			HJ_PKHSSEC_Q4 = 165,//前四
			HJ_PKHSSEC_Q5 = 166,//前五
			HJ_PKHSSEC_LH = 167,//龙虎
			HJ_PKHSSEC_BS = 168,//大小
			HJ_PKHSSEC_SD = 169,//单双
			HJ_PKHSSEC_SV = 170,//和值
			HJ_PKHSSEC_Q6 = 195,//前六
		}

		public enum SSC_TAIJIN_PlayType
		{
			TAIJIN_3xZhix = 173,//三星直选
			TAIJIN_3xZux = 174,//三星组选
			TAIJIN_2xZhix = 175,//二星直选
			TAIJIN_2xZux = 176,//二星组选
			TAIJIN_Bdw = 177,//不定位胆
			TAIJIN_Dw = 178,//定位胆
			TAIJIN_Rx = 179,//任选
			TAIJIN_ZXHH = 180,//组选混合单式
			TAIJIN_WX = 171,//五星
			TAIJIN_SX = 172,//四星
			TAIJIN_DXDS = 181//大小单双
		}

		public enum SSC_TWBINGGO_PlayType
		{
			TWBINGGO_3xZhix = 184,//三星直选
			TWBINGGO_3xZux = 185,//三星组选
			TWBINGGO_2xZhix = 186,//二星直选
			TWBINGGO_2xZux = 187,//二星组选
			TWBINGGO_Bdw = 188,//不定位胆
			TWBINGGO_Dw = 189,//定位胆
			TWBINGGO_Rx = 190,//任选
			TWBINGGO_ZXHH = 191,//组选混合单式
			TWBINGGO_WX = 182,//五星
			TWBINGGO_SX = 183,//四星
			TWBINGGO_DXDS = 192//大小单双
		}

		public enum SSC_BJ8_PlayType
		{
			BJ8_3xZhix = 201,//三星直选
			BJ8_3xZux = 202,//三星组选
			BJ8_2xZhix = 203,//二星直选
			BJ8_2xZux = 204,//二星组选
			BJ8_Bdw = 205,//不定位胆
			BJ8_Dw = 206,//定位胆
			BJ8_Rx = 207,//任选
			BJ8_ZXHH = 208,//组选混合单式
			BJ8_WX = 199,//五星
			BJ8_SX = 200,//四星
			BJ8_DXDS = 209//大小单双
		}

		public enum SSC_KOR5_PlayType
		{
			KOR5_3xZhix = 212,//三星直选
			KOR5_3xZux = 213,//三星组选
			KOR5_2xZhix = 214,//二星直选
			KOR5_2xZux = 215,//二星组选
			KOR5_Bdw = 216,//不定位胆
			KOR5_Dw = 217,//定位胆
			KOR5_Rx = 218,//任选
			KOR5_ZXHH = 219,//组选混合单式
			KOR5_WX = 210,//五星
			KOR5_SX = 211,//四星
			KOR5_DXDS = 220//大小单双
		}

        public enum SSC_MQQ_PlayType
        {
            MQQ_3xZhix = 1069, //三星直选
            MQQ_3xZux = 1070, //三星组选
            MQQ_2xZhix = 1071, //二星直选
            MQQ_2xZux = 1072, //二星组选
            MQQ_Bdw = 1073, //不定位胆
            MQQ_Dw = 1074, //定位胆
            MQQ_Rx = 1075, //任选
            MQQ_ZXHH = 1076, //组选混合单式
            MQQ_WX = 1077, //五星
            MQQ_SX = 1078, //四星
            MQQ_DXDS = 1079, //大小单双
        }
        public enum SSC_MQQ5_PlayType
        {
            MQQ5_3xZhix = 1094, //三星直选
            MQQ5_3xZux = 1095, //三星组选
            MQQ5_2xZhix = 1096, //二星直选
            MQQ5_2xZux = 1097, //二星组选
            MQQ5_Bdw = 1098, //不定位胆
            MQQ5_Dw = 1099, //定位胆
            MQQ5_Rx = 1100, //任选
            MQQ5_ZXHH = 1101, //组选混合单式
            MQQ5_WX = 1102, //五星
            MQQ5_SX = 1103, //四星
            MQQ5_DXDS = 1104
        }

        public enum SSC_FHQQ_PlayType
        {
            FHQQ_3xZhix = 1119, //三星直选
            FHQQ_3xZux = 1120, //三星组选
            FHQQ_2xZhix = 1121, //二星直选
            FHQQ_2xZux = 1122, //二星组选
            FHQQ_Bdw = 1123, //不定位胆
            FHQQ_Dw = 1124, //定位胆
            FHQQ_Rx = 1125, //任选
            FHQQ_ZXHH = 1126, //组选混合单式
            FHQQ_WX = 1127, //五星
            FHQQ_SX = 1128, //四星
            FHQQ_DXDS = 1129 //大小单双
        }
        public enum SSC_WeixinQQ_PlayType
        {
            WeixinQQ_3xZhix = 1144,//三星直选
            WeixinQQ_3xZux = 1145,//三星组选
            WeixinQQ_2xZhix = 1146,//二星直选
            WeixinQQ_2xZux = 1147,//二星组选
            WeixinQQ_Bdw = 1148,//不定位胆
            WeixinQQ_Dw = 1149,//定位胆
            WeixinQQ_Rx = 1150,//任选
            WeixinQQ_ZXHH = 1151,//组选混合单式
            WeixinQQ_WX = 1152,//五星
            WeixinQQ_SX = 1153,//四星
            WeixinQQ_DXDS = 1154//大小单双
        }

        public enum SSC_ChichuQQ_PlayType
        {
            ChichuQQ_3xZhix = 1279,//三星直选
            ChichuQQ_3xZux = 1280,//三星组选
            ChichuQQ_2xZhix = 1281,//二星直选
            ChichuQQ_2xZux = 1282,//二星组选
            ChichuQQ_Bdw = 1283,//不定位胆
            ChichuQQ_Dw = 1284,//定位胆
            ChichuQQ_Rx = 1285,//任选
            ChichuQQ_ZXHH = 1286,//组选混合单式
            ChichuQQ_WX = 1287,//五星
            ChichuQQ_SX = 1288,//四星
            ChichuQQ_DXDS = 1289//大小单双
        }

        public enum Jx11x5_PlayType
        {
            Jx11x5_3xZhix = 1169,//三星直选
            Jx11x5_3xZux = 1170,//三星组选
            Jx11x5_2xZhix = 1171,//二星直选
            Jx11x5_2xZux = 1172,//二星组选
            Jx11x5_Bdw = 1173,//不定位胆
            Jx11x5_Dw = 1174,//定位胆
            Jx11x5_RxFs = 1175,//任选复式
            Jx11x5_RxDt = 1176,//任选胆拖
            Jx11x5_RxDs = 1177//任选单式
        }

        public enum Js11x5_PlayType
        {
            Js11x5_3xZhix = 1186,//三星直选
            Js11x5_3xZux = 1187,//三星组选
            Js11x5_2xZhix = 1188,//二星直选
            Js11x5_2xZux = 1189,//二星组选
            Js11x5_Bdw = 1190,//不定位胆
            Js11x5_Dw = 1191,//定位胆
            Js11x5_RxFs = 1192,//任选复式
            Js11x5_RxDt = 1193,//任选胆拖
            Js11x5_RxDs = 1194//任选单式
        }

        public enum SSC_Viet_PlayType
        {
            VietSSC_3xZhix = 1203, //三星直选
            VietSSC_3xZux = 1204, //三星组选
            VietSSC_2xZhix = 1205, //二星直选
            VietSSC_2xZux = 1206, //二星组选
            VietSSC_Bdw = 1207, //不定位胆
            VietSSC_Dw = 1208, //定位胆
            VietSSC_Rx = 1209, //任选
            VietSSC_ZXHH = 1210, //组选混合单式
            VietSSC_WX = 1211, //五星
            VietSSC_SX = 1212, //四星
            VietSSC_DXDS = 1213 //大小单双
        }

        #endregion


        public enum PlayType
		{
			CQ_3xZhix = 1,//三星直选
			CQ_3xZux = 2,//三星组选
			CQ_2xZhix = 3,//二星直选
			CQ_2xZux = 4,//二星组选
			CQ_Bdw = 6,//不定位胆
			CQ_Dw = 9,//定位胆
			CQ_Rx = 41,//任选
			JX_3xZhix = 10,//三星直选
			JX_3xZux = 11,//三星组选
			JX_2xZhix = 12,//二星直选
			JX_2xZux = 13,//二星组选
			JX_Bdw = 17,//不定位胆
			JX_Dw = 18,//定位胆
			JX_Rx = 42,//任选
			FC3D_3xZhix = 19,//三星直选
			FC3D_3xZux = 20,//三星组选
			FC3D_2xZhix = 21,//二星直选
			FC3D_Bdw = 22,//不定位胆
			FC3D_Dw = 25,//定位胆
			TC3_3xZhix = 26,//三星直选
			TC3_3xZux = 27,//三星组选
			TC3_2xZhix = 28,//二星直选
			TC3_Bdw = 29,//不定位胆
			TC3_Dw = 30,//定位胆
			QQ_3xZhix = 848,//三星直选
			QQ_3xZux = 849,//三星组选
			QQ_2xZhix = 850,//二星直选
			QQ_2xZux = 851,//二星组选
			QQ_Bdw = 852,//不定位胆
			QQ_Dw = 853,//定位胆
			QQ_Rx = 854,//任选

			Italy_3xZhix = 895,//三星直选
			Italy_3xZux = 896,//三星组选
			Italy_2xZhix = 897,//二星直选
			Italy_2xZux = 898,//二星组选
			Italy_Bdw = 899,//不定位胆
			Italy_Dw = 900,//定位胆
			Italy_Rx = 901,//任选

			SSC_Ele_Five_3xZhix = 43,//三星直选
			SSC_Ele_Five_3xZux = 44,//三星组选
			SSC_Ele_Five_2xZhix = 45,//二星直选
			SSC_Ele_Five_2xZux = 46,//二星组选
			SSC_Ele_Bdw = 47,//不定位胆
			SSC_Ele_Dw = 48,//定位胆
			SSC_Ele_RxFs = 50,//任选复式
			SSC_Ele_RxDt = 51,//任选胆拖
			SSC_Ele_RxDs = 196,//任选单式

			CQ_ZXHH = 52,//组选混合单式
			JX_ZXHH = 53,//组选混合单式
			FC3D_ZXHH = 54,//组选混合单式
			TC3_ZXHH = 55,//组选混合单式
			QQ_ZXHH = 855,//组选混合单式
			Italy_ZXHH = 902,//组选混合单式
			CQ_WX = 56,//五星
			CQ_SX = 57,//四星
			JX_WX = 58,//五星
			JX_SX = 59,//四星
			QQ_WX = 856,//五星
			QQ_SX = 857,//四星
			Italy_WX = 903,//五星
			Italy_SX = 904,//四星

			Sinkiang_3xZhix = 60,//三星直选
			Sinkiang_3xZux = 61,//三星组选
			Sinkiang_2xZhix = 62,//二星直选
			Sinkiang_2xZux = 63,//二星组选

			Sinkiang_Bdw = 64,//不定位胆
			Sinkiang_Dw = 65,//定位胆
			Sinkiang_Rx = 68,//任选
			Sinkiang_ZXHH = 69,//组选混合单式
			Sinkiang_WX = 66,//五星
			Sinkiang_SX = 67,//四星

			CQ_Ele_Five_3xZhix = 70,//三星直选
			CQ_Ele_Five_3xZux = 71,//三星组选
			CQ_Ele_Five_2xZhix = 72,//二星直选
			CQ_Ele_Five_2xZux = 73,//二星组选
			CQ_Ele_Bdw = 74,//不定位胆
			CQ_Ele_Dw = 75,//定位胆
			CQ_Ele_RxFs = 76,//任选复式
			CQ_Ele_RxDt = 77,//任选胆拖

			TJ_3xZhix = 80,//三星直选
			TJ_3xZux = 81,//三星组选
			TJ_2xZhix = 82,//二星直选
			TJ_2xZux = 83,//二星组选
			TJ_Bdw = 84,//不定位胆
			TJ_Dw = 85,//定位胆
			TJ_Rx = 86,//任选
			TJ_ZXHH = 87,//组选混合单式
			TJ_WX = 78,//五星
			TJ_SX = 79,//四星

			SD_Ele_Five_3xZhix = 88,//三星直选
			SD_Ele_Five_3xZux = 89,//三星组选
			SD_Ele_Five_2xZhix = 90,//二星直选
			SD_Ele_Five_2xZux = 91,//二星组选
			SD_Ele_Bdw = 92,//不定位胆
			SD_Ele_Dw = 93,//定位胆
			SD_Ele_RxFs = 94,//任选复式
			SD_Ele_RxDt = 95,//任选胆拖
			SD_Ele_RxDs = 197,//任选单式

			BJ_PK_Q1 = 96,//前一
			BJ_PK_Q2 = 97,//前二
			BJ_PK_Q3 = 98,//前三
			BJ_PK_Dw = 99,//定位胆
			BJ_PK_Q4 = 108,//前四
			BJ_PK_Q5 = 109,//前五
			BJ_PK_LH = 110,//龙虎
			BJ_PK_BS = 111,//大小
			BJ_PK_SD = 112,//单双
			BJ_PK_SV = 113,//和值
			BJ_PK_Q6 = 193,//前六

			De_PK_Q1 = 414,//前一
			De_PK_Q2 = 415,//前二
			De_PK_Q3 = 416,//前三
			De_PK_Dw = 417,//定位胆
			De_PK_Q4 = 418,//前四
			De_PK_Q5 = 419,//前五
			De_PK_LH = 420,//龙虎
			De_PK_BS = 421,//大小
			De_PK_SD = 422,//单双
			De_PK_SV = 423,//和值
			De_PK_Q6 = 424,//前六

			Italy_PK_Q1 = 873,//前一
			Italy_PK_Q2 = 874,//前二
			Italy_PK_Q3 = 875,//前三
			Italy_PK_Dw = 876,//定位胆
			Italy_PK_Q4 = 877,//前四
			Italy_PK_Q5 = 878,//前五
			Italy_PK_LH = 879,//龙虎
			Italy_PK_BS = 880,//大小
			Italy_PK_SD = 881,//单双
			Italy_PK_SV = 882,//和值
			Italy_PK_Q6 = 883,//前六

			HJ_Ele_Five_3xZhix = 100,//三星直选
			HJ_Ele_Five_3xZux = 101,//三星组选
			HJ_Ele_Five_2xZhix = 102,//二星直选
			HJ_Ele_Five_2xZux = 103,//二星组选
			HJ_Ele_Bdw = 104,//不定位胆
			HJ_Ele_Dw = 105,//定位胆
			HJ_Ele_RxFs = 106,//任选复式
			HJ_Ele_RxDt = 107,//任选胆拖
			HJ_Ele_RxDs = 198,//任选单式

			CQ_DXDS = 114,//大小单双
			JX_DXDS = 115,//大小单双
			Sinkiang_DXDS = 116,//大小单双
			TJ_DXDS = 117,//大小单双
			QQ_DXDS = 858,//大小单双
			Italy_DXDS = 905,//大小单双

			HJ_SFC_3xZhix = 118,//三星直选
			HJ_SFC_3xZux = 119,//三星组选
			HJ_SFC_2xZhix = 120,//二星直选
			HJ_SFC_2xZux = 121,//二星组选
			HJ_SFC_Bdw = 122,//不定位胆
			HJ_SFC_Dw = 123,//定位胆
			HJ_SFC_Rx = 124,//任选
			HJ_SFC_ZXHH = 125,//组选混合单式
			HJ_SFC_WX = 126,//五星
			HJ_SFC_SX = 127,//四星
			HJ_SFC_DXDS = 128,//大小单双

			HJ_PK_Q1 = 129,//前一
			HJ_PK_Q2 = 130,//前二
			HJ_PK_Q3 = 131,//前三
			HJ_PK_Dw = 132,//定位胆
			HJ_PK_Q4 = 133,//前四
			HJ_PK_Q5 = 134,//前五
			HJ_PK_LH = 135,//龙虎
			HJ_PK_BS = 136,//大小
			HJ_PK_SD = 137,//单双
			HJ_PK_SV = 138,//和值
			HJ_PK_Q6 = 194,//前六

			JSKS_3LTX = 139, // 三連號
			JSKS_3BTH = 140,
			JSKS_3THDX = 141,
			JSKS_3THTX = 142,
			JSKS_2THDX = 143,
			JSKS_2THFX = 144,
			JSKS_2BTX = 145,
			JSKS_HZ = 146,
			JSKS_DX = 147,
			JSKS_DS = 148,
			JSKS_C1G = 149,

			HSSEC_3xZhix = 152,//三星直选
			HSSEC_3xZux = 153,//三星组选
			HSSEC_2xZhix = 154,//二星直选
			HSSEC_2xZux = 155,//二星组选
			HSSEC_Bdw = 156,//不定位胆
			HSSEC_Dw = 157,//定位胆
			HSSEC_Rx = 158,//任选
			HSSEC_ZXHH = 159,//组选混合单式
			HSSEC_WX = 150,//五星
			HSSEC_SX = 151,//四星
			HSSEC_DXDS = 160,//大小单双

			HJ_PKHSSEC_Q1 = 161,//前一
			HJ_PKHSSEC_Q2 = 162,//前二
			HJ_PKHSSEC_Q3 = 163,//前三
			HJ_PKHSSEC_Dw = 164,//定位胆
			HJ_PKHSSEC_Q4 = 165,//前四
			HJ_PKHSSEC_Q5 = 166,//前五
			HJ_PKHSSEC_LH = 167,//龙虎
			HJ_PKHSSEC_BS = 168,//大小
			HJ_PKHSSEC_SD = 169,//单双
			HJ_PKHSSEC_SV = 170,//和值
			HJ_PKHSSEC_Q6 = 195,//前六


			TAIJIN_3xZhix = 173,//三星直选
			TAIJIN_3xZux = 174,//三星组选
			TAIJIN_2xZhix = 175,//二星直选
			TAIJIN_2xZux = 176,//二星组选
			TAIJIN_Bdw = 177,//不定位胆
			TAIJIN_Dw = 178,//定位胆
			TAIJIN_Rx = 179,//任选
			TAIJIN_ZXHH = 180,//组选混合单式
			TAIJIN_WX = 171,//五星
			TAIJIN_SX = 172,//四星
			TAIJIN_DXDS = 181,//大小单双

			TWBINGGO_3xZhix = 184,//三星直选
			TWBINGGO_3xZux = 185,//三星组选
			TWBINGGO_2xZhix = 186,//二星直选
			TWBINGGO_2xZux = 187,//二星组选
			TWBINGGO_Bdw = 188,//不定位胆
			TWBINGGO_Dw = 189,//定位胆
			TWBINGGO_Rx = 190,//任选
			TWBINGGO_ZXHH = 191,//组选混合单式
			TWBINGGO_WX = 182,//五星
			TWBINGGO_SX = 183,//四星
			TWBINGGO_DXDS = 192,//大小单双

			BJ8_3xZhix = 201,//三星直选
			BJ8_3xZux = 202,//三星组选
			BJ8_2xZhix = 203,//二星直选
			BJ8_2xZux = 204,//二星组选
			BJ8_Bdw = 205,//不定位胆
			BJ8_Dw = 206,//定位胆
			BJ8_Rx = 207,//任选
			BJ8_ZXHH = 208,//组选混合单式
			BJ8_WX = 199,//五星
			BJ8_SX = 200,//四星
			BJ8_DXDS = 209,//大小单双

			KOR5_3xZhix = 212,//三星直选
			KOR5_3xZux = 213,//三星组选
			KOR5_2xZhix = 214,//二星直选
			KOR5_2xZux = 215,//二星组选
			KOR5_Bdw = 216,//不定位胆
			KOR5_Dw = 217,//定位胆
			KOR5_Rx = 218,//任选
			KOR5_ZXHH = 219,//组选混合单式
			KOR5_WX = 210,//五星
			KOR5_SX = 211,//四星
			KOR5_DXDS = 220,//大小单双

			// 20180818 Yark 加入黑龍江時時彩的代碼
			// -------------- 经典 ---------------
			HLJ_3xZhix = 920,//三星直选
			HLJ_3xZux = 921,//三星组选
			HLJ_2xZhix = 922,//二星直选
			HLJ_2xZux = 923,//二星组选
			HLJ_Bdw = 924,//不定位胆
			HLJ_Dw = 925,//定位胆
			HLJ_Rx = 926,//任选
			HLJ_ZXHH = 927,//组选混合单式
			HLJ_WX = 928,//五星
			HLJ_SX = 929,//四星
			HLJ_DXDS = 930,//大小单双
			HLJ_Ele_WX = 931,//五星
			HLJ_Ele_SX = 932,//四星
			HLJ_Ele_A3x = 933,//后三
			HLJ_Ele_M3x = 934,//中三
			HLJ_Ele_F3x = 935,//前三
			HLJ_Ele_A2x = 936,//后二
			HLJ_Ele_F2x = 937,//前二
			HLJ_Ele_Dw = 938,//定位胆
			HLJ_Ele_Bdw = 939,//不定位胆
			HLJ_Ele_DXDS = 940,//大小单双
			HLJ_Ele_Rx2x = 941,//任二
			HLJ_Ele_Rx3x = 942,//任三
			HLJ_Ele_Rx4x = 943,//任四
			HLJ_Ele_Lhh = 944,//龙虎和

            //20181113 加入法國快三代碼 by Charles
            FRKS_3LTX = 945,    // 三连号通选
            FRKS_3BTH = 946,    // 三不同号
            FRKS_3THDX = 947,   //三同号单选
            FRKS_3THTX = 948,   //三同号通选
            FRKS_2THDX = 949,   //二同号单选
            FRKS_2THFX = 950,   //二同号复选
            FRKS_2BTX = 951,    //二不同号
            FRKS_HZ = 952,      //和值
            FRKS_DX = 953,      //大小
            FRKS_DS = 954,      //单双
            FRKS_C1G = 955,     //猜一个号

            // 幸運飛艇
            LKA_PK_Q1 = 956,//前一
            LKA_PK_Q2 = 957,//前二
            LKA_PK_Q3 = 958,//前三
            LKA_PK_Dw = 959,//定位胆
            LKA_PK_Q4 = 960,//前四
            LKA_PK_Q5 = 961,//前五
            LKA_PK_LH = 962,//龙虎
            LKA_PK_BS = 963,//大小
            LKA_PK_SD = 964,//单双
            LKA_PK_SV = 965,//和值
            LKA_PK_Q6 = 966,//前六

            // 腾讯五分彩
            QQ5_3xZhix = 978,//三星直选
            QQ5_3xZux = 979,//三星组选
            QQ5_2xZhix = 980,//二星直选
            QQ5_2xZux = 981,//二星组选
            QQ5_Bdw = 982,//不定位胆
            QQ5_Dw = 983,//定位胆
            QQ5_Rx = 984,//任选
            QQ5_ZXHH = 985,//组选混合单式
            QQ5_WX = 986,//五星
            QQ5_SX = 987,//四星
            QQ5_DXDS = 988,//大小单双

            // 威尼斯飞艇
            VNS_PK_Q1 = 1003,//前一
            VNS_PK_Q2 = 1004,//前二
            VNS_PK_Q3 = 1005,//前三
            VNS_PK_Dw = 1006,//定位胆
            VNS_PK_Q4 = 1007,//前四
            VNS_PK_Q5 = 1008,//前五
            VNS_PK_LH = 1009,//龙虎
            VNS_PK_BS = 1010,//大小
            VNS_PK_SD = 1011,//单双
            VNS_PK_SV = 1012,//和值
            VNS_PK_Q6 = 1013, //前六

            // 腾讯赛车分分彩
            QQRC_PK_Q1 = 1025,//前一
            QQRC_PK_Q2 = 1026,//前二
            QQRC_PK_Q3 = 1027,//前三
            QQRC_PK_Dw = 1028,//定位胆
            QQRC_PK_Q4 = 1029,//前四
            QQRC_PK_Q5 = 1030,//前五
            QQRC_PK_LH = 1031,//龙虎
            QQRC_PK_BS = 1032,//大小
            QQRC_PK_SD = 1033,//单双
            QQRC_PK_SV = 1034,//和值
            QQRC_PK_Q6 = 1035,//前六

            // 腾讯赛车5分彩
            QQRC5_PK_Q1 = 1047,//前一
            QQRC5_PK_Q2 = 1048,//前二
            QQRC5_PK_Q3 = 1049,//前三
            QQRC5_PK_Dw = 1050,//定位胆
            QQRC5_PK_Q4 = 1051,//前四
            QQRC5_PK_Q5 = 1052,//前五
            QQRC5_PK_LH = 1053,//龙虎
            QQRC5_PK_BS = 1054,//大小
            QQRC5_PK_SD = 1055,//单双
            QQRC5_PK_SV = 1056,//和值
            QQRC5_PK_Q6 = 1057, //前六

            // QQ分分彩
            MQQ_3xZhix = 1069, //三星直选
            MQQ_3xZux = 1070, //三星组选
            MQQ_2xZhix = 1071, //二星直选
            MQQ_2xZux = 1072, //二星组选
            MQQ_Bdw = 1073, //不定位胆
            MQQ_Dw = 1074, //定位胆
            MQQ_Rx = 1075, //任选
            MQQ_ZXHH = 1076, //组选混合单式
            MQQ_WX = 1077, //五星
            MQQ_SX = 1078, //四星
            MQQ_DXDS = 1079, //大小单双

            // QQ5分彩
            MQQ5_3xZhix = 1094, //三星直选
            MQQ5_3xZux = 1095, //三星组选
            MQQ5_2xZhix = 1096, //二星直选
            MQQ5_2xZux = 1097, //二星组选
            MQQ5_Bdw = 1098, //不定位胆
            MQQ5_Dw = 1099, //定位胆
            MQQ5_Rx = 1100, //任选
            MQQ5_ZXHH = 1101, //组选混合单式
            MQQ5_WX = 1102, //五星
            MQQ5_SX = 1103, //四星
            MQQ5_DXDS = 1104,

            // 鳳凰騰訊分分彩
            FHQQ_3xZhix = 1119, //三星直选
            FHQQ_3xZux = 1120, //三星组选
            FHQQ_2xZhix = 1121, //二星直选
            FHQQ_2xZux = 1122, //二星组选
            FHQQ_Bdw = 1123, //不定位胆
            FHQQ_Dw = 1124, //定位胆
            FHQQ_Rx = 1125, //任选
            FHQQ_ZXHH = 1126, //组选混合单式
            FHQQ_WX = 1127, //五星
            FHQQ_SX = 1128, //四星
            FHQQ_DXDS = 1129, //大小单双
            
            
            // 微信分分彩  usertype = 1的playtypeid
            WeixinQQ_3xZhix = 1144,//三星直选
            WeixinQQ_3xZux = 1145,//三星组选
            WeixinQQ_2xZhix = 1146,//二星直选
            WeixinQQ_2xZux = 1147,//二星组选
            WeixinQQ_Bdw = 1148,//不定位胆
            WeixinQQ_Dw = 1149,//定位胆
            WeixinQQ_Rx = 1150,//任选
            WeixinQQ_ZXHH = 1151,//组选混合单式
            WeixinQQ_WX = 1152,//五星
            WeixinQQ_SX = 1153,//四星
            WeixinQQ_DXDS = 1154,//大小单双
            
            
            // 江西11選5
            Jx11x5_3xZhix = 1169,//三星直选
            Jx11x5_3xZux = 1170,//三星组选
            Jx11x5_2xZhix = 1171,//二星直选
            Jx11x5_2xZux = 1172,//二星组选
            Jx11x5_Bdw = 1173,//不定位胆
            Jx11x5_Dw = 1174,//定位胆
            Jx11x5_RxFs = 1175,//任选复式
            Jx11x5_RxDt = 1176,//任选胆拖
            Jx11x5_RxDs = 1177,//任选单式

            // 江蘇11選5
            Js11x5_3xZhix = 1186,//三星直选
            Js11x5_3xZux = 1187,//三星组选
            Js11x5_2xZhix = 1188,//二星直选
            Js11x5_2xZux = 1189,//二星组选
            Js11x5_Bdw = 1190,//不定位胆
            Js11x5_Dw = 1191,//定位胆
            Js11x5_RxFs = 1192,//任选复式
            Js11x5_RxDt = 1193,//任选胆拖
            Js11x5_RxDs = 1194,//任选单式

            VietSSC_3xZhix = 1203, //三星直选
            VietSSC_3xZux = 1204, //三星组选
            VietSSC_2xZhix = 1205, //二星直选
            VietSSC_2xZux = 1206, //二星组选
            VietSSC_Bdw = 1207, //不定位胆
            VietSSC_Dw = 1208, //定位胆
            VietSSC_Rx = 1209, //任选
            VietSSC_ZXHH = 1210, //组选混合单式
            VietSSC_WX = 1211, //五星
            VietSSC_SX = 1212, //四星
            VietSSC_DXDS = 1213, //大小单双

            // 奇趣腾讯分分彩 usertype = 1的playtypeid
            ChichuQQ_3xZhix = 1279,//三星直选
            ChichuQQ_3xZux = 1280,//三星组选
            ChichuQQ_2xZhix = 1281,//二星直选
            ChichuQQ_2xZux = 1282,//二星组选
            ChichuQQ_Bdw = 1283,//不定位胆
            ChichuQQ_Dw = 1284,//定位胆
            ChichuQQ_Rx = 1285,//任选
            ChichuQQ_ZXHH = 1286,//组选混合单式
            ChichuQQ_WX = 1287,//五星
            ChichuQQ_SX = 1288,//四星
            ChichuQQ_DXDS = 1289,//大小单双
        }

		public enum PlayTypeRadio
		{
			//-----SSC_HLJSST----
			// -------------- 专家 ---------------
			HLJ_五星直选直选复式 = 2723, HLJ_五星直选直选单式 = 2724,
			HLJ_五星组合 = 2634, HLJ_五星组选120 = 2635, HLJ_五星组选60 = 2636, HLJ_五星组选30 = 2637, HLJ_五星组选20 = 2638, HLJ_五星组选10 = 2639, HLJ_五星组选5 = 2640,
			HLJ_五星一帆风顺 = 2641, HLJ_五星好事成双 = 2642, HLJ_五星三星报喜 = 2643, HLJ_五星四季发财 = 2644,

			HLJ_四星直选直选复式 = 2725, HLJ_四星直选直选单式 = 2726,
			HLJ_四星组合 = 2645, HLJ_四星组选24 = 2646, HLJ_四星组选12 = 2647, HLJ_四星组选6 = 2648, HLJ_四星组选4 = 2649,

			HLJ_前三直选直选复式 = 2598, HLJ_前三直选直选单式 = 2599,
			HLJ_前三直选组合 = 2670, HLJ_前三直选和值 = 2671, HLJ_前三直选跨度 = 2672, HLJ_前三组三单式 = 2673,
			HLJ_前三组六单式 = 2674, HLJ_前三混合组选 = 2675, HLJ_前三组选和值 = 2676, HLJ_前三组选包胆 = 2677,
			HLJ_前三和值尾数 = 2678,
			HLJ_前三特殊号 = 2718,
			HLJ_前三组六复式复式 = 2737, HLJ_前三组三复式复式 = 2738,

			HLJ_中三直选直选复式 = 2600, HLJ_中三直选直选单式 = 2601,
			HLJ_中三直选组合 = 2660, HLJ_中三直选和值 = 2661, HLJ_中三直选跨度 = 2662, HLJ_中三组三单式 = 2663,
			HLJ_中三组六单式 = 2664, HLJ_中三混合组选 = 2665, HLJ_中三组选和值 = 2666, HLJ_中三组选包胆 = 2667,
			HLJ_中三和值尾数 = 2668,
			HLJ_中三特殊号 = 2727,
			HLJ_中三组六复式复式 = 2739, HLJ_中三组三复式复式 = 2740,

			HLJ_后三直选直选复式 = 2602, HLJ_后三直选直选单式 = 2603,
			HLJ_后三直选组合 = 2650, HLJ_后三直选和值 = 2651, HLJ_后三直选跨度 = 2652, HLJ_后三组三单式 = 2653,
			HLJ_后三组六单式 = 2654, HLJ_后三混合组选 = 2655, HLJ_后三组选和值 = 2656, HLJ_后三组选包胆 = 2657,
			HLJ_后三和值尾数 = 2658,
			HLJ_后三特殊号 = 2732,
			HLJ_后三组六复式复式 = 2741, HLJ_后三组三复式复式 = 2742,

			HLJ_前二直选直选复式 = 2743, HLJ_前二直选直选单式 = 2744, HLJ_前二直选和值 = 2745, HLJ_前二直选跨度 = 2746,
			HLJ_前二组选组选复式 = 2747, HLJ_前二组选组选单式 = 2748, HLJ_前二组选和值 = 2784, HLJ_前二组选包胆 = 2785,

			HLJ_后二直选直选复式 = 2749, HLJ_后二直选直选单式 = 2750, HLJ_后二直选和值 = 2751, HLJ_后二直选跨度 = 2752,
			HLJ_后二组选组选复式 = 2753, HLJ_后二组选组选单式 = 2754, HLJ_后二组选和值 = 2782, HLJ_后二组选包胆 = 2783,

			HLJ_定位胆 = 2686,

			HLJ_后三一码 = 2687, HLJ_前三一码 = 2688,
			HLJ_后三二码 = 2689, HLJ_前三二码 = 2690,
			HLJ_四星一码 = 2691, HLJ_四星二码 = 2692,
			HLJ_五星二码 = 2693, HLJ_五星三码 = 2694,

			HLJ_前二组选大小单双 = 2755, HLJ_后二组选大小单双 = 2756, HLJ_前三组选大小单双 = 2757, HLJ_后三组选大小单双 = 2758, HLJ_总和大小单双 = 2695,

			HLJ_任二直选复式 = 2696, HLJ_任二直选单式 = 2697, HLJ_任二直选和值 = 2698, HLJ_任二组选复式 = 2699, HLJ_任二组选单式 = 2700, HLJ_任二组选和值 = 2701,
			HLJ_任三直选复式 = 2702, HLJ_任三直选单式 = 2703, HLJ_任三直选和值 = 2704, HLJ_任三组三复式 = 2705, HLJ_任三组三单式 = 2706, HLJ_任三组六复式 = 2707, HLJ_任三组六单式 = 2708, HLJ_任三混合组选 = 2709, HLJ_任三组选和值 = 2710,
			HLJ_任四直选复式 = 2711, HLJ_任四直选单式 = 2712, HLJ_任四组选24 = 2713, HLJ_任四组选12 = 2714, HLJ_任四组选6 = 2715, HLJ_任四组选4 = 2716,
			HLJ_龙虎和 = 2717,

			// -------------- 经典 ---------------
			HLJ_五星直选复式 = 2592, HLJ_五星直选单式 = 2593,
			HLJ_前四直选复式 = 2594, HLJ_前四直选单式 = 2595, HLJ_后四直选复式 = 2596, HLJ_后四直选单式 = 2597,
			HLJ_前三直选复式 = 2761, HLJ_前三直选单式 = 2762, HLJ_中三直选复式 = 2763, HLJ_中三直选单式 = 2764, HLJ_后三直选复式 = 2765, HLJ_后三直选单式 = 2766,
			HLJ_前三组六复式 = 2604, HLJ_前三组三复式 = 2605, HLJ_中三组六复式 = 2606, HLJ_中三组三复式 = 2607, HLJ_后三组六复式 = 2608, HLJ_后三组三复式 = 2609,
			HLJ_前二直选复式 = 2610, HLJ_前二直选单式 = 2611, HLJ_后二直选复式 = 2612, HLJ_后二直选单式 = 2613,
			HLJ_前二组选复式 = 2614, HLJ_后二组选复式 = 2615,
			HLJ_前三不定胆 = 2616, HLJ_中三不定胆 = 2617, HLJ_后三不定胆 = 2618, HLJ_五星不定胆 = 2619,
			HLJ_五星定位胆 = 2620,
			HLJ_任选四复式 = 2621, HLJ_任选四单式 = 2622, HLJ_任选三复式 = 2623, HLJ_任选三单式 = 2624, HLJ_任选二复式 = 2625, HLJ_任选二单式 = 2626,
			HLJ_前三混合单式 = 2627, HLJ_中三混合单式 = 2628, HLJ_后三混合单式 = 2629,
			HLJ_前二大小单双 = 2630, HLJ_后二大小单双 = 2631, HLJ_前三大小单双 = 2632, HLJ_后三大小单双 = 2633,

			//-----SSC_CQ-----
			CQ_前三直选复式 = 165, CQ_前三直选单式 = 166, CQ_中三直选复式 = 167, CQ_中三直选单式 = 168,
			CQ_后三直选复式 = 169, CQ_后三直选单式 = 170, CQ_前三组六复式 = 171, CQ_前三组三复式 = 172, CQ_前三组选单式 = 173, CQ_中三组六复式 = 174,
			CQ_中三组三复式 = 175, CQ_后三组六复式 = 254,
			CQ_后三组三复式 = 255, CQ_后三组选单式 = 176, CQ_前二直选复式 = 177, CQ_前二直选单式 = 178,
			CQ_后二直选复式 = 179, CQ_后二直选单式 = 180, CQ_前二组选复式 = 181, CQ_前二组选单式 = 182,
			CQ_后二组选复式 = 183,
			CQ_后二组选单式 = 184,
			CQ_前三不定胆 = 187, CQ_中三不定胆 = 188, CQ_后三不定胆 = 189, CQ_五星不定胆 = 190,
			CQ_五星定位胆 = 192, CQ_任选三复式 = 246, CQ_任选三单式 = 247, CQ_任选二复式 = 248, CQ_任选二单式 = 249,
			CQ_前三混合单式 = 292, CQ_中三混合单式 = 293, CQ_后三混合单式 = 294, CQ_五星直选复式 = 304, CQ_五星直选单式 = 305,
			CQ_五星组合 = 306, CQ_后四星直选复式 = 307, CQ_后四星直选单式 = 308, CQ_四星组合 = 309,
			CQ_前四星直选复式 = 482, CQ_前四星直选单式 = 483, CQ_任选五复式 = 510, CQ_任选五单式 = 511, CQ_任选四复式 = 512,
			CQ_任选四单式 = 513, CQ_前二大小单双 = 526, CQ_后二大小单双 = 527, CQ_前三大小单双 = 528, CQ_后三大小单双 = 529,
			
			//-----SSC_QQ-----
			QQ_前三直选复式 = 2223,
			QQ_前三直选单式 = 2224,
			QQ_中三直选复式 = 2225,
			QQ_中三直选单式 = 2226,
			QQ_后三直选复式 = 2227,
			QQ_后三直选单式 = 2228,
			QQ_前三组六复式 = 2229,
			QQ_前三组三复式 = 2230,
			QQ_中三组六复式 = 2231,
			QQ_中三组三复式 = 2232,
			QQ_前二直选复式 = 2233,
			QQ_前二直选单式 = 2234,
			QQ_后二直选复式 = 2235,
			QQ_后二直选单式 = 2236,
			QQ_前二组选复式 = 2237,
			QQ_后二组选复式 = 2238,
			QQ_前三不定胆 = 2239,
			QQ_中三不定胆 = 2240,
			QQ_后三不定胆 = 2241,
			QQ_五星不定胆 = 2242,
			QQ_五星定位胆 = 2243,
			QQ_任选三复式 = 2244,
			QQ_任选三单式 = 2245,
			QQ_任选二复式 = 2246,
			QQ_任选二单式 = 2247,
			QQ_后三组六复式 = 2248,
			QQ_后三组三复式 = 2249,
			QQ_前三混合单式 = 2250,
			QQ_中三混合单式 = 2251,
			QQ_后三混合单式 = 2252,
			QQ_直选复式 = 2253,
			QQ_直选单式 = 2254,
			QQ_后四直选复式 = 2255,
			QQ_后四直选单式 = 2256,
			QQ_前四直选复式 = 2257,
			QQ_前四直选单式 = 2258,
			QQ_任选四复式 = 2259,
			QQ_任选四单式 = 2260,
			QQ_前二大小单双 = 2261,
			QQ_后二大小单双 = 2262,
			QQ_前三大小单双 = 2263,
			QQ_后三大小单双 = 2264,
			//-----SSC_Italy-----
			Italy_前三直选复式 = 2436,
			Italy_前三直选单式 = 2437,
			Italy_中三直选复式 = 2438,
			Italy_中三直选单式 = 2439,
			Italy_后三直选复式 = 2440,
			Italy_后三直选单式 = 2441,
			Italy_前三组六复式 = 2442,
			Italy_前三组三复式 = 2443,
			Italy_中三组六复式 = 2444,
			Italy_中三组三复式 = 2445,
			Italy_后三组六复式 = 2446,
			Italy_后三组三复式 = 2447,
			Italy_前二直选复式 = 2448,
			Italy_前二直选单式 = 2449,
			Italy_后二直选复式 = 2450,
			Italy_后二直选单式 = 2451,
			Italy_前二组选复式 = 2452,
			Italy_后二组选复式 = 2453,
			Italy_前三不定胆 = 2454,
			Italy_中三不定胆 = 2455,
			Italy_后三不定胆 = 2456,
			Italy_五星不定胆 = 2457,
			Italy_五星定位胆 = 2458,
			Italy_任选三复式 = 2459,
			Italy_任选三单式 = 2460,
			Italy_任选二复式 = 2461,
			Italy_任选二单式 = 2462,
			Italy_任选四复式 = 2463,
			Italy_任选四单式 = 2464,
			Italy_前三混合单式 = 2465,
			Italy_中三混合单式 = 2466,
			Italy_后三混合单式 = 2467,
			Italy_直选复式 = 2468,
			Italy_直选单式 = 2469,
			Italy_后四直选复式 = 2470,
			Italy_后四直选单式 = 2471,
			Italy_前四直选复式 = 2472,
			Italy_前四直选单式 = 2473,
			Italy_前二大小单双 = 2474,
			Italy_后二大小单双 = 2475,
			Italy_前三大小单双 = 2476,
			Italy_后三大小单双 = 2477,
			//------------SSC_JX          
			JX_前三直选复式 = 193, JX_前三直选单式 = 194, JX_中三直选复式 = 195, JX_中三直选单式 = 196,
			JX_后三直选复式 = 197, JX_后三直选单式 = 198, JX_前三组六复式 = 199, JX_前三组三复式 = 200, JX_前三组选单式 = 201, JX_中三组六复式 = 202,
			JX_中三组三复式 = 203, JX_后三组六复式 = 256,
			JX_后三组三复式 = 257, JX_后三组选单式 = 204, JX_前二直选复式 = 205, JX_前二直选单式 = 206,
			JX_后二直选复式 = 207, JX_后二直选单式 = 208, JX_前二组选复式 = 209, JX_前二组选单式 = 210,
			JX_后二组选复式 = 211,
			JX_后二组选单式 = 212, JX_前三不定胆 = 216, JX_中三不定胆 = 217, JX_后三不定胆 = 218, JX_五星不定胆 = 219,
			JX_五星定位胆 = 220, JX_任选三复式 = 250, JX_任选三单式 = 251, JX_任选二复式 = 252, JX_任选二单式 = 253,
			JX_前三混合单式 = 295, JX_中三混合单式 = 296, JX_后三混合单式 = 297, JX_五星直选复式 = 310, JX_五星直选单式 = 311,
			JX_五星组合 = 312, JX_后四星直选复式 = 313,
			JX_后四星直选单式 = 314, JX_四星组合 = 315,
			JX_前四星直选复式 = 484, JX_前四星直选单式 = 485, JX_任选五复式 = 514, JX_任选五单式 = 515, JX_任选四复式 = 516,
			JX_任选四单式 = 517, JX_前二大小单双 = 530, JX_后二大小单双 = 531, JX_前三大小单双 = 532, JX_后三大小单双 = 533,
			//-------------SSC_FC3D-----------------
			FC3D_直选复式 = 221, FC3D_直选单式 = 222, FC3D_组六复式 = 223, FC3D_组三复式 = 224, FC3D_组选单式 = 225, FC3D_前二直选复式 = 226, FC3D_前二直选单式 = 227, FC3D_后二直选复式 = 228, FC3D_后二直选单式 = 229, FC3D_不定位胆 = 230, FC3D_定位胆 = 232, FC3D_混合单式 = 298,
			//-------------SSC_TC3-----------------
			TC3_直选复式 = 233, TC3_直选单式 = 234, TC3_组六复式 = 235, TC3_组三复式 = 236, TC3_组选单式 = 237, TC3_前二直选复式 = 238, TC3_前二直选单式 = 239, TC3_后二直选复式 = 240, TC3_后二直选单式 = 241, TC3_不定位胆 = 242, TC3_定位胆 = 245, TC3_混合单式 = 299,
			//-------------SSC_Ele_Five------------
			SSCEleFive_前三直选复式 = 258, SSCEleFive_前三直选单式 = 259, SSCEleFive_中三直选复式 = 260,
			SSCEleFive_中三直选单式 = 261, SSCEleFive_后三直选复式 = 262, SSCEleFive_后三直选单式 = 263,
			SSCEleFive_3前三组选 = 264, SSCEleFive_3中三组选 = 265, SSCEleFive_3后三组选 = 266,
			SSCEleFive_前二直选复式 = 267, SSCEleFive_前二直选单式 = 268, SSCEleFive_后二直选复式 = 269,
			SSCEleFive_后二直选单式 = 270, SSCEleFive_2前二组选 = 271, SSCEleFive_2后二组选 = 272,
			SSCEleFive_前三不定胆 = 273,
			//SSCEleFive_中三不定胆 = 274, SSCEleFive_后三不定胆 = 275,
			SSCEleFive_前三定位胆 = 276, SSCEleFiveRX_一中一 = 277, SSCEleFiveRX_二中二 = 278, SSCEleFiveRX_三中三 = 279,
			SSCEleFiveRX_四中四 = 280, SSCEleFiveRX_五中五 = 281, SSCEleFiveRX_六中五 = 282, SSCEleFiveRX_七中五 = 283,
			SSCEleFiveRX_八中五 = 284, SSCEleFive_d二中二 = 285, SSCEleFive_d三中三 = 286, SSCEleFive_d四中四 = 287,
			SSCEleFive_d五中五 = 288, SSCEleFive_d六中五 = 289, SSCEleFive_d七中五 = 290, SSCEleFive_d八中五 = 291,

			SSCEleFiveDS_一中一 = 789, SSCEleFiveDS_二中二 = 790, SSCEleFiveDS_三中三 = 791,
			SSCEleFiveDS_四中四 = 792, SSCEleFiveDS_五中五 = 793, SSCEleFiveDS_六中五 = 794, SSCEleFiveDS_七中五 = 795,
			SSCEleFiveDS_八中五 = 796,
			//---xjssc----
			Sinkiang_前三直选复式 = 322, Sinkiang_前三直选单式 = 323, Sinkiang_中三直选复式 = 324, Sinkiang_中三直选单式 = 325,
			Sinkiang_后三直选复式 = 326, Sinkiang_后三直选单式 = 327, Sinkiang_前三组六复式 = 328, Sinkiang_前三组三复式 = 329, Sinkiang_中三组六复式 = 330,
			Sinkiang_中三组三复式 = 331, Sinkiang_后三组六复式 = 347, Sinkiang_后三组三复式 = 348,
			Sinkiang_前二直选复式 = 332, Sinkiang_前二直选单式 = 333,
			Sinkiang_后二直选复式 = 334, Sinkiang_后二直选单式 = 335, Sinkiang_前二组选复式 = 336,
			Sinkiang_后二组选复式 = 337,
			Sinkiang_前三不定胆 = 338, Sinkiang_中三不定胆 = 339, Sinkiang_后三不定胆 = 340, Sinkiang_五星不定胆 = 341,
			Sinkiang_五星定位胆 = 342, Sinkiang_任选三复式 = 343, Sinkiang_任选三单式 = 344, Sinkiang_任选二复式 = 345,
			Sinkiang_任选二单式 = 346, Sinkiang_前三混合单式 = 349, Sinkiang_中三混合单式 = 350, Sinkiang_后三混合单式 = 351,
			Sinkiang_五星直选复式 = 316, Sinkiang_五星直选单式 = 317, Sinkiang_后四星直选复式 = 319, Sinkiang_后四星直选单式 = 320,
			Sinkiang_前四星直选复式 = 486, Sinkiang_前四星直选单式 = 487, Sinkiang_任选五复式 = 522, Sinkiang_任选五单式 = 523, Sinkiang_任选四复式 = 524,
			Sinkiang_任选四单式 = 525, Sinkiang_前二大小单双 = 534, Sinkiang_后二大小单双 = 535, Sinkiang_前三大小单双 = 536,
			Sinkiang_后三大小单双 = 537,
			//----重庆十一选五
			CQCCSEleFive_前三直选复式 = 352, CQCCSEleFive_前三直选单式 = 353, CQCCSEleFive_中三直选复式 = 354,
			CQCCSEleFive_中三直选单式 = 355, CQCCSEleFive_后三直选复式 = 356, CQCCSEleFive_后三直选单式 = 357,
			CQCCSEleFive_3前三组选 = 358, CQCCSEleFive_3中三组选 = 359, CQCCSEleFive_3后三组选 = 360,
			CQCCSEleFive_前二直选复式 = 361, CQCCSEleFive_前二直选单式 = 362, CQCCSEleFive_后二直选复式 = 363,
			CQCCSEleFive_后二直选单式 = 364, CQCCSEleFive_2前二组选 = 365, CQCCSEleFive_2后二组选 = 366,
			CQCCSEleFive_前三不定胆 = 367,
			//CQCCSEleFive_中三不定胆 = 274, CQCCSEleFive_后三不定胆 = 275,
			CQCCSEleFive_前三定位胆 = 368, CQCCSEleFiveRX_一中一 = 369, CQCCSEleFiveRX_二中二 = 370, CQCCSEleFiveRX_三中三 = 371, CQCCSEleFiveRX_四中四 = 372, CQCCSEleFiveRX_五中五 = 373, CQCCSEleFiveRX_六中五 = 374, CQCCSEleFiveRX_七中五 = 375, CQCCSEleFiveRX_八中五 = 376, CQCCSEleFive_d二中二 = 377, CQCCSEleFive_d三中三 = 378, CQCCSEleFive_d四中四 = 379, CQCCSEleFive_d五中五 = 380, CQCCSEleFive_d六中五 = 381, CQCCSEleFive_d七中五 = 382, CQCCSEleFive_d八中五 = 383,
			//--新疆时时彩
			TJ_五星直选复式 = 384, TJ_五星直选单式 = 385, TJ_后四星直选复式 = 386, TJ_后四星直选单式 = 387,
			TJ_前三直选复式 = 388, TJ_前三直选单式 = 389, TJ_中三直选复式 = 390, TJ_中三直选单式 = 391,
			TJ_后三直选复式 = 392, TJ_后三直选单式 = 393, TJ_前三组六复式 = 394, TJ_前三组三复式 = 395, TJ_中三组六复式 = 396,
			TJ_中三组三复式 = 397, TJ_后三组六复式 = 398, TJ_后三组三复式 = 399,
			TJ_前二直选复式 = 400, TJ_前二直选单式 = 401,
			TJ_后二直选复式 = 402, TJ_后二直选单式 = 403, TJ_前二组选复式 = 404,
			TJ_后二组选复式 = 405,
			TJ_前三不定胆 = 406, TJ_中三不定胆 = 407, TJ_后三不定胆 = 408, TJ_五星不定胆 = 409, TJ_五星定位胆 = 410,
			TJ_任选三复式 = 411, TJ_任选三单式 = 412, TJ_任选二复式 = 413, TJ_任选二单式 = 414, TJ_前三混合单式 = 415,
			TJ_中三混合单式 = 416, TJ_后三混合单式 = 417, TJ_前四星直选复式 = 497, TJ_前四星直选单式 = 498, TJ_任选五复式 = 518, TJ_任选五单式 = 519, TJ_任选四复式 = 520,
			TJ_任选四单式 = 521, TJ_前二大小单双 = 538, TJ_后二大小单双 = 539, TJ_前三大小单双 = 540,
			TJ_后三大小单双 = 541,
			//山东十一选五
			SDCCSEleFive_前三直选复式 = 418, SDCCSEleFive_前三直选单式 = 419, SDCCSEleFive_中三直选复式 = 420,
			SDCCSEleFive_中三直选单式 = 421, SDCCSEleFive_后三直选复式 = 422, SDCCSEleFive_后三直选单式 = 423,
			SDCCSEleFive_3前三组选 = 424, SDCCSEleFive_3中三组选 = 425, SDCCSEleFive_3后三组选 = 426,
			SDCCSEleFive_前二直选复式 = 427, SDCCSEleFive_前二直选单式 = 428, SDCCSEleFive_后二直选复式 = 429,
			SDCCSEleFive_后二直选单式 = 430, SDCCSEleFive_2前二组选 = 431, SDCCSEleFive_2后二组选 = 432,
			SDCCSEleFive_前三不定胆 = 433,
			SDCCSEleFive_前三定位胆 = 434, SDCCSEleFiveRX_一中一 = 435, SDCCSEleFiveRX_二中二 = 436, SDCCSEleFiveRX_三中三 = 437,
			SDCCSEleFiveRX_四中四 = 438, SDCCSEleFiveRX_五中五 = 439, SDCCSEleFiveRX_六中五 = 440,
			SDCCSEleFiveRX_七中五 = 441, SDCCSEleFiveRX_八中五 = 442, SDCCSEleFive_d二中二 = 443,
			SDCCSEleFive_d三中三 = 444, SDCCSEleFive_d四中四 = 445, SDCCSEleFive_d五中五 = 446, SDCCSEleFive_d六中五 = 447,
			SDCCSEleFive_d七中五 = 448, SDCCSEleFive_d八中五 = 449,

			SDCCSEleFiveDS_一中一 = 797, SDCCSEleFiveDS_二中二 = 798, SDCCSEleFiveDS_三中三 = 799,
			SDCCSEleFiveDS_四中四 = 800, SDCCSEleFiveDS_五中五 = 801, SDCCSEleFiveDS_六中五 = 802,
			SDCCSEleFiveDS_七中五 = 803, SDCCSEleFiveDS_八中五 = 804,

			// 北京PK10
			BJPK_前一直选复式 = 450, BJPK_前二直选复式 = 451, BJPK_前二直选单式 = 452, BJPK_前三直选复式 = 453,
			BJPK_前三直选单式 = 454, BJPK_定位胆 = 455, //BJPK_610定位胆 = 740,
			BJPK_前四直选复式 = 488, BJPK_前四直选单式 = 489,
			BJPK_前五直选复式 = 490, BJPK_前五直选单式 = 491, BJPK_龙虎斗_1Vs10 = 492, BJPK_龙虎斗_2Vs9 = 493,
			BJPK_龙虎斗_3Vs8 = 494, BJPK_龙虎斗_4Vs7 = 495, BJPK_龙虎斗_5Vs6 = 496, BJPK_大小_冠军 = 499, BJPK_大小_亚军 = 500, BJPK_大小_季军 = 501, BJPK_大小_第四名 = 502, BJPK_大小_第五名 = 503,
			BJPK_单双_冠军 = 504, BJPK_单双_亚军 = 505, BJPK_单双_季军 = 506, BJPK_单双_第四名 = 507, BJPK_单双_第五名 = 508,
			BJPK_冠亚和值 = 509, BJPK_前六直选复式 = 783, BJPK_前六直选单式 = 784,

			//--DePK10
			DePK_前一直选复式 = 2166, DePK_前二直选复式 = 2167, DePK_前二直选单式 = 2168, DePK_前三直选复式 = 2169,
			DePK_前三直选单式 = 2170, DePK_定位胆 = 2171,
			DePK_前四直选复式 = 2172, DePK_前四直选单式 = 2173,
			DePK_前五直选复式 = 2174, DePK_前五直选单式 = 2175, DePK_龙虎斗_1Vs10 = 2176, DePK_龙虎斗_2Vs9 = 2177,
			DePK_龙虎斗_3Vs8 = 2178, DePK_龙虎斗_4Vs7 = 2179, DePK_龙虎斗_5Vs6 = 217780, DePK_大小_冠军 = 2181, DePK_大小_亚军 = 2182, DePK_大小_季军 = 2183, DePK_大小_第四名 = 2184, DePK_大小_第五名 = 2185,
			DePK_单双_冠军 = 2186, DePK_单双_亚军 = 2187, DePK_单双_季军 = 2188, DePK_单双_第四名 = 2189, DePK_单双_第五名 = 2190,
			DePK_冠亚和值 = 2191, DePK_前六直选复式 = 2192, DePK_前六直选单式 = 2193,

			//--ItalyPK10
			ItalyPK_前一直选复式 = 2379, ItalyPK_前二直选复式 = 2380, ItalyPK_前二直选单式 = 2381, ItalyPK_前三直选复式 = 2382,
			ItalyPK_前三直选单式 = 2383, ItalyPK_定位胆 = 2384,
			ItalyPK_前四直选复式 = 2385, ItalyPK_前四直选单式 = 2386,
			ItalyPK_前五直选复式 = 2387, ItalyPK_前五直选单式 = 2388, ItalyPK_龙虎斗_1Vs10 = 2389, ItalyPK_龙虎斗_2Vs9 = 2390,
			ItalyPK_龙虎斗_3Vs8 = 2391, ItalyPK_龙虎斗_4Vs7 = 2392, ItalyPK_龙虎斗_5Vs6 = 2393, ItalyPK_大小_冠军 = 2394, ItalyPK_大小_亚军 = 2395, ItalyPK_大小_季军 = 2396, ItalyPK_大小_第四名 = 2397, ItalyPK_大小_第五名 = 2398,
			ItalyPK_单双_冠军 = 2399, ItalyPK_单双_亚军 = 2400, ItalyPK_单双_季军 = 2401, ItalyPK_单双_第四名 = 2402, ItalyPK_单双_第五名 = 2403,
			ItalyPK_冠亚和值 = 2404, ItalyPK_前六直选复式 = 2405, ItalyPK_前六直选单式 = 2406,

			//HJ十一选五
			HJEleFive_前三直选复式 = 456, HJEleFive_前三直选单式 = 457,
			HJEleFive_3前三组选 = 458,
			HJEleFive_前二直选复式 = 459, HJEleFive_前二直选单式 = 460, HJEleFive_后二直选复式 = 461,
			HJEleFive_后二直选单式 = 462, HJEleFive_2前二组选 = 463, HJEleFive_2后二组选 = 464,
			HJEleFive_前三不定胆 = 465,
			HJEleFive_前三定位胆 = 466,
			HJEleFive_一中一 = 467, HJEleFive_二中二 = 468, HJEleFive_三中三 = 469,
			HJEleFive_四中四 = 470, HJEleFive_五中五 = 471,
			HJEleFive_六中五 = 472, HJEleFive_七中五 = 473, HJEleFive_八中五 = 474, HJEleFive_d二中二 = 475,
			HJEleFive_d三中三 = 476, HJEleFive_d四中四 = 477, HJEleFive_d五中五 = 478, HJEleFive_d六中五 = 479,
			HJEleFive_d七中五 = 480, HJEleFive_d八中五 = 481,

			HJEleFiveDS_一中一 = 805, HJEleFiveDS_二中二 = 806, HJEleFiveDS_三中三 = 807,
			HJEleFiveDS_四中四 = 808, HJEleFiveDS_五中五 = 809,
			HJEleFiveDS_六中五 = 810, HJEleFiveDS_七中五 = 811, HJEleFiveDS_八中五 = 812,
			//三分彩
			HJ_SFC_前三直选复式 = 542, HJ_SFC_前三直选单式 = 543, HJ_SFC_中三直选复式 = 544, HJ_SFC_中三直选单式 = 545,
			HJ_SFC_后三直选复式 = 546, HJ_SFC_后三直选单式 = 547, HJ_SFC_前三组六复式 = 548, HJ_SFC_前三组三复式 = 549,
			HJ_SFC_中三组六复式 = 550,
			HJ_SFC_中三组三复式 = 551,
			HJ_SFC_后三组六复式 = 552,
			HJ_SFC_后三组三复式 = 553,
			HJ_SFC_前二直选复式 = 554, HJ_SFC_前二直选单式 = 555,
			HJ_SFC_后二直选复式 = 556, HJ_SFC_后二直选单式 = 557,
			HJ_SFC_前二组选复式 = 558,
			HJ_SFC_后二组选复式 = 559,
			HJ_SFC_前三不定胆 = 560, HJ_SFC_中三不定胆 = 561, HJ_SFC_后三不定胆 = 562, HJ_SFC_五星不定胆 = 563,
			HJ_SFC_五星定位胆 = 564, HJ_SFC_任选三复式 = 565, HJ_SFC_任选三单式 = 566, HJ_SFC_任选二复式 = 567,
			HJ_SFC_任选二单式 = 568, HJ_SFC_任选四复式 = 569,
			HJ_SFC_任选四单式 = 570, HJ_SFC_前三混合单式 = 571, HJ_SFC_中三混合单式 = 572, HJ_SFC_后三混合单式 = 573,
			HJ_SFC_五星直选复式 = 574,
			HJ_SFC_五星直选单式 = 575, HJ_SFC_后四星直选复式 = 576, HJ_SFC_后四星直选单式 = 577,
			HJ_SFC_前四星直选复式 = 578, HJ_SFC_前四星直选单式 = 579, HJ_SFC_前二大小单双 = 580,
			HJ_SFC_后二大小单双 = 581, HJ_SFC_前三大小单双 = 582, HJ_SFC_后三大小单双 = 583,

			// 和记PK10
			HJ_PK_前一直选复式 = 584, HJ_PK_前二直选复式 = 585, HJ_PK_前二直选单式 = 586, HJ_PK_前三直选复式 = 587,
			HJ_PK_前三直选单式 = 588, HJ_PK_定位胆 = 589,
			// HJ_PK_6到10定位胆 = 610,
			HJ_PK_前四直选复式 = 590,
			HJ_PK_前四直选单式 = 591,
			HJ_PK_前五直选复式 = 592, HJ_PK_前五直选单式 = 593, HJ_PK_龙虎斗_1Vs10 = 594, HJ_PK_龙虎斗_2Vs9 = 595,
			HJ_PK_龙虎斗_3Vs8 = 596, HJ_PK_龙虎斗_4Vs7 = 597, HJ_PK_龙虎斗_5Vs6 = 598,
			HJ_PK_大小_冠军 = 599, HJ_PK_大小_亚军 = 600, HJ_PK_大小_季军 = 601, HJ_PK_大小_第四名 = 602, HJ_PK_大小_第五名 = 603,
			HJ_PK_单双_冠军 = 604, HJ_PK_单双_亚军 = 605, HJ_PK_单双_季军 = 606, HJ_PK_单双_第四名 = 607, HJ_PK_单双_第五名 = 608,
			HJ_PK_冠亚和值 = 609, HJ_PK_前六直选复式 = 785, HJ_PK_前六直选单式 = 786,

			// 江蘇快三
			JSKS_三连号通选 = 611, JSKS_三不同标准选号 = 612, JSKS_三不同标准选号单式 = 613, JSKS_三不同胆拖选号 = 614, JSKS_三不同和值 = 615,
			JSKS_三同号单选 = 616, JSKS_三同号通选 = 617, JSKS_二同号单选 = 618, JSKS_二同号单式 = 619, JSKS_二同号复选 = 620,
			JSKS_二不同号选号 = 621, JSKS_二不同号单式 = 622, JSKS_二不同胆拖选号 = 623, JSKS_和值 = 624, JSKS_大小 = 625, JSKS_单双 = 626,
			JSKS_猜一个号 = 627,

			//时时彩
			HSSEC_五星直选复式 = 628, HSSEC_五星直选单式 = 629, HSSEC_后四星直选复式 = 630, HSSEC_后四星直选单式 = 631,
			HSSEC_前四星直选复式 = 632, HSSEC_前四星直选单式 = 633,
			HSSEC_前三直选复式 = 634, HSSEC_前三直选单式 = 635, HSSEC_中三直选复式 = 636, HSSEC_中三直选单式 = 637,
			HSSEC_后三直选复式 = 638, HSSEC_后三直选单式 = 639, HSSEC_前三组六复式 = 640, HSSEC_前三组三复式 = 641, HSSEC_中三组六复式 = 642,
			HSSEC_中三组三复式 = 643, HSSEC_后三组六复式 = 644, HSSEC_后三组三复式 = 645,
			HSSEC_前二直选复式 = 646, HSSEC_前二直选单式 = 647,
			HSSEC_后二直选复式 = 648, HSSEC_后二直选单式 = 649, HSSEC_前二组选复式 = 650,
			HSSEC_后二组选复式 = 651,
			HSSEC_前三不定胆 = 652, HSSEC_中三不定胆 = 653, HSSEC_后三不定胆 = 654, HSSEC_五星不定胆 = 655, HSSEC_五星定位胆 = 656,
			HSSEC_任选三复式 = 657, HSSEC_任选三单式 = 658, HSSEC_任选二复式 = 659, HSSEC_任选二单式 = 660, HSSEC_任选四复式 = 661,
			HSSEC_任选四单式 = 662, HSSEC_前三混合单式 = 663,
			HSSEC_中三混合单式 = 664, HSSEC_后三混合单式 = 665, HSSEC_前二大小单双 = 666, HSSEC_后二大小单双 = 667, HSSEC_前三大小单双 = 668,
			HSSEC_后三大小单双 = 669,


			HJ_PKHSSEC_前一直选复式 = 671, HJ_PKHSSEC_前二直选复式 = 672, HJ_PKHSSEC_前二直选单式 = 673, HJ_PKHSSEC_前三直选复式 = 674,
			HJ_PKHSSEC_前三直选单式 = 675, HJ_PKHSSEC_定位胆 = 676,
			//HJ_PKHSSEC_6到10定位胆 = 697,
			HJ_PKHSSEC_前四直选复式 = 677,
			HJ_PKHSSEC_前四直选单式 = 678,
			HJ_PKHSSEC_前五直选复式 = 679, HJ_PKHSSEC_前五直选单式 = 680, HJ_PKHSSEC_龙虎斗_1Vs10 = 681, HJ_PKHSSEC_龙虎斗_2Vs9 = 682,
			HJ_PKHSSEC_龙虎斗_3Vs8 = 683, HJ_PKHSSEC_龙虎斗_4Vs7 = 684, HJ_PKHSSEC_龙虎斗_5Vs6 = 685,
			HJ_PKHSSEC_大小_冠军 = 686, HJ_PKHSSEC_大小_亚军 = 687, HJ_PKHSSEC_大小_季军 = 688, HJ_PKHSSEC_大小_第四名 = 689, HJ_PKHSSEC_大小_第五名 = 690,
			HJ_PKHSSEC_单双_冠军 = 691, HJ_PKHSSEC_单双_亚军 = 692, HJ_PKHSSEC_单双_季军 = 693, HJ_PKHSSEC_单双_第四名 = 694, HJ_PKHSSEC_单双_第五名 = 695,
			HJ_PKHSSEC_冠亚和值 = 696, HJ_PKHSSEC_前六直选复式 = 787, HJ_PKHSSEC_前六直选单式 = 788,


			TAIJIN_五星直选复式 = 698, TAIJIN_五星直选单式 = 699, TAIJIN_后四星直选复式 = 700, TAIJIN_后四星直选单式 = 701,
			TAIJIN_前四星直选复式 = 702, TAIJIN_前四星直选单式 = 703,
			TAIJIN_前三直选复式 = 704, TAIJIN_前三直选单式 = 705, TAIJIN_中三直选复式 = 706, TAIJIN_中三直选单式 = 707,
			TAIJIN_后三直选复式 = 708, TAIJIN_后三直选单式 = 709, TAIJIN_前三组六复式 = 710, TAIJIN_前三组三复式 = 711,
			TAIJIN_中三组六复式 = 712, TAIJIN_中三组三复式 = 713, TAIJIN_后三组六复式 = 714, TAIJIN_后三组三复式 = 715,
			TAIJIN_前二直选复式 = 716, TAIJIN_前二直选单式 = 717, TAIJIN_后二直选复式 = 718, TAIJIN_后二直选单式 = 719,
			TAIJIN_前二组选复式 = 720, TAIJIN_后二组选复式 = 721, TAIJIN_前三不定胆 = 722, TAIJIN_中三不定胆 = 723,
			TAIJIN_后三不定胆 = 724, TAIJIN_五星不定胆 = 725, TAIJIN_五星定位胆 = 726,
			TAIJIN_任选三复式 = 727, TAIJIN_任选三单式 = 728, TAIJIN_任选二复式 = 729, TAIJIN_任选二单式 = 730,
			TAIJIN_前三混合单式 = 733, TAIJIN_中三混合单式 = 734, TAIJIN_后三混合单式 = 735,
			TAIJIN_任选四复式 = 731, TAIJIN_任选四单式 = 732, TAIJIN_前二大小单双 = 736, TAIJIN_后二大小单双 = 737, TAIJIN_前三大小单双 = 738,
			TAIJIN_后三大小单双 = 739,

			TWBINGGO_五星直选复式 = 741, TWBINGGO_五星直选单式 = 742, TWBINGGO_后四星直选复式 = 743, TWBINGGO_后四星直选单式 = 744,
			TWBINGGO_前四星直选复式 = 745, TWBINGGO_前四星直选单式 = 746,
			TWBINGGO_前三直选复式 = 747, TWBINGGO_前三直选单式 = 748, TWBINGGO_中三直选复式 = 749, TWBINGGO_中三直选单式 = 750,
			TWBINGGO_后三直选复式 = 751, TWBINGGO_后三直选单式 = 752, TWBINGGO_前三组六复式 = 753, TWBINGGO_前三组三复式 = 754,
			TWBINGGO_中三组六复式 = 755, TWBINGGO_中三组三复式 = 756, TWBINGGO_后三组六复式 = 757, TWBINGGO_后三组三复式 = 758,
			TWBINGGO_前二直选复式 = 759, TWBINGGO_前二直选单式 = 760, TWBINGGO_后二直选复式 = 761, TWBINGGO_后二直选单式 = 762,
			TWBINGGO_前二组选复式 = 763, TWBINGGO_后二组选复式 = 764, TWBINGGO_前三不定胆 = 765, TWBINGGO_中三不定胆 = 766,
			TWBINGGO_后三不定胆 = 767, TWBINGGO_五星不定胆 = 768, TWBINGGO_五星定位胆 = 769,
			TWBINGGO_任选三复式 = 770, TWBINGGO_任选三单式 = 771, TWBINGGO_任选二复式 = 772, TWBINGGO_任选二单式 = 773,
			TWBINGGO_任选四复式 = 774, TWBINGGO_任选四单式 = 775,
			TWBINGGO_前三混合单式 = 776, TWBINGGO_中三混合单式 = 777, TWBINGGO_后三混合单式 = 778,
			TWBINGGO_前二大小单双 = 779, TWBINGGO_后二大小单双 = 780, TWBINGGO_前三大小单双 = 781,
			TWBINGGO_后三大小单双 = 782,

			BJ8_五星直选复式 = 813, BJ8_五星直选单式 = 814, BJ8_后四星直选复式 = 815, BJ8_后四星直选单式 = 816,
			BJ8_前四星直选复式 = 817, BJ8_前四星直选单式 = 818,
			BJ8_前三直选复式 = 819, BJ8_前三直选单式 = 820, BJ8_中三直选复式 = 821, BJ8_中三直选单式 = 822,
			BJ8_后三直选复式 = 823, BJ8_后三直选单式 = 824, BJ8_前三组六复式 = 825, BJ8_前三组三复式 = 826,
			BJ8_中三组六复式 = 827, BJ8_中三组三复式 = 828, BJ8_后三组六复式 = 829, BJ8_后三组三复式 = 830,
			BJ8_前二直选复式 = 831, BJ8_前二直选单式 = 832, BJ8_后二直选复式 = 833, BJ8_后二直选单式 = 834,
			BJ8_前二组选复式 = 835, BJ8_后二组选复式 = 836, BJ8_前三不定胆 = 837, BJ8_中三不定胆 = 838,
			BJ8_后三不定胆 = 839, BJ8_五星不定胆 = 840, BJ8_五星定位胆 = 841,
			BJ8_任选三复式 = 842, BJ8_任选三单式 = 843, BJ8_任选二复式 = 844, BJ8_任选二单式 = 845,
			BJ8_任选四复式 = 846, BJ8_任选四单式 = 847,
			BJ8_前三混合单式 = 848, BJ8_中三混合单式 = 849, BJ8_后三混合单式 = 850,
			BJ8_前二大小单双 = 851, BJ8_后二大小单双 = 852, BJ8_前三大小单双 = 853,
			BJ8_后三大小单双 = 854,

			KOR5_五星直选复式 = 855, KOR5_五星直选单式 = 856, KOR5_后四星直选复式 = 857, KOR5_后四星直选单式 = 858,
			KOR5_前四星直选复式 = 859, KOR5_前四星直选单式 = 860,
			KOR5_前三直选复式 = 861, KOR5_前三直选单式 = 862, KOR5_中三直选复式 = 863, KOR5_中三直选单式 = 864,
			KOR5_后三直选复式 = 865, KOR5_后三直选单式 = 866, KOR5_前三组六复式 = 867, KOR5_前三组三复式 = 868,
			KOR5_中三组六复式 = 869, KOR5_中三组三复式 = 870, KOR5_后三组六复式 = 871, KOR5_后三组三复式 = 872,
			KOR5_前二直选复式 = 873, KOR5_前二直选单式 = 874, KOR5_后二直选复式 = 875, KOR5_后二直选单式 = 876,
			KOR5_前二组选复式 = 877, KOR5_后二组选复式 = 878, KOR5_前三不定胆 = 879, KOR5_中三不定胆 = 880,
			KOR5_后三不定胆 = 881, KOR5_五星不定胆 = 882, KOR5_五星定位胆 = 883,
			KOR5_任选三复式 = 884, KOR5_任选三单式 = 885, KOR5_任选二复式 = 886, KOR5_任选二单式 = 887,
			KOR5_任选四复式 = 888, KOR5_任选四单式 = 889,
			KOR5_前三混合单式 = 890, KOR5_中三混合单式 = 891, KOR5_后三混合单式 = 892,
			KOR5_前二大小单双 = 893, KOR5_后二大小单双 = 894, KOR5_前三大小单双 = 895,
			KOR5_后三大小单双 = 896,

            //法国快三
            FRKS_三连号通选 = 2786, FRKS_三不同标准选号 = 2787, FRKS_三不同标准选号单式 = 2788, FRKS_三不同胆拖选号 = 2789, FRKS_三不同和值 = 2790,
            FRKS_三同号单选 = 2791, FRKS_三同号通选 = 2792, FRKS_二同号单选 = 2793, FRKS_二同号单式 = 2794, FRKS_二同号复选 = 2795,
            FRKS_二不同号选号 = 2796, FRKS_二不同号单式 = 2797, FRKS_二不同胆拖选号 = 2798, FRKS_和值 = 2799, FRKS_三位大小 = 2800, FRKS_三位单双 = 2801,
            FRKS_猜一个号 = 2802,

            // LuckyAirship
            LKAPK_前一直选复式 = 2803, LKAPK_前二直选复式 = 2804, LKAPK_前二直选单式 = 2805, LKAPK_前三直选复式 = 2806, LKAPK_前三直选单式 = 2807,
            LKAPK_定位胆 = 2808, LKAPK_前四直选复式 = 2809, LKAPK_前四直选单式 = 2810, LKAPK_前五直选复式 = 2811, LKAPK_前五直选单式 = 2812, LKAPK_龙虎斗_1Vs10 = 2813,
            LKAPK_龙虎斗_2Vs9 = 2814, LKAPK_龙虎斗_3Vs8 = 2815, LKAPK_龙虎斗_4Vs7 = 2816, LKAPK_龙虎斗_5Vs6 = 2817, LKAPK_大小_冠军 = 2818, LKAPK_大小_亚军 = 2819,
            LKAPK_大小_季军 = 2820, LKAPK_大小_第四名 = 2821, LKAPK_大小_第五名 = 2822, LKAPK_单双_冠军 = 2823, LKAPK_单双_亚军 = 2824, LKAPK_单双_季军 = 2825,
            LKAPK_单双_第四名 = 2826, LKAPK_单双_第五名 = 2827, LKAPK_冠亚和值 = 2828, LKAPK_前六直选复式 = 2829, LKAPK_前六直选单式 = 2830,

            //-----SSC_QQ5-----
            QQ5_前三直选复式 = 2860, QQ5_前三直选单式 = 2861, QQ5_中三直选复式 = 2862, QQ5_中三直选单式 = 2863, QQ5_后三直选复式 = 2864, QQ5_后三直选单式 = 2865, QQ5_前三组六复式 = 2866,
            QQ5_前三组三复式 = 2867, QQ5_中三组六复式 = 2868, QQ5_中三组三复式 = 2869, QQ5_前二直选复式 = 2870, QQ5_前二直选单式 = 2871, QQ5_后二直选复式 = 2872, QQ5_后二直选单式 = 2873,
            QQ5_前二组选复式 = 2874, QQ5_后二组选复式 = 2875, QQ5_前三不定胆 = 2876, QQ5_中三不定胆 = 2877, QQ5_后三不定胆 = 2878, QQ5_五星不定胆 = 2879, QQ5_五星定位胆 = 2880, QQ5_任选三复式 = 2881,
            QQ5_任选三单式 = 2882, QQ5_任选二复式 = 2883, QQ5_任选二单式 = 2884, QQ5_后三组六复式 = 2885, QQ5_后三组三复式 = 2886, QQ5_前三混合单式 = 2887, QQ5_中三混合单式 = 2888,
            QQ5_后三混合单式 = 2889, QQ5_直选复式 = 2890, QQ5_直选单式 = 2891, QQ5_后四直选复式 = 2892, QQ5_后四直选单式 = 2893, QQ5_前四直选复式 = 2894, QQ5_前四直选单式 = 2895, QQ5_任选四复式 = 2896,
            QQ5_任选四单式 = 2897, QQ5_前二大小单双 = 2898, QQ5_后二大小单双 = 2899, QQ5_前三大小单双 = 2900, QQ5_后三大小单双 = 2901,

            // 威尼斯飞艇
            VNSPK_前一直选复式 = 3016, VNSPK_前二直选复式 = 3017, VNSPK_前二直选单式 = 3018, VNSPK_前三直选复式 = 3019, VNSPK_前三直选单式 = 3020,
            VNSPK_定位胆 = 3021, VNSPK_前四直选复式 = 3022, VNSPK_前四直选单式 = 3023, VNSPK_前五直选复式 = 3024, VNSPK_前五直选单式 = 3025, VNSPK_龙虎斗_1Vs10 = 3026,
            VNSPK_龙虎斗_2Vs9 = 3027, VNSPK_龙虎斗_3Vs8 = 3028, VNSPK_龙虎斗_4Vs7 = 3029, VNSPK_龙虎斗_5Vs6 = 3030, VNSPK_大小_冠军 = 3031, VNSPK_大小_亚军 = 3032,
            VNSPK_大小_季军 = 3033, VNSPK_大小_第四名 = 3034, VNSPK_大小_第五名 = 3035, VNSPK_单双_冠军 = 3036, VNSPK_单双_亚军 = 3037, VNSPK_单双_季军 = 3038,
            VNSPK_单双_第四名 = 3039, VNSPK_单双_第五名 = 3040, VNSPK_冠亚和值 = 3041, VNSPK_前六直选复式 = 3042, VNSPK_前六直选单式 = 3043,

            // 腾讯赛车分分彩
            QQRCPK10_前一直选复式 = 3073, QQRCPK10_前二直选复式 = 3074, QQRCPK10_前二直选单式 = 3075, QQRCPK10_前三直选复式 = 3076, QQRCPK10_前三直选单式 = 3077,
            QQRCPK10_定位胆 = 3078, QQRCPK10_前四直选复式 = 3079, QQRCPK10_前四直选单式 = 3080, QQRCPK10_前五直选复式 = 3081, QQRCPK10_前五直选单式 = 3082, QQRCPK10_龙虎斗_1Vs10 = 3083,
            QQRCPK10_龙虎斗_2Vs9 = 3084, QQRCPK10_龙虎斗_3Vs8 = 3085, QQRCPK10_龙虎斗_4Vs7 = 3086, QQRCPK10_龙虎斗_5Vs6 = 3087, QQRCPK10_大小_冠军 = 3088, QQRCPK10_大小_亚军 = 3089,
            QQRCPK10_大小_季军 = 3090, QQRCPK10_大小_第四名 = 3091, QQRCPK10_大小_第五名 = 3092, QQRCPK10_单双_冠军 = 3093, QQRCPK10_单双_亚军 = 3094, QQRCPK10_单双_季军 = 3095,
            QQRCPK10_单双_第四名 = 3096, QQRCPK10_单双_第五名 = 3097, QQRCPK10_冠亚和值 = 3098, QQRCPK10_前六直选复式 = 3099, QQRCPK10_前六直选单式 = 3100,

            // 腾讯赛车5分彩
            QQRC5PK10_前一直选复式 = 3130, QQRC5PK10_前二直选复式 = 3131, QQRC5PK10_前二直选单式 = 3132, QQRC5PK10_前三直选复式 = 3133, QQRC5PK10_前三直选单式 = 3134,
            QQRC5PK10_定位胆 = 3135, QQRC5PK10_前四直选复式 = 3136, QQRC5PK10_前四直选单式 = 3137, QQRC5PK10_前五直选复式 = 3138, QQRC5PK10_前五直选单式 = 3139, QQRC5PK10_龙虎斗_1Vs10 = 3140,
            QQRC5PK10_龙虎斗_2Vs9 = 3141, QQRC5PK10_龙虎斗_3Vs8 = 3142, QQRC5PK10_龙虎斗_4Vs7 = 3143, QQRC5PK10_龙虎斗_5Vs6 = 3144, QQRC5PK10_大小_冠军 = 3145, QQRC5PK10_大小_亚军 = 3146,
            QQRC5PK10_大小_季军 = 3147, QQRC5PK10_大小_第四名 = 3148, QQRC5PK10_大小_第五名 = 3149, QQRC5PK10_单双_冠军 = 3150, QQRC5PK10_单双_亚军 = 3151, QQRC5PK10_单双_季军 = 3152,
            QQRC5PK10_单双_第四名 = 3153, QQRC5PK10_单双_第五名 = 3154, QQRC5PK10_冠亚和值 = 3155, QQRC5PK10_前六直选复式 = 3156, QQRC5PK10_前六直选单式 = 3157,

            // QQ分分彩
            MQQ_前三直选复式 = 3187,
            MQQ_前三直选单式 = 3188,
            MQQ_中三直选复式 = 3189,
            MQQ_中三直选单式 = 3190,
            MQQ_后三直选复式 = 3191,
            MQQ_后三直选单式 = 3192,
            MQQ_前三组六复式 = 3193,
            MQQ_前三组三复式 = 3194,
            MQQ_中三组六复式 = 3195,
            MQQ_中三组三复式 = 3196,
            MQQ_前二直选复式 = 3197,
            MQQ_前二直选单式 = 3198,
            MQQ_后二直选复式 = 3199,
            MQQ_后二直选单式 = 3200,
            MQQ_前二组选复式 = 3201,
            MQQ_后二组选复式 = 3202,
            MQQ_前三不定胆 = 3203,
            MQQ_中三不定胆 = 3204,
            MQQ_后三不定胆 = 3205,
            MQQ_五星不定胆 = 3206,
            MQQ_五星定位胆 = 3207,
            MQQ_任选三复式 = 3208,
            MQQ_任选三单式 = 3209,
            MQQ_任选二复式 = 3210,
            MQQ_任选二单式 = 3211,
            MQQ_后三组六复式 = 3212,
            MQQ_后三组三复式 = 3213,
            MQQ_前三混合单式 = 3214,
            MQQ_中三混合单式 = 3215,
            MQQ_后三混合单式 = 3216,
            MQQ_直选复式 = 3217,
            MQQ_直选单式 = 3218,
            MQQ_后四直选复式 = 3219,
            MQQ_后四直选单式 = 3220,
            MQQ_前四直选复式 = 3221,
            MQQ_前四直选单式 = 3222,
            MQQ_任选四复式 = 3223,
            MQQ_任选四单式 = 3224,
            MQQ_前二大小单双 = 3225,
            MQQ_后二大小单双 = 3226,
            MQQ_前三大小单双 = 3227,
            MQQ_后三大小单双 = 3228,

            // QQ5分彩           
            MQQ5_前三直选复式 = 3343,
            MQQ5_前三直选单式 = 3344,
            MQQ5_中三直选复式 = 3345,
            MQQ5_中三直选单式 = 3346,
            MQQ5_后三直选复式 = 3347,
            MQQ5_后三直选单式 = 3348,
            MQQ5_前三组六复式 = 3349,
            MQQ5_前三组三复式 = 3350,
            MQQ5_中三组六复式 = 3351,
            MQQ5_中三组三复式 = 3352,
            MQQ5_前二直选复式 = 3353,
            MQQ5_前二直选单式 = 3354,
            MQQ5_后二直选复式 = 3355,
            MQQ5_后二直选单式 = 3356,
            MQQ5_前二组选复式 = 3357,
            MQQ5_后二组选复式 = 3358,
            MQQ5_前三不定胆 = 3359,
            MQQ5_中三不定胆 = 3360,
            MQQ5_后三不定胆 = 3361,
            MQQ5_五星不定胆 = 3362,
            MQQ5_五星定位胆 = 3363,
            MQQ5_任选三复式 = 3364,
            MQQ5_任选三单式 = 3365,
            MQQ5_任选二复式 = 3366,
            MQQ5_任选二单式 = 3367,
            MQQ5_后三组六复式 = 3368,
            MQQ5_后三组三复式 = 3369,
            MQQ5_前三混合单式 = 3370,
            MQQ5_中三混合单式 = 3371,
            MQQ5_后三混合单式 = 3372,
            MQQ5_直选复式 = 3373,
            MQQ5_直选单式 = 3374,
            MQQ5_后四直选复式 = 3375,
            MQQ5_后四直选单式 = 3376,
            MQQ5_前四直选复式 = 3377,
            MQQ5_前四直选单式 = 3378,
            MQQ5_任选四复式 = 3379,
            MQQ5_任选四单式 = 3380,
            MQQ5_前二大小单双 = 3381,
            MQQ5_后二大小单双 = 3382,
            MQQ5_前三大小单双 = 3383,
            MQQ5_后三大小单双 = 3384,
            
            // 鳳凰騰訊分分彩
            FHQQ_前三直选复式 = 3499,
            FHQQ_前三直选单式 = 3500,
            FHQQ_中三直选复式 = 3501,
            FHQQ_中三直选单式 = 3502,
            FHQQ_后三直选复式 = 3503,
            FHQQ_后三直选单式 = 3504,
            FHQQ_前三组六复式 = 3505,
            FHQQ_前三组三复式 = 3506,
            FHQQ_中三组六复式 = 3507,
            FHQQ_中三组三复式 = 3508,
            FHQQ_前二直选复式 = 3509,
            FHQQ_前二直选单式 = 3510,
            FHQQ_后二直选复式 = 3511,
            FHQQ_后二直选单式 = 3512,
            FHQQ_前二组选复式 = 3513,
            FHQQ_后二组选复式 = 3514,
            FHQQ_前三不定胆 = 3515,
            FHQQ_中三不定胆 = 3516,
            FHQQ_后三不定胆 = 3517,
            FHQQ_五星不定胆 = 3518,
            FHQQ_五星定位胆 = 3519,
            FHQQ_任选三复式 = 3520,
            FHQQ_任选三单式 = 3521,
            FHQQ_任选二复式 = 3522,
            FHQQ_任选二单式 = 3523,
            FHQQ_后三组六复式 = 3524,
            FHQQ_后三组三复式 = 3525,
            FHQQ_前三混合单式 = 3526,
            FHQQ_中三混合单式 = 3527,
            FHQQ_后三混合单式 = 3528,
            FHQQ_直选复式 = 3529,
            FHQQ_直选单式 = 3530,
            FHQQ_后四直选复式 = 3531,
            FHQQ_后四直选单式 = 3532,
            FHQQ_前四直选复式 = 3533,
            FHQQ_前四直选单式 = 3534,
            FHQQ_任选四复式 = 3535,
            FHQQ_任选四单式 = 3536,
            FHQQ_前二大小单双 = 3537,
            FHQQ_后二大小单双 = 3538,
            FHQQ_前三大小单双 = 3539,
            FHQQ_后三大小单双 = 3540,
            
            // 微信分分彩 usertype = 1的playtyperadioid
            WeixinQQ_前三直选复式 = 3655, WeixinQQ_前三直选单式 = 3656, WeixinQQ_中三直选复式 = 3657, WeixinQQ_中三直选单式 = 3658,
            WeixinQQ_后三直选复式 = 3659, WeixinQQ_后三直选单式 = 3660, WeixinQQ_前三组六复式 = 3661, WeixinQQ_前三组三复式 = 3662,
            WeixinQQ_中三组六复式 = 3663, WeixinQQ_中三组三复式 = 3664, WeixinQQ_前二直选复式 = 3665, WeixinQQ_前二直选单式 = 3666,
            WeixinQQ_后二直选复式 = 3667, WeixinQQ_后二直选单式 = 3668, WeixinQQ_前二组选复式 = 3669, WeixinQQ_后二组选复式 = 3670,
            WeixinQQ_前三不定胆 = 3671, WeixinQQ_中三不定胆 = 3672, WeixinQQ_后三不定胆 = 3673, WeixinQQ_五星不定胆 = 3674,
            WeixinQQ_五星定位胆 = 3675, WeixinQQ_任选三复式 = 3676, WeixinQQ_任选三单式 = 3677, WeixinQQ_任选二复式 = 3678,
            WeixinQQ_任选二单式 = 3679, WeixinQQ_后三组六复式 = 3680, WeixinQQ_后三组三复式 = 3681, WeixinQQ_前三混合单式 = 3682,
            WeixinQQ_中三混合单式 = 3683, WeixinQQ_后三混合单式 = 3684, WeixinQQ_直选复式 = 3685, WeixinQQ_直选单式 = 3686,
            WeixinQQ_后四星直选复式 = 3687, WeixinQQ_后四星直选单式 = 3688, WeixinQQ_前四星直选复式 = 3689, WeixinQQ_前四星直选单式 = 3690,
            WeixinQQ_任选四复式 = 3691, WeixinQQ_任选四单式 = 3692, WeixinQQ_前二大小单双 = 3693, WeixinQQ_后二大小单双 = 3694,
            WeixinQQ_前三大小单双 = 3695, WeixinQQ_后三大小单双 = 3696,
            
            // 江西11選5
            Jx11x5_前三直选复式 = 3811, Jx11x5_前三直选单式 = 3812, Jx11x5_前三组选 = 3813, Jx11x5_前二直选复式 = 3814,
            Jx11x5_前二直选单式 = 3815, Jx11x5_后二直选复式 = 3816, Jx11x5_后二直选单式 = 3817, Jx11x5_前二组选 = 3818,
            Jx11x5_后二组选 = 3819, Jx11x5_前三不定胆 = 3820, Jx11x5_定位胆 = 3821, Jx11x5Rx_一中一 = 3822,
            Jx11x5Rx_二中二 = 3823, Jx11x5Rx_三中三 = 3824, Jx11x5Rx_四中四 = 3825, Jx11x5Rx_五中五 = 3826,
            Jx11x5Rx_六中五 = 3827, Jx11x5Rx_七中五 = 3828, Jx11x5Rx_八中五 = 3829, Jx11x5d_二中二 = 3830,
            Jx11x5d_三中三 = 3831, Jx11x5d_四中四 = 3832, Jx11x5d_五中五 = 3833, Jx11x5d_六中五 = 3834,
            Jx11x5d_七中五 = 3835, Jx11x5d_八中五 = 3836, Jx11x5Ds_一中一 = 3837, Jx11x5Ds_二中二 = 3838,
            Jx11x5Ds_三中三 = 3839, Jx11x5Ds_四中四 = 3840, Jx11x5Ds_五中五 = 3841, Jx11x5Ds_六中五 = 3842,
            Jx11x5Ds_七中五 = 3843, Jx11x5Ds_八中五 = 3844,

            // 江苏11選5
            Js11x5_前三直选复式 = 3882, Js11x5_前三直选单式 = 3883, Js11x5_前三组选 = 3884, Js11x5_前二直选复式 = 3885,
            Js11x5_前二直选单式 = 3886, Js11x5_后二直选复式 = 3887, Js11x5_后二直选单式 = 3888, Js11x5_前二组选 = 3889,
            Js11x5_后二组选 = 3890, Js11x5_前三不定胆 = 3891, Js11x5_定位胆 = 3892, Js11x5Rx_一中一 = 3893,
            Js11x5Rx_二中二 = 3894, Js11x5Rx_三中三 = 3895, Js11x5Rx_四中四 = 3896, Js11x5Rx_五中五 = 3897,
            Js11x5Rx_六中五 = 3898, Js11x5Rx_七中五 = 3899, Js11x5Rx_八中五 = 3900, Js11x5d_二中二 = 3901,
            Js11x5d_三中三 = 3902, Js11x5d_四中四 = 3903, Js11x5d_五中五 = 3904, Js11x5d_六中五 = 3905,
            Js11x5d_七中五 = 3906, Js11x5d_八中五 = 3907, Js11x5Ds_一中一 = 3908, Js11x5Ds_二中二 = 3909,
            Js11x5Ds_三中三 = 3910, Js11x5Ds_四中四 = 3911, Js11x5Ds_五中五 = 3912, Js11x5Ds_六中五 = 3913,
            Js11x5Ds_七中五 = 3914, Js11x5Ds_八中五 = 3915,

            // 河內分分彩 usertype = 1 的 playtyperadioid
            VietSSC_前三直选复式 = 3953, VietSSC_前三直选单式 = 3954, VietSSC_中三直选复式 = 3955, VietSSC_中三直选单式 = 3956,
            VietSSC_后三直选复式 = 3957, VietSSC_后三直选单式 = 3958, VietSSC_前三组六复式 = 3959, VietSSC_前三组三复式 = 3960,
            VietSSC_中三组六复式 = 3961, VietSSC_中三组三复式 = 3962, VietSSC_前二直选复式 = 3963, VietSSC_前二直选单式 = 3964,
            VietSSC_后二直选复式 = 3965, VietSSC_后二直选单式 = 3966, VietSSC_前二组选复式 = 3967, VietSSC_后二组选复式 = 3968,
            VietSSC_前三不定胆 = 3969, VietSSC_中三不定胆 = 3970, VietSSC_后三不定胆 = 3971, VietSSC_五星不定胆 = 3972,
            VietSSC_五星定位胆 = 3973, VietSSC_任选三复式 = 3974, VietSSC_任选三单式 = 3975, VietSSC_任选二复式 = 3976,
            VietSSC_任选二单式 = 3977, VietSSC_后三组六复式 = 3978, VietSSC_后三组三复式 = 3979, VietSSC_前三混合单式 = 3980,
            VietSSC_中三混合单式 = 3981, VietSSC_后三混合单式 = 3982, VietSSC_直选复式 = 3983, VietSSC_直选单式 = 3984,
            VietSSC_后四星直选复式 = 3985, VietSSC_后四星直选单式 = 3986, VietSSC_前四星直选复式 = 3987, VietSSC_前四星直选单式 = 3988,
            VietSSC_任选四复式 = 3989, VietSSC_任选四单式 = 3990, VietSSC_前二大小单双 = 3991, VietSSC_后二大小单双 = 3992,
            VietSSC_前三大小单双 = 3993, VietSSC_后三大小单双 = 3994,

            // 奇趣腾讯分分彩 usertype = 1的playtyperadioid
            ChichuQQ_前三直选复式 = 4327, ChichuQQ_前三直选单式 = 4328, ChichuQQ_中三直选复式 = 4329, ChichuQQ_中三直选单式 = 4330,
            ChichuQQ_后三直选复式 = 4331, ChichuQQ_后三直选单式 = 4332, ChichuQQ_前三组六复式 = 4333, ChichuQQ_前三组三复式 = 4334,
            ChichuQQ_中三组六复式 = 4335, ChichuQQ_中三组三复式 = 4336, ChichuQQ_前二直选复式 = 4337, ChichuQQ_前二直选单式 = 4338,
            ChichuQQ_后二直选复式 = 4339, ChichuQQ_后二直选单式 = 4340, ChichuQQ_前二组选复式 = 4341, ChichuQQ_后二组选复式 = 4342,
            ChichuQQ_前三不定胆 = 4343, ChichuQQ_中三不定胆 = 4344, ChichuQQ_后三不定胆 = 4345, ChichuQQ_五星不定胆 = 4346,
            ChichuQQ_五星定位胆 = 4347, ChichuQQ_任选三复式 = 4348, ChichuQQ_任选三单式 = 4349, ChichuQQ_任选二复式 = 4350,
            ChichuQQ_任选二单式 = 4351, ChichuQQ_后三组六复式 = 4352, ChichuQQ_后三组三复式 = 4353, ChichuQQ_前三混合单式 = 4354,
            ChichuQQ_中三混合单式 = 4355, ChichuQQ_后三混合单式 = 4356, ChichuQQ_直选复式 = 4357, ChichuQQ_直选单式 = 4358,
            ChichuQQ_后四星直选复式 = 4359, ChichuQQ_后四星直选单式 = 4360, ChichuQQ_前四星直选复式 = 4361, ChichuQQ_前四星直选单式 = 4362,
            ChichuQQ_任选四复式 = 4363, ChichuQQ_任选四单式 = 4364, ChichuQQ_前二大小单双 = 4365, ChichuQQ_后二大小单双 = 4366,
            ChichuQQ_前三大小单双 = 4367, ChichuQQ_后三大小单双 = 4368,
        }
		#endregion
	}
}