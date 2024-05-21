using System;
namespace SLPolyGame.Web.Model
{
	/// <summary>
	/// VersionInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
    //[Serializable]
	public class VersionInfo
	{
		public VersionInfo()
		{}
		#region Model
		private string version;
		/// <summary>
		/// 版本号
		/// </summary>
		public string Version
		{
			set{ version=value;}
			get{return version;}
		}
		#endregion Model

	}
}

