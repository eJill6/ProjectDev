using System.Collections.Generic;
using JxBackendService.Common.Util;
using JxBackendService.Model.Param.Setting;

namespace JxBackendService.Model.ViewModel.Setting
{
	public class RefreshFrequencyInfo : SaveRefreshFrequencySettingParam
	{
		public int UserID { get; set; }
	}

	public class RefreshFrequencySettingViewModel
	{
		public int IntervalSeconds { get; set; }

		public List<TimeIntervalInfo> TimeIntervalInfos { get; set; }
	}

	public class TimeIntervalInfo
	{
		public int? IntervalSeconds { get; set; }

		public bool IsChecked { get; set; }

		public string DisplayName { get; set; }

		public bool IsCustomized => !RefElementId.IsNullOrEmpty();

		public string RefElementId { get; set; }
	}
}