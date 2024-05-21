using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    /// <summary>
    /// 游戏平台
    /// </summary>
    public class GamePlatformModel
    {
        /// <summary>
        /// 游戏平台ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformName { get; set; }

        /// <summary>
        /// 游戏ID
        /// </summary>
        public string GameId { get; set; }
        /// <summary>
        /// 游戏名称
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 游戏类型
        /// </summary>
        public GameType GameType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int? Satus { get; set; }

        /// <summary>
        /// 菜单logo
        /// </summary>
        public string MenuImg { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string UpdateUser { get; set; }
    }

    /// <summary>
    /// 游戏类型
    /// </summary>
    public enum GameType
    {
        UNKNOWN = 0,
        /// <summary>
        /// 1真人
        /// </summary>
        Trueman = 1,

        /// <summary>
        /// 2彩票
        /// </summary>
        Lottery = 2,

        /// <summary>
        /// 3体育
        /// </summary>
        Sport = 3,

        /// <summary>
        /// 4电子
        /// </summary>
        Electron = 4
    }

    public class GameTypeName
    {
        public static string GetGameTypeName(GameType type)
        {
            switch (type)
            {
                case GameType.Trueman:
                    return "真人";
                case GameType.Lottery:
                    return "彩票";
                case GameType.Sport:
                    return "体育";
                case GameType.Electron:
                    return "电子";
            }
            return "";
        }
    }
}