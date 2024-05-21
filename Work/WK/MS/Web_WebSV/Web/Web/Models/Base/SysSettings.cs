using System;
namespace Web.Models.Base
{
	/// <summary>
	/// SysSettings:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
    //[Serializable]
	public class SysSettings
	{
		public SysSettings()
		{}
		#region Model
		private int settingsid;
		private int? maxbetcount;
		private decimal? maxonebetmoney;
		private decimal? minonebetmoney;
		private decimal? maxbonusmoney;
		private decimal? minmoneyout;
		private int? maxmoneyoutcount;
		private decimal? maxmoneyout;
		private decimal? minmoneyin;
		private decimal? maxuserrebatepro;
		/// <summary>
		/// ID
		/// </summary>
		public int SettingsID
		{
			set{ settingsid=value;}
			get{return settingsid;}
		}
		/// <summary>
		/// 每次下单最大注数
		/// </summary>
		public int? MaxBetCount
		{
			set{ maxbetcount=value;}
			get{return maxbetcount;}
		}
		/// <summary>
		/// 单注下单分数最大值
		/// </summary>
		public decimal? MaxOneBetMoney
		{
			set{ maxonebetmoney=value;}
			get{return maxonebetmoney;}
		}
		/// <summary>
		/// 单注下单分数最小值
		/// </summary>
		public decimal? MinOneBetMoney
		{
			set{ minonebetmoney=value;}
			get{return minonebetmoney;}
		}
	
		/// <summary>
		/// 最大中奖金额
		/// </summary>
		public decimal? MaxBonusMoney
		{
			set{ maxbonusmoney=value;}
			get{return maxbonusmoney;}
		}
		/// <summary>
		/// 最小提现金额
		/// </summary>
		public decimal? MinMoneyOut
		{
			set{ minmoneyout=value;}
			get{return minmoneyout;}
		}
		/// <summary>
		/// 每天最多提现次数
		/// </summary>
		public int? MaxMoneyOutCount
		{
			set{ maxmoneyoutcount=value;}
			get{return maxmoneyoutcount;}
		}
		/// <summary>
		/// 最大提现金额
		/// </summary>
		public decimal? MaxMoneyOut
		{
			set{ maxmoneyout=value;}
			get{return maxmoneyout;}
		}
		/// <summary>
		/// 最小充值金额
		/// </summary>
		public decimal? MinMoneyIn
		{
			set{ minmoneyin=value;}
			get{return minmoneyin;}
		}
		/// <summary>
		/// 最高配额值，总号使用
		/// </summary>
		public decimal? MaxUserRebatePro
		{
			set{ maxuserrebatepro=value;}
			get{return maxuserrebatepro;}
		}

		/// <summary>
		/// 最小提现金额
		/// </summary>
		public decimal? MinUsdtMoneyOut { get; set; }

		/// <summary>
		/// 最大提现金额
		/// </summary>
		public decimal? MaxUsdtMoneyOut { get; set; }

		#endregion Model

	}
}

