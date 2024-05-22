using IMBGDataBase.Enums;
using System;

namespace IMBGDataBase.Common
{
	/// <summary>
	/// DateTimeUtility
	/// </summary>
	public class DateTimeUtility
	{
		/// <summary>
		/// The instance
		/// </summary>
		private static readonly Lazy<DateTimeUtility> _instance = new Lazy<DateTimeUtility>(() => new DateTimeUtility());

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>
		/// The instance.
		/// </value>
		public static DateTimeUtility Instance => _instance.Value;

		#region Get Timestamp
		/// <summary>
		/// Get Timestamp (到秒)
		/// </summary>
		/// <param name="gmt">GMT +8 = 台北時間</param>
		/// <returns>long</returns>
		public long GetTimestamp(int gmt = 8)
		{
			DateTime gtm = new DateTime(1970, 1, 1);//宣告一個GTM時間出來
			DateTime utc = DateTime.UtcNow.AddHours(gmt);//宣告一個目前的時間
														 //我們把現在時間減掉GTM時間得到的秒數就是timpStamp，因為我不要小數點後面的所以我把它轉成int
			return Convert.ToInt64(((TimeSpan)utc.Subtract(gtm)).TotalSeconds);
		}

		/// <summary>
		/// Get Timestamp (到毫秒)
		/// </summary>
		/// <param name="gmt">GMT +8 = 台北時間</param>
		/// <returns>long</returns>
		public long GetTimestampMs(int gmt = 8)
		{
			DateTime gtm = new DateTime(1970, 1, 1);//宣告一個GTM時間出來
			DateTime utc = DateTime.UtcNow.AddHours(gmt);//宣告一個目前的時間
														 //我們把現在時間減掉GTM時間得到的秒數就是timpStamp，因為我不要小數點後面的所以我把它轉成int
			return Convert.ToInt64(((TimeSpan)utc.Subtract(gtm)).TotalMilliseconds);			
		}
		#endregion

		#region Convert Unix timestamp to DateTime
		/// <summary>
		/// Convert Unix timestamp to DateTime
		/// </summary>
		/// <param name="unixTimeStamp">timestamp</param>
		/// <returns>DateTime</returns>
		public DateTime ConvertTimestampToDateTime(long unixTimeStamp)
		{
			DateTime unixYear0 = new DateTime(1970, 1, 1);
			long unixTimeStampInTicks = unixTimeStamp * TimeSpan.TicksPerSecond;
			DateTime dtUnix = new DateTime(unixYear0.Ticks + unixTimeStampInTicks);
			return dtUnix;
		}

		/// <summary>
		/// method for converting a System.DateTime value to a UNIX Timestamp
		/// </summary>
		/// <param name="value">date to convert</param>
		/// <returns></returns>
		public long ConvertToTimestamp(DateTime value)
		{
			//create Timespan by subtracting the value provided from
			//the Unix Epoch
			TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

			//return the total seconds (which is a UNIX timestamp)
			return Convert.ToInt64(span.TotalSeconds);
		}

        /// <summary>
        /// Convert Unix timestamp to DateTime
        /// </summary>
        /// <param name="unixTimestamp"></param>
        /// <returns></returns>
        public DateTime ToLocalTime(string unixTimestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)
                .AddSeconds(long.Parse(unixTimestamp))
                .ToLocalTime();
        }
        #endregion

        #region 判斷 字串是否符合日期格式
        /// <summary>
        /// 判斷 字串是否符合日期格式
        /// </summary>
        /// <param name="val">輸入日期格式字串</param>
        /// <returns>bool</returns>
        public bool IsValidDate(string val)
		{
			DateTime sd;
			if (string.IsNullOrWhiteSpace(val) ||
				!DateTime.TryParse(val, out sd))
				return false;
			return true;
		}
		#endregion

		#region 判斷 字串是否符合日期格式
		/// <summary>
		/// 判斷 字串是否符合日期格式
		/// </summary>
		/// <param name="val">日期字串</param>
		/// <returns>bool</returns>
		public bool IsDateTime(string val)
		{
			DateTime myDt;
			bool isValid = false;

			if (!string.IsNullOrWhiteSpace(val))
			{
				isValid = DateTime.TryParse(val, out myDt);
			}

			return isValid;
		}

		/// <summary>
		/// 判斷 字串是否符合日期格式
		/// </summary>
		/// <param name="val">日期字串</param>
		/// <param name="format">DateTime Format, EX:yyyy/MM/dd</param>
		/// <returns>bool</returns>
		public bool IsDateTime(string val, string format)
		{
			DateTime myDt;
			bool isValid = false;
			if (!string.IsNullOrWhiteSpace(val))
			{
				isValid = DateTime.TryParseExact(val, format, null, System.Globalization.DateTimeStyles.None, out myDt);
			}
			return isValid;
		}
		#endregion

		#region 取得 DateTime To 年齡
		/// <summary>
		/// 取得 DateTime To 年齡
		/// </summary>
		/// <param name="dt">DateTime</param>
		/// <returns>int</returns>
		public int GetDateTimeToAge(DateTime dt)
		{
			int intAge = -1;
			try
			{
				double dbl = GetDateTimeDiff(dt, DateTimeDiffTicks.TotalYears);
				intAge = Convert.ToInt32(dbl);
			}
			catch
			{
				throw;
			}
			return intAge;
		}
		#endregion

		#region 取得 WeekDay 一周的星期幾
		/// <summary>
		/// 取得 WeekDay 一周的星期幾
		/// </summary>
		/// <param name="dt">The string date time.</param>
		/// <returns>string</returns>
		public string GetDayOfWeek(string dt)
		{
			return !string.IsNullOrWhiteSpace(dt)
				? GetDayOfWeek(Convert.ToDateTime(dt)) : string.Empty;
		}

		/// <summary>
		/// 取得 WeekDay 一周的星期幾
		/// </summary>
		/// <param name="dt">The dt time.</param>
		/// <returns>string</returns>
		public string GetDayOfWeek(DateTime dt)
		{
			string strWeekDay = string.Empty;

			int intWeekDay = (int)dt.DayOfWeek;

			switch (intWeekDay)
			{
				case 1:
					strWeekDay = "一";
					break;
				case 2:
					strWeekDay = "二";
					break;
				case 3:
					strWeekDay = "三";
					break;
				case 4:
					strWeekDay = "四";
					break;
				case 5:
					strWeekDay = "五";
					break;
				case 6:
					strWeekDay = "六";
					break;
				case 0:
					strWeekDay = "日";
					break;
			}

			return strWeekDay;
		}
		#endregion

		#region 取得 從1970/1/1 開始的秒數
		/// <summary>
		/// 取得 從1970/1/1 開始的秒數
		/// </summary>
		/// <param name="dt">The dt.</param>
		/// <returns>double</returns>
		public double GetUtcMilliseconds(DateTime dt)
		{
			DateTime baseDate = new DateTime(1970, 1, 1);
			TimeSpan diff = dt - baseDate;
			return diff.TotalMilliseconds;
		}
		#endregion

		#region DateTime 時間比較

		#region 比目前時間大
		/// <summary>
		/// 比目前時間大
		/// </summary>
		/// <param name="compareTime">要比較的時間 格式</param>
		/// <returns>bool</returns>
		public bool IsBiggerThanNow(string compareTime)
		{
			if (IsValidDate(compareTime))
				return IsBiggerThanNow(Convert.ToDateTime(compareTime));
			return false;
		}

		/// <summary>
		/// 比目前時間大
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <returns>bool</returns>
		public bool IsBiggerThanNow(DateTime compareTime)
		{
			return IsBiggerThanNow(compareTime, DateTime.Now);
		}

		/// <summary>
		/// 比目前時間大
		/// </summary>
		/// <param name="compareTime">要比較的時間 格式</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsBiggerThanNow(string compareTime, string nowTime)
		{
			if (IsValidDate(compareTime) && IsValidDate(nowTime))
				return IsBiggerThanNow(Convert.ToDateTime(compareTime), Convert.ToDateTime(nowTime));
			return false;
		}

		/// <summary>
		/// 比目前時間大
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsBiggerThanNow(DateTime compareTime, DateTime nowTime)
		{
			bool isBig = false;
			if (DateTime.Compare(compareTime, nowTime) > 0)
				isBig = true;
			return isBig;
		}
		#endregion

		#region 比目前時間 >=
		/// <summary>
		/// 比目前時間 >=
		/// </summary>
		/// <param name="compare">要比較的時間 格式</param>
		/// <returns>bool</returns>
		public bool IsBiggerAndEquallyNow(string compare)
		{
			if (IsValidDate(compare))
				return IsBiggerAndEquallyNow(Convert.ToDateTime(compare));
			return false;
		}

		/// <summary>
		/// 比目前時間 >=
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <returns>bool</returns>
		public bool IsBiggerAndEquallyNow(DateTime compareTime)
		{
			return IsBiggerAndEquallyNow(compareTime, DateTime.Now);
		}

		/// <summary>
		/// 比目前時間 >=
		/// </summary>
		/// <param name="compareTime">要比較的時間 格式</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsBiggerAndEquallyNow(string compareTime, string nowTime)
		{
			if (IsValidDate(compareTime) && IsValidDate(nowTime))
				return IsBiggerAndEquallyNow(Convert.ToDateTime(compareTime), Convert.ToDateTime(nowTime));
			return false;
		}

		/// <summary>
		/// 比目前時間 >=
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsBiggerAndEquallyNow(DateTime compareTime, DateTime nowTime)
		{
			bool isBig = false;
			if (DateTime.Compare(compareTime, nowTime) >= 0)
				isBig = true;
			return isBig;
		}
		#endregion

		#region 比目前時間 小
		/// <summary>
		/// 比目前時間 小
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <returns>bool</returns>
		public bool IsSmallerThanNow(string compareTime)
		{
			if (IsValidDate(compareTime))
				return IsSmallerThanNow(Convert.ToDateTime(compareTime));
			return false;
		}

		/// <summary>
		/// 比目前時間 小
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <returns>bool</returns>
		public bool IsSmallerThanNow(DateTime compareTime)
		{
			return IsSmallerThanNow(compareTime, DateTime.Now);
		}

		/// <summary>
		/// 比目前時間 小
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsSmallerThanNow(string compareTime, string nowTime)
		{
			if (IsValidDate(compareTime) && IsValidDate(nowTime))
				return IsSmallerThanNow(Convert.ToDateTime(compareTime), Convert.ToDateTime(nowTime));
			return false;
		}

		/// <summary>
		/// 比目前時間 小
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsSmallerThanNow(DateTime compareTime, DateTime nowTime)
		{
			bool isSmall = false;
			if (DateTime.Compare(compareTime, nowTime) < 0)
				isSmall = true;
			return isSmall;
		}
		#endregion

		#region 比目前時間 <=
		/// <summary>
		/// 比目前時間 小於 且 等於
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <returns>bool</returns>
		public bool IsSmallerAndEquallyNow(string compareTime)
		{
			if (IsValidDate(compareTime))
				return IsSmallerAndEquallyNow(Convert.ToDateTime(compareTime));
			return false;
		}

		/// <summary>
		/// 比目前時間 小於 且 等於
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <returns>bool</returns>
		public bool IsSmallerAndEquallyNow(DateTime compareTime)
		{
			return IsSmallerAndEquallyNow(compareTime, DateTime.Now);
		}

		/// <summary>
		/// 比目前時間 小於 且 等於
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsSmallerAndEquallyNow(string compareTime, string nowTime)
		{
			if (IsValidDate(compareTime) && IsValidDate(nowTime))
				return IsSmallerAndEquallyNow(Convert.ToDateTime(compareTime), Convert.ToDateTime(nowTime));
			return false;
		}

		/// <summary>
		/// 比目前時間 小於 且 等於
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsSmallerAndEquallyNow(DateTime compareTime, DateTime nowTime)
		{
			bool isSmall = false;
			if (DateTime.Compare(compareTime, nowTime) <= 0)
				isSmall = true;
			return isSmall;
		}
		#endregion

		#region 跟目前時間一樣
		/// <summary>
		/// 跟目前時間一樣
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <returns>bool</returns>
		public bool IsSameAsNow(string compareTime)
		{
			if (IsValidDate(compareTime))
				return IsSameAsNow(Convert.ToDateTime(compareTime));
			return false;
		}

		/// <summary>
		/// 跟目前時間一樣
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <returns>bool</returns>
		public bool IsSameAsNow(DateTime compareTime)
		{
			return IsSameAsNow(compareTime, DateTime.Now);
		}

		/// <summary>
		/// 跟目前時間一樣
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsSameAsNow(string compareTime, string nowTime)
		{
			if (IsValidDate(compareTime) && IsValidDate(nowTime))
				return IsSameAsNow(Convert.ToDateTime(compareTime), Convert.ToDateTime(nowTime));
			return false;
		}

		/// <summary>
		/// 跟目前時間一樣
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsSameAsNow(DateTime compareTime, DateTime nowTime)
		{
			bool isSame = false;
			if (DateTime.Compare(compareTime, nowTime) == 0)
				isSame = true;
			return isSame;
		}
		#endregion

		#region (>= 目前時間) AND (<= 目前時間)
		/// <summary>
		/// (大於等於 目前時間) AND (小於等於 目前時間)
		/// </summary>
		/// <param name="startTime">開始時間</param>
		/// <param name="endTime">結束時間</param>
		/// <returns>bool</returns>
		public bool IsBetweenNow(string startTime, string endTime)
		{
			if (IsValidDate(startTime) && IsValidDate(endTime))
				return IsBetweenNow(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime));
			return false;
		}

		/// <summary>
		/// (大於等於 目前時間) AND (小於等於 目前時間)
		/// </summary>
		/// <param name="startTime">開始時間</param>
		/// <param name="endTime">結束時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsBetweenNow(string startTime, string endTime, string nowTime)
		{
			if (IsValidDate(startTime) && IsValidDate(endTime) && IsValidDate(nowTime))
				return IsBetweenNow(Convert.ToDateTime(startTime), Convert.ToDateTime(endTime), Convert.ToDateTime(nowTime));
			return false;
		}

		/// <summary>
		/// (大於等於 目前時間) AND (小於等於 目前時間)
		/// </summary>
		/// <param name="startTime">開始時間</param>
		/// <param name="endTime">結束時間</param>
		/// <returns>bool</returns>
		public bool IsBetweenNow(DateTime startTime, DateTime endTime)
		{
			return IsBetweenNow(startTime, endTime, DateTime.Now);
		}

		/// <summary>
		/// (大於等於 目前時間) AND (小於等於 目前時間)
		/// </summary>
		/// <param name="startTime">開始時間</param>
		/// <param name="endTime">結束時間</param>
		/// <param name="nowTime">目前的時間</param>
		/// <returns>bool</returns>
		public bool IsBetweenNow(DateTime startTime, DateTime endTime, DateTime nowTime)
		{
			bool isBetween = false;
			if (IsBiggerAndEquallyNow(nowTime, startTime) && IsSmallerAndEquallyNow(nowTime, endTime))
			{
				isBetween = true;
			}
			return isBetween;
		}
		#endregion

		#region 取得和目前系統時間差 DateDiff (系統時間 - 要比較的時間)
		/// <summary>
		/// 取得和目前系統時間差 DateDiff (系統時間 - 要比較的時間)
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="diffTicks">回傳Diff</param>
		/// <returns>double</returns>
		public double GetDateTimeDiff(string compareTime, DateTimeDiffTicks diffTicks)
		{
			if (!string.IsNullOrWhiteSpace(compareTime))
				return GetDateTimeDiff(DateTime.Now, Convert.ToDateTime(compareTime), diffTicks);
			else
				return 0;
		}

		/// <summary>
		/// 取得和目前系統時間差 DateDiff (系統時間 - 要比較的時間)
		/// </summary>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="diffTicks">回傳Diff</param>
		/// <returns>double</returns>
		public double GetDateTimeDiff(DateTime compareTime, DateTimeDiffTicks diffTicks)
		{
			return GetDateTimeDiff(DateTime.Now, compareTime, diffTicks);
		}

		/// <summary>
		/// 取得和目前系統時間差 DateDiff (系統時間 - 要比較的時間)
		/// </summary>
		/// <param name="nowTime">系統時間</param>
		/// <param name="compareTime">要比較的時間</param>
		/// <param name="diffTicks">回傳Diff</param>
		/// <returns>double</returns>
		/// <remarks>RayChien 2013.11.15 created</remarks>
		public double GetDateTimeDiff(DateTime nowTime, DateTime compareTime, DateTimeDiffTicks diffTicks)
		{
			double dblDiff = 0;
			try
			{
				if (diffTicks == DateTimeDiffTicks.TotalDays)
					dblDiff = new TimeSpan(nowTime.Ticks - compareTime.Ticks).TotalDays;
				else if (diffTicks == DateTimeDiffTicks.TotalHours)
					dblDiff = new TimeSpan(nowTime.Ticks - compareTime.Ticks).TotalHours;
				else if (diffTicks == DateTimeDiffTicks.TotalMinutes)
					dblDiff = new TimeSpan(nowTime.Ticks - compareTime.Ticks).TotalMinutes;
				else if (diffTicks == DateTimeDiffTicks.TotalSeconds)
					dblDiff = new TimeSpan(nowTime.Ticks - compareTime.Ticks).TotalSeconds;
				else if (diffTicks == DateTimeDiffTicks.TotalMilliseconds)
					dblDiff = new TimeSpan(nowTime.Ticks - compareTime.Ticks).TotalMilliseconds;
				else if (diffTicks == DateTimeDiffTicks.TotalYears)
				{
					dblDiff = (nowTime.Year - compareTime.Year - 1) +
						(((nowTime.Month > compareTime.Month) ||
						((nowTime.Month == compareTime.Month) && (nowTime.Day >= compareTime.Day))) ? 1 : 0);
				}
			}
			catch { }
			return dblDiff;
		}
		#endregion

		#endregion
	}
}
