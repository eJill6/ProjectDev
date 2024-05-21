using System.ComponentModel;

namespace MS.Core.MMModel.Models.Post.Enums
{
    /// <summary>
    /// Cup 定義
    /// </summary>
    public enum CupDefined
    {
        /// <summary>
        ///
        /// </summary>
        [Description("A")]
        A = 1,

        /// <summary>
        ///
        /// </summary>
        [Description("B")]
        B = 2,

        /// <summary>
        ///
        /// </summary>
        [Description("C")]
        C = 3,

        /// <summary>
        ///
        /// </summary>
        [Description("D")]
        D = 4,

        /// <summary>
        /// E Cup
        /// </summary>
        [Description("E")]
        E = 5,

        /// <summary>
        /// E+ 主要是若要擴展選項，plus會錯用，因此plus用於上限延展
        /// </summary>
        [Description("E+")]
        Plus = 99
    }
}