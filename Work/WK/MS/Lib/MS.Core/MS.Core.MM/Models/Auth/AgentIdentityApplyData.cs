using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Models.Auth
{
    /// <summary>
    /// 覓经纪身份申請資料
    /// </summary>
    public class AgentIdentityApplyData
    {
		/// <summary>
		/// 聯絡方式。1：微信、2：QQ、3：手機號
		/// </summary>
		public string ContactType { get; set; }

		/// <summary>
		/// 聯繫方式
		/// </summary>
		public string Contact { get; set; } = string.Empty;
	}
}