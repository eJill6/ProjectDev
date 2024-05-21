using System.ComponentModel;

namespace MS.Core.MMModel.Models.User.Enums
{
    /// <summary>
    /// 身份類型
    /// </summary>
    public enum IdentityType
    {
        /// <summary>
        /// 一般
        /// </summary>
        [Description("一般")]
        General = 0,

        /// <summary>
        /// 觅经纪
        /// </summary>
        [Description("觅经纪")]
        Agent = 1,

        /// <summary>
        /// 觅女郎
        /// </summary>
        [Description("觅女郎")]
        Girl = 2,

        /// <summary>
        /// 觅老板
        /// </summary>
        [Description("觅老板")]
        Boss = 3,

        /// <summary>
        /// 星觅官
        /// </summary>
        [Description("星觅官")]
        Officeholder = 4,
        /// <summary>
        /// 超觅老板
        /// </summary>
        [Description("超觅老板")]
        SuperBoss = 5
    }
}