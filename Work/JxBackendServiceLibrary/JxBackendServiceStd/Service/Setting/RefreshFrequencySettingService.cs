using System.Collections.Generic;
using System.Linq;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Setting;
using JxBackendService.Interface.Service.Setting;
using JxBackendService.Model.Entity.Setting;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Setting;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Setting;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.Setting
{
	public class RefreshFrequencySettingService : BaseService, IRefreshFrequencySettingService
	{
		private readonly IRefreshFrequencySettingRep _refreshFrequencySettingRep;

		public RefreshFrequencySettingService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
		{
			_refreshFrequencySettingRep = ResolveJxBackendService<IRefreshFrequencySettingRep>();
		}

		public BaseReturnModel SaveRefreshFrequencySetting(SaveRefreshFrequencySettingParam settingParm)
		{
			int intervalMinutes = settingParm.IntervalSeconds / 60;

			if (intervalMinutes < 1 || intervalMinutes > 99)
			{
				return new BaseReturnModel(ReturnCode.SomeDataTypeFormatFail, CommonElement.TimeInterval);
			}

			int bwUserId = EnvLoginUser.LoginUser.UserId;

			RefreshFrequencySetting source = GetRefreshFrequencySettingByUser(settingParm.PermissionKey);

			var target = settingParm.CastByJson<RefreshFrequencySetting>();
			target.UserID = bwUserId;

			if (source == null)
			{
				return _refreshFrequencySettingRep.CreateByProcedure(target).CastByJson<BaseReturnModel>();
			}

			bool isSuccess = _refreshFrequencySettingRep.UpdateByProcedure(target);

			if (!isSuccess)
			{
				return new BaseReturnModel(ReturnCode.UpdateFailed);
			}

			return new BaseReturnModel(ReturnCode.Success);
		}

		public RefreshFrequencySettingViewModel GetBWUserRefreshFrequencySettingInfo(string permissionKey)
		{
			var settingViewModel = new RefreshFrequencySettingViewModel()
			{
				TimeIntervalInfos = CreateInitTimeIntervalInfo()
			};

			RefreshFrequencyInfo source = GetRefreshFrequencyInfo(permissionKey);

			if (source == null)
			{
				settingViewModel.TimeIntervalInfos.First().IsChecked = true;
				settingViewModel.IntervalSeconds = 60;

				return settingViewModel;
			}

			settingViewModel.IntervalSeconds = source.IntervalSeconds;
			TimeIntervalInfo foundInfo = settingViewModel.TimeIntervalInfos.SingleOrDefault(w => w.IntervalSeconds == source.IntervalSeconds);

			if (foundInfo != null)
			{
				foundInfo.IsChecked = true;
			}
			else
			{
				TimeIntervalInfo customizedInfo = settingViewModel.TimeIntervalInfos.Where(w => w.IsCustomized).First();
				customizedInfo.IsChecked = true;
				customizedInfo.IntervalSeconds = source.IntervalSeconds;
			}

			return settingViewModel;
		}

		public RefreshFrequencyInfo GetRefreshFrequencyInfo(string permissionKey)
		{
			return GetRefreshFrequencySettingByUser(permissionKey).CastByJson<RefreshFrequencyInfo>();
		}

		private RefreshFrequencySetting GetRefreshFrequencySettingByUser(string permissionKey)
		{
			int bwUserId = EnvLoginUser.LoginUser.UserId;

			return _refreshFrequencySettingRep.GetSingleByKey(InlodbType.Inlodb,
				new RefreshFrequencySetting
				{
					UserID = bwUserId,
					PermissionKey = permissionKey
				});
		}

		private List<TimeIntervalInfo> CreateInitTimeIntervalInfo()
			=> new List<TimeIntervalInfo>
			{
				 new TimeIntervalInfo { IntervalSeconds = (60 * 1), DisplayName = "1 分钟" },
				 new TimeIntervalInfo { IntervalSeconds = (60 * 10), DisplayName = "10 分钟" },
				 new TimeIntervalInfo { IntervalSeconds = (60 * 3), DisplayName = "3 分钟" },
				 new TimeIntervalInfo { IntervalSeconds = (60 * 15), DisplayName = "15 分钟" },
				 new TimeIntervalInfo { IntervalSeconds = (60 * 5), DisplayName = "5 分钟" },
				 new TimeIntervalInfo { DisplayName = "分钟", RefElementId = "jqCustomizedInterval" },
			};
	}
}