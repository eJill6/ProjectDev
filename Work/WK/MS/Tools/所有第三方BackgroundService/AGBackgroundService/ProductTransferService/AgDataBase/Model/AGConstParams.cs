﻿namespace ProductTransferService.AgDataBase.Model
{
    public static class AGConstParams
    {
        public static string Url { get; set; }

        public static string Cagent { get; set; }

        public static string Cur { get; set; }

        public static int Actype { get; set; }

        public static string DesKey { get; set; }

        public static string Md5Key { get; set; }

        public static readonly Dictionary<string, string> PlatformTypes = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> Rounds = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> GameTypes = new Dictionary<string, string>();

        public static readonly Dictionary<string, string> PlayTypes = new Dictionary<string, string>();

        static AGConstParams()
        {
            PlatformTypes.Add("AGIN", "AG国际厅");
            PlatformTypes.Add("AG", "AG旗舰厅极速版");
            PlatformTypes.Add("DSP", "AG实地厅");
            PlatformTypes.Add("IPM", "IPM");
            PlatformTypes.Add("BBIN", "BBIN宝盈集团");
            PlatformTypes.Add("MG", "Microgaming");
            PlatformTypes.Add("SABAH", "沙巴体育");
            PlatformTypes.Add("HG", "HOGaming");
            PlatformTypes.Add("PT", "PlayTech");
            PlatformTypes.Add("NMG", "Microgaming (New)");
            PlatformTypes.Add("OG", "东方游戏");
            PlatformTypes.Add("UGS", "Unified Gaming Server");
            PlatformTypes.Add("HUNTER", "捕鱼王");
            PlatformTypes.Add("AGTEX", "棋牌大厅");
            PlatformTypes.Add("HBR", "Habanero");
            PlatformTypes.Add("XTD", "新天地");
            PlatformTypes.Add("PNG", "Play’n GO");
            PlatformTypes.Add("NYX", "NYX Gaming Group");
            PlatformTypes.Add("ENDO", "Endorphina");
            PlatformTypes.Add("BG", "Booming Games");
            PlatformTypes.Add("XIN", "XIN Gaming");
            PlatformTypes.Add("SBTA", "AG体育");
            PlatformTypes.Add("YOPLAY", "YoPlay");
            PlatformTypes.Add("NMGE", "Microgaming (New)");
            PlatformTypes.Add("TTG", "TTG");

            Rounds.Add("DSP", "国际厅");
            Rounds.Add("AGQ", "旗舰厅");
            Rounds.Add("VIP", "包桌厅");
            Rounds.Add("SLOT", "电子游戏");
            Rounds.Add("LED", "竞咪厅");
            Rounds.Add("AGHH", "豪华厅");
            Rounds.Add("LOTTO", "彩票");

            GameTypes.Add("BAC", "百家乐");
            GameTypes.Add("CBAC", "包桌百家乐");
            GameTypes.Add("LINK", "连环百家乐");
            GameTypes.Add("DT", "龙虎");
            GameTypes.Add("SHB", "骰宝");
            GameTypes.Add("ROU", "轮盘");
            GameTypes.Add("FT", "番摊");
            GameTypes.Add("LBAC", "竞咪百家乐");
            GameTypes.Add("ULPK", "终极德州扑克");
            GameTypes.Add("SBAC", "保險百家樂");

            GameTypes.Add("NN", "牛牛");
            GameTypes.Add("BJ", "21点");
            GameTypes.Add("ZJH", "炸金花");

            GameTypes.Add("SL1", "巴西世界杯");
            GameTypes.Add("SL2", "疯狂水果店");
            GameTypes.Add("SL3", "3D水族馆");
            GameTypes.Add("PK_J", "视频扑克(杰克高手)");
            GameTypes.Add("SL4", "极速赛车");
            GameTypes.Add("PKBJ", "新视频扑克(杰克高手)");
            GameTypes.Add("FRU", "水果拉霸");
            GameTypes.Add("HUNTER", "捕鱼王");
            GameTypes.Add("SLM1", "美女沙排(沙滩排球)");
            GameTypes.Add("SLM2", "运财羊(新年运财羊)");
            GameTypes.Add("SLM3", "武圣传");
            GameTypes.Add("SC01", "幸运老虎机");
            GameTypes.Add("TGLW", "极速幸运轮");
            GameTypes.Add("SLM4", "武则天");
            GameTypes.Add("TGCW", "赌场战争");
            GameTypes.Add("SB01", "太空漫游");
            GameTypes.Add("SB02", "复古花园");
            GameTypes.Add("SB03", "关东煮");
            GameTypes.Add("SB04", "牧场咖啡");
            GameTypes.Add("SB05", "甜一甜屋");
            GameTypes.Add("SB06", "日本武士");
            GameTypes.Add("SB07", "象棋老虎机");
            GameTypes.Add("SB08", "麻将老虎机");
            GameTypes.Add("SB09", "西洋棋老虎机");
            GameTypes.Add("SB10", "开心农场");
            GameTypes.Add("SB11", "夏日营地");
            GameTypes.Add("SB12", "海底漫游");
            GameTypes.Add("SB13", "鬼马小丑");
            GameTypes.Add("SB14", "机动乐园");
            GameTypes.Add("SB15", "惊吓鬼屋");
            GameTypes.Add("SB16", "疯狂马戏团");
            GameTypes.Add("SB17", "海洋剧场");
            GameTypes.Add("SB18", "水上乐园");
            GameTypes.Add("SB25", "土地神");
            GameTypes.Add("SB26", "布袋和尚");
            GameTypes.Add("SB27", "正财神");
            GameTypes.Add("SB28", "武财神");
            GameTypes.Add("SB29", "偏财神");
            GameTypes.Add("SB19", "空中战争");
            GameTypes.Add("SB20", "摇滚狂迷");
            GameTypes.Add("SB21", "越野机车");
            GameTypes.Add("SB22", "埃及奥秘");
            GameTypes.Add("SB23", "欢乐时光");
            GameTypes.Add("SB24", "侏罗纪");
            GameTypes.Add("AV01", "性感女仆");
            GameTypes.Add("XG01", "龙珠");
            GameTypes.Add("XG02", "幸运8");
            GameTypes.Add("XG03", "闪亮女郎");
            GameTypes.Add("XG04", "金鱼");
            GameTypes.Add("XG05", "中国新年");
            GameTypes.Add("XG06", "海盗王");
            GameTypes.Add("XG07", "鲜果狂热");
            GameTypes.Add("XG08", "小熊猫");
            GameTypes.Add("XG09", "大豪客");
            GameTypes.Add("SB30", "灵猴献瑞");
            GameTypes.Add("SB31", "天空守护者");
            //GameTypes.Add("XG10", "龙舟竞渡");
            GameTypes.Add("PKBD", "百搭二王");
            GameTypes.Add("PKBB", "红利百搭");
            GameTypes.Add("SB32", "齐天大圣");
            GameTypes.Add("SB33", "糖果碰碰乐");
            GameTypes.Add("SB34", "冰河世界");
            GameTypes.Add("FRU2", "水果拉霸2");
            GameTypes.Add("TG01", "21点 (电子游戏)");
            GameTypes.Add("TG02", "百家乐 (电子游戏)");
            GameTypes.Add("TG03", "轮盘 (电子游戏)");
            GameTypes.Add("SB35", "欧洲列强争霸");
            GameTypes.Add("SB36", "捕鱼王者");
            GameTypes.Add("SB37", "上海百乐门");

            GameTypes.Add("SB38", "竞技狂热");
            GameTypes.Add("SB39", "太空水果");
            GameTypes.Add("SB40", "秦始皇");
            GameTypes.Add("TA01", "多手二十一点 低额投注");
            GameTypes.Add("TA02", "多手二十一点");
            GameTypes.Add("TA03", "多手二十一点 高额投注");
            GameTypes.Add("TA04", "1手二十一点 低额投注");
            GameTypes.Add("TA05", "1手二十一点");
            GameTypes.Add("TA06", "1手二十一点 高额投注");
            GameTypes.Add("TA07", "Hilo 低额投注");
            GameTypes.Add("TA08", "Hilo");
            GameTypes.Add("TA09", "Hilo 高額投注");
            GameTypes.Add("TA0A", "5手 Hilo");
            GameTypes.Add("TA0B", "5手 Hilo 高额投注");
            GameTypes.Add("TA0C", "3手 Hilo 高额投注");
            GameTypes.Add("TA0F", "轮盘 高额投注");
            GameTypes.Add("TA0G", "轮盘");
            GameTypes.Add("TA0Z", "5手杰克高手");
            GameTypes.Add("TA10", "5手百搭小丑");
            GameTypes.Add("TA11", "5手百搭二王");
            GameTypes.Add("TA12", "1手杰克高手");
            GameTypes.Add("TA13", "10手杰克高手");
            GameTypes.Add("TA14", "25手杰克高手");
            GameTypes.Add("TA15", "50手杰克高手");
            GameTypes.Add("TA17", "1手百搭小丑");
            GameTypes.Add("TA18", "10手百搭小丑");
            GameTypes.Add("TA19", "25手百搭小丑");
            GameTypes.Add("TA1A", "50手百搭小丑");
            GameTypes.Add("TA1C", "1手百搭二王");
            GameTypes.Add("TA1D", "10手百搭二王");
            GameTypes.Add("TA1E", "25手百搭二王");
            GameTypes.Add("TA1F", "50手百搭二王");
            GameTypes.Add("TA0U", "经典轿车");
            GameTypes.Add("TA0V", "星际大战");
            GameTypes.Add("TA0W", "海盗夺宝");
            GameTypes.Add("TA0X", "巴黎茶座");
            GameTypes.Add("TA0Y", "金龙献宝");
            GameTypes.Add("XG10", "龙舟竞渡");
            GameTypes.Add("XG11", "中秋佳节");
            GameTypes.Add("XG12", "韩风劲舞");
            GameTypes.Add("XG13", "美女大格斗");
            GameTypes.Add("XG14", "龙凤呈祥");
            GameTypes.Add("XG16", "黄金对垒");
            GameTypes.Add("TA0P", "怪兽食坊");
            GameTypes.Add("TA0S", "足球竞赛");
            GameTypes.Add("TA0L", "无法无天");
            GameTypes.Add("TA0M", "法老秘密");
            GameTypes.Add("TA0N", "烈火战车");
            GameTypes.Add("TA0O", "捕猎季节");
            GameTypes.Add("TA0Q", "日与夜");
            GameTypes.Add("TA0R", "七大奇迹");
            GameTypes.Add("TA0T", "珠光宝气");
            GameTypes.Add("TA1N", "欧洲轮盘 高额投注(移动版)");
            GameTypes.Add("TA1O", "欧洲轮盘(移动版)");
            GameTypes.Add("TA1P", "欧洲轮盘 低额投注(移动版)");
            GameTypes.Add("TA1K", "欧洲轮盘 高额投注(桌面版)");
            GameTypes.Add("TA1L", "欧洲轮盘(桌面版)");
            GameTypes.Add("TA1M", "欧洲轮盘 低额投注(桌面版)");

            GameTypes.Add("SV41", "富贵金鸡");
            GameTypes.Add("SX01", "赛亚烈战");
            GameTypes.Add("SX02", "街头烈战");
            GameTypes.Add("SC03", "金拉霸");

            GameTypes.Add("27", "江苏快三");
            GameTypes.Add("24", "重庆时时彩");
            GameTypes.Add("13", "中国福彩3D");
            GameTypes.Add("25", "北京快乐8");
            GameTypes.Add("26", "湖南快乐十分");
            GameTypes.Add("29", "十一运夺金");
            GameTypes.Add("23", "江西时时彩");

            GameTypes.Add("DZPK", "德州扑克");
            GameTypes.Add("GDMJ", "广东麻將");

            GameTypes.Add("FIFA", "体育");
            GameTypes.Add("SPTA", "AG体育");

            GameTypes.Add("YFP", "水果派对");
            GameTypes.Add("YDZ", "德州牛仔");
            GameTypes.Add("YBIR", "飞禽走兽");
            GameTypes.Add("YMFD", "森林舞会多人版");
            GameTypes.Add("YFD", "森林舞会");
            GameTypes.Add("YBEN", "奔驰宝马");
            GameTypes.Add("YHR", "极速赛马");
            GameTypes.Add("YMFR", "水果拉霸多人版");
            GameTypes.Add("YGS", "猜猜乐");
            GameTypes.Add("YFR", "水果拉霸");
            GameTypes.Add("YMGS", "猜猜乐多人版");
            GameTypes.Add("YMBN", "百人牛牛");
            GameTypes.Add("YGFS", "多宝水果拉霸");
            GameTypes.Add("YJFS", "彩金水果拉霸");
            GameTypes.Add("YMBI", "飞禽走兽多人版");

            PlayTypes.Add("1", "庄");
            PlayTypes.Add("2", "闲");
            PlayTypes.Add("3", "和");
            PlayTypes.Add("4", "庄对");
            PlayTypes.Add("5", "闲对");
            PlayTypes.Add("6", "大");
            PlayTypes.Add("7", "小");
            PlayTypes.Add("8", "莊保險");
            PlayTypes.Add("9", "閑保險");
            PlayTypes.Add("11", "庄免佣");
            PlayTypes.Add("12", "庄龙宝");
            PlayTypes.Add("13", "闲龙宝");

            PlayTypes.Add("14", "超级六");
            PlayTypes.Add("15", "任意对子");
            PlayTypes.Add("16", "完美对子");

            PlayTypes.Add("21", "龙");
            PlayTypes.Add("22", "虎");
            PlayTypes.Add("23", "和（龙虎）");

            PlayTypes.Add("41", "大");
            PlayTypes.Add("42", "小");
            PlayTypes.Add("43", "单");
            PlayTypes.Add("44", "双");
            PlayTypes.Add("45", "全围");
            PlayTypes.Add("46", "围1");
            PlayTypes.Add("47", "围2");
            PlayTypes.Add("48", "围3");
            PlayTypes.Add("49", "围4");
            PlayTypes.Add("50", "围5");
            PlayTypes.Add("51", "围6");
            PlayTypes.Add("52", "单点1");
            PlayTypes.Add("53", "单点2");
            PlayTypes.Add("54", "单点3");
            PlayTypes.Add("55", "单点4");
            PlayTypes.Add("56", "单点5");
            PlayTypes.Add("57", "单点6");
            PlayTypes.Add("58", "对子1");
            PlayTypes.Add("59", "对子2");
            PlayTypes.Add("60", "对子3");
            PlayTypes.Add("61", "对子4");
            PlayTypes.Add("62", "对子5");
            PlayTypes.Add("63", "对子6");
            PlayTypes.Add("64", "组合12");
            PlayTypes.Add("65", "组合13");
            PlayTypes.Add("66", "组合14");
            PlayTypes.Add("67", "组合15");
            PlayTypes.Add("68", "组合16");
            PlayTypes.Add("69", "组合23");
            PlayTypes.Add("70", "组合24");
            PlayTypes.Add("71", "组合25");
            PlayTypes.Add("72", "组合26");
            PlayTypes.Add("73", "组合34");
            PlayTypes.Add("74", "组合35");
            PlayTypes.Add("75", "组合36");
            PlayTypes.Add("76", "组合45");
            PlayTypes.Add("77", "组合46");
            PlayTypes.Add("78", "组合56");
            PlayTypes.Add("79", "和值4");
            PlayTypes.Add("80", "和值5");
            PlayTypes.Add("81", "和值6");
            PlayTypes.Add("82", "和值7");
            PlayTypes.Add("83", "和值8");
            PlayTypes.Add("84", "和值9");
            PlayTypes.Add("85", "和值10");
            PlayTypes.Add("86", "和值11");
            PlayTypes.Add("87", "和值12");
            PlayTypes.Add("88", "和值13");
            PlayTypes.Add("89", "和值14");
            PlayTypes.Add("90", "和值15");
            PlayTypes.Add("91", "和值16");
            PlayTypes.Add("92", "和值17");

            PlayTypes.Add("101", "直接注");
            PlayTypes.Add("102", "分注");
            PlayTypes.Add("103", "街注");
            PlayTypes.Add("104", "三數");
            PlayTypes.Add("105", "4個號碼");
            PlayTypes.Add("106", "角注");
            PlayTypes.Add("107", "列注(列1)");
            PlayTypes.Add("108", "列注(列2)");
            PlayTypes.Add("109", "列注(列3)");
            PlayTypes.Add("110", "線注");
            PlayTypes.Add("111", "打一");
            PlayTypes.Add("112", "打二");
            PlayTypes.Add("113", "打三");
            PlayTypes.Add("114", "紅");
            PlayTypes.Add("115", "黑");
            PlayTypes.Add("116", "大");
            PlayTypes.Add("117", "小");
            PlayTypes.Add("118", "單");
            PlayTypes.Add("119", "雙");

            PlayTypes.Add("130", "1番");
            PlayTypes.Add("131", "2番");
            PlayTypes.Add("132", "3番");
            PlayTypes.Add("133", "4番");
            PlayTypes.Add("134", "1念2");
            PlayTypes.Add("135", "1念3");
            PlayTypes.Add("136", "1念4");
            PlayTypes.Add("137", "2念1");
            PlayTypes.Add("138", "2念3");
            PlayTypes.Add("139", "2念4");
            PlayTypes.Add("140", "3念1");
            PlayTypes.Add("141", "3念2");
            PlayTypes.Add("142", "3念4");
            PlayTypes.Add("143", "4念1");
            PlayTypes.Add("144", "4念2");
            PlayTypes.Add("145", "4念3");
            PlayTypes.Add("146", "角(1,2)");
            PlayTypes.Add("147", "單");
            PlayTypes.Add("148", "角(1,4)");
            PlayTypes.Add("149", "角(2,3)");
            PlayTypes.Add("150", "雙");
            PlayTypes.Add("151", "角(3,4)");
            PlayTypes.Add("152", "1,2四 通");
            PlayTypes.Add("153", "1,2三 通");
            PlayTypes.Add("154", "1,3四 通");
            PlayTypes.Add("155", "1,3二 通");
            PlayTypes.Add("156", "1,4三 通");
            PlayTypes.Add("157", "1,4二 通");
            PlayTypes.Add("158", "2,3四 通");
            PlayTypes.Add("159", "2,3一 通");
            PlayTypes.Add("160", "2,4三 通");
            PlayTypes.Add("161", "2,4一 通");
            PlayTypes.Add("162", "3,4二 通");
            PlayTypes.Add("163", "3,4一 通");
            PlayTypes.Add("164", "三門(3,2,1)");
            PlayTypes.Add("165", "三門(2,1,4)");
            PlayTypes.Add("166", "三門(1,4,3)");
            PlayTypes.Add("167", "三門(4,3,2)");

            PlayTypes.Add("10", "任选五");
            PlayTypes.Add("17", "任选六");
            PlayTypes.Add("18", "任选七");
            PlayTypes.Add("19", "任选八");
            PlayTypes.Add("20", "任选九");
            PlayTypes.Add("587", "三星组六");

            PlayTypes.Add("180", "底注+盲注");
            PlayTypes.Add("181", "一倍加注");
            PlayTypes.Add("182", "二倍加注");
            PlayTypes.Add("183", "三倍加注");
            PlayTypes.Add("184", "四倍加注");

            PlayTypes.Add("190", "主队");
            PlayTypes.Add("191", "客队");
            PlayTypes.Add("192", "和");
            PlayTypes.Add("193", "大");
            PlayTypes.Add("194", "小");
            PlayTypes.Add("195", "单");
            PlayTypes.Add("196", "双");

            PlayTypes.Add("211", "闲1平倍");
            PlayTypes.Add("212", "闲1翻倍");
            PlayTypes.Add("213", "闲2平倍");
            PlayTypes.Add("214", "闲2翻倍");
            PlayTypes.Add("215", "闲3平倍");
            PlayTypes.Add("216", "闲3翻倍");
            PlayTypes.Add("207", "庄1平倍");
            PlayTypes.Add("208", "庄1翻倍");
            PlayTypes.Add("209", "庄2平倍");
            PlayTypes.Add("210", "庄2翻倍");
            PlayTypes.Add("217", "庄3平倍");
            PlayTypes.Add("218", "庄3翻倍");
            PlayTypes.Add("220", "底注");
            PlayTypes.Add("221", "分牌");
            PlayTypes.Add("222", "保险");
            PlayTypes.Add("223", "分牌保险");
            PlayTypes.Add("224", "加注");
            PlayTypes.Add("225", "分牌加注");
            PlayTypes.Add("226", "完美对子");
            PlayTypes.Add("227", "21+3");
            PlayTypes.Add("228", "旁注");
            PlayTypes.Add("229", "旁注分牌");
            PlayTypes.Add("230", "旁注保险");
            PlayTypes.Add("231", "旁注分牌保险");
            PlayTypes.Add("232", "旁注加注");
            PlayTypes.Add("233", "旁注分牌加注");
            PlayTypes.Add("260", "龙");
            PlayTypes.Add("261", "凤");
            PlayTypes.Add("262", "对8以上");
            PlayTypes.Add("263", "同花");
            PlayTypes.Add("264", "顺子");
            PlayTypes.Add("265", "豹子");
            PlayTypes.Add("266", "同花顺");
        }
    }
}