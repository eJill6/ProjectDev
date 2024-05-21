namespace MS.Core.MMModel.Models.SystemSettings
{
    /// <summary>
    /// 項目
    /// </summary>
    public class OptionItemForClient
    {
        /// <summary>
        /// 項目編號
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// 項目值
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}