using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class LotteryRecommendTitleSettings : BaseIntValueModel<LotteryRecommendTitleSettings>
    {
        private LotteryRecommendTitleSettings() { }

        public string Title { get; set; }
        public string SubTitle { get; set; }

        public static LotteryRecommendTitleSettings ChongQing = new LotteryRecommendTitleSettings() { Value = 1, Title = "重庆", SubTitle = "时时彩" };
        public static LotteryRecommendTitleSettings JiangXi = new LotteryRecommendTitleSettings() { Value = 2, Title = "江西", SubTitle = "时时彩" };
        public static LotteryRecommendTitleSettings Welfare3D = new LotteryRecommendTitleSettings() { Value = 3, Title = "福彩", SubTitle = "3D" };
        public static LotteryRecommendTitleSettings Sports = new LotteryRecommendTitleSettings() { Value = 4, Title = "体彩", SubTitle = "排列三" };
        public static LotteryRecommendTitleSettings GDSelectFive = new LotteryRecommendTitleSettings() { Value = 6, Title = "广东", SubTitle = "十一选五" };
        public static LotteryRecommendTitleSettings CQSelectFive = new LotteryRecommendTitleSettings() { Value = 9, Title = "重庆", SubTitle = "十一选五" };
        public static LotteryRecommendTitleSettings SDSelectFive = new LotteryRecommendTitleSettings() { Value = 11, Title = "山东", SubTitle = "十一选五" };
        public static LotteryRecommendTitleSettings BeijingPK10 = new LotteryRecommendTitleSettings() { Value = 12, Title = "北京", SubTitle = "PK拾" };
        public static LotteryRecommendTitleSettings Heji = new LotteryRecommendTitleSettings() { Value = 13, Title = GlobalVariables.BrandCode, SubTitle = "时时彩" };
        public static LotteryRecommendTitleSettings HjSelectFive = new LotteryRecommendTitleSettings() { Value = 14, Title = GlobalVariables.BrandCode, SubTitle = "11选5" };
        public static LotteryRecommendTitleSettings Xinjiang = new LotteryRecommendTitleSettings() { Value = 15, Title = "新疆", SubTitle = "时时彩" };
        public static LotteryRecommendTitleSettings HSSFRealtime = new LotteryRecommendTitleSettings() { Value = 16, Title = GlobalVariables.BrandCode, SubTitle = "3分彩" };
        public static LotteryRecommendTitleSettings HSPK10 = new LotteryRecommendTitleSettings() { Value = 17, Title = GlobalVariables.BrandCode, SubTitle = "PK拾" };
        public static LotteryRecommendTitleSettings JiangSuKuaiSan = new LotteryRecommendTitleSettings() { Value = 18, Title = "江苏", SubTitle = "快三" };
        public static LotteryRecommendTitleSettings HSMMCRealtime = new LotteryRecommendTitleSettings() { Value = 19, Title = GlobalVariables.BrandCode, SubTitle = "秒秒彩" };
        public static LotteryRecommendTitleSettings HSMMCPK10 = new LotteryRecommendTitleSettings() { Value = 20, Title = "PK拾", SubTitle = "秒秒彩" };
        public static LotteryRecommendTitleSettings Tianjin = new LotteryRecommendTitleSettings() { Value = 21, Title = "天津", SubTitle = "时时彩" };
        public static LotteryRecommendTitleSettings BingGo = new LotteryRecommendTitleSettings() { Value = 23, Title = "台湾", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings Keno = new LotteryRecommendTitleSettings() { Value = 24, Title = "北京", SubTitle = "快乐8" };
        public static LotteryRecommendTitleSettings Korea5 = new LotteryRecommendTitleSettings() { Value = 25, Title = "韩国", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings DePK10 = new LotteryRecommendTitleSettings() { Value = 26, Title = "德国", SubTitle = "PK拾" };
        public static LotteryRecommendTitleSettings QQ = new LotteryRecommendTitleSettings() { Value = 27, Title = "腾讯", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings ItalyPK10 = new LotteryRecommendTitleSettings() { Value = 28, Title = "意大利", SubTitle = "PK拾" };
        public static LotteryRecommendTitleSettings ItalySSC = new LotteryRecommendTitleSettings() { Value = 29, Title = "意大利", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings HLJSSC = new LotteryRecommendTitleSettings() { Value = 30, Title = "黑龙江", SubTitle = "时时彩" };
        public static LotteryRecommendTitleSettings FRKS = new LotteryRecommendTitleSettings() { Value = 31, Title = "法国", SubTitle = "快三" };
        public static LotteryRecommendTitleSettings LKAPK10 = new LotteryRecommendTitleSettings() { Value = 32, Title = "幸运", SubTitle = "飞艇" };
        public static LotteryRecommendTitleSettings QQ5 = new LotteryRecommendTitleSettings() { Value = 33, Title = "腾讯", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings VNSPK10 = new LotteryRecommendTitleSettings() { Value = 34, Title = "威尼斯", SubTitle = "飞艇" };
        public static LotteryRecommendTitleSettings QQRCPK10 = new LotteryRecommendTitleSettings() { Value = 35, Title = "腾讯赛车", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings QQRC5PK10 = new LotteryRecommendTitleSettings() { Value = 36, Title = "腾讯赛车", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings MQQ = new LotteryRecommendTitleSettings() { Value = 37, Title = "QQ", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings MQQ5 = new LotteryRecommendTitleSettings() { Value = 38, Title = "QQ", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings FHQQ = new LotteryRecommendTitleSettings() { Value = 39, Title = "凤凰腾讯", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings WeixinQQ = new LotteryRecommendTitleSettings() { Value = 40, Title = "微信", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings Jx11x5 = new LotteryRecommendTitleSettings() { Value = 41, Title = "江西", SubTitle = "十一选五" };
        public static LotteryRecommendTitleSettings Js11x5 = new LotteryRecommendTitleSettings() { Value = 42, Title = "江苏", SubTitle = "十一选五" };
        public static LotteryRecommendTitleSettings VietSSC = new LotteryRecommendTitleSettings() { Value = 43, Title = "河內", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings VRSCPK10 = new LotteryRecommendTitleSettings() { Value = 44, Title = "VR", SubTitle = "赛车" };
        public static LotteryRecommendTitleSettings Viet5SSC = new LotteryRecommendTitleSettings() { Value = 45, Title = "河內", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings ChichuQQ = new LotteryRecommendTitleSettings() { Value = 46, Title = "奇趣腾讯", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings JL11x5 = new LotteryRecommendTitleSettings() { Value = 47, Title = "吉林", SubTitle = "十一选五" };
        public static LotteryRecommendTitleSettings NVietSSC = new LotteryRecommendTitleSettings() { Value = 48, Title = "河内", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings NViet5SSC = new LotteryRecommendTitleSettings() { Value = 49, Title = "河内", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings AHK3 = new LotteryRecommendTitleSettings() { Value = 50, Title = "安徽", SubTitle = "快三" };
        public static LotteryRecommendTitleSettings GSK3 = new LotteryRecommendTitleSettings() { Value = 51, Title = "甘肃", SubTitle = "快三" };
        public static LotteryRecommendTitleSettings ChichuQQ5 = new LotteryRecommendTitleSettings() { Value = 52, Title = "奇趣腾讯", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings GugeSSC = new LotteryRecommendTitleSettings() { Value = 53, Title = "谷歌", SubTitle = "分分彩" };
        public static LotteryRecommendTitleSettings YTSSC = new LotteryRecommendTitleSettings() { Value = 54, Title = "水管", SubTitle = "分分彩" };
        //public static LotteryRecommendTitleSettings MOLHC = new LotteryRecommendTitleSettings() { Value = 55, Title = "澳门", SubTitle = "六合彩" };
        public static LotteryRecommendTitleSettings XGLHC = new LotteryRecommendTitleSettings() { Value = 56, Title = "香港", SubTitle = "六合彩" };
        public static LotteryRecommendTitleSettings VR5SSC = new LotteryRecommendTitleSettings() { Value = 57, Title = "VR", SubTitle = "5分彩" };
        public static LotteryRecommendTitleSettings VR5PK10 = new LotteryRecommendTitleSettings() { Value = 58, Title = "VR5分", SubTitle = "PK10" };
        public static LotteryRecommendTitleSettings VR511x5 = new LotteryRecommendTitleSettings() { Value = 59, Title = "VR5分十一选五", SubTitle = "十一选五" };
        public static LotteryRecommendTitleSettings VR5K3 = new LotteryRecommendTitleSettings() { Value = 60, Title = "VR5分快三", SubTitle = "快三" };

        public static LotteryRecommendTitleSettings Xy28 = new LotteryRecommendTitleSettings() { Value = 63, Title = "幸运28", SubTitle = "幸运28" };
    }
}
