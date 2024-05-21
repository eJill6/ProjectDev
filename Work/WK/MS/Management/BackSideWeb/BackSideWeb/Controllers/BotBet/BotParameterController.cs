using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Models.Enums;
using BackSideWeb.Models.ViewModel;
using BackSideWeb.Models.ViewModel.BotBet;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.BotBet;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class BotParameterController : BaseCRUDController<BotBetParam, BotParameterInput, BotParameterInput>
    {
        private readonly string lastWarnMessage = "此为同组、同时间、同分类最后一笔，不可删除。";

        private const int maxAmount = 10000;

        private readonly Lazy<IBotParameterRep> _rep;

        public BotParameterController()
        {
            _rep = DependencyUtil.ResolveJxBackendService<IBotParameterRep>(EnvLoginUser, DbConnectionTypes.Master);
        }

        protected override string[] PageJavaScripts => new string[]
        {
          "business/botBet/botParameterSearchService.min.js"
        };

        protected override string ClientServiceName => "botParameterSearchService";

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.BotParameter;

        public override ActionResult Index()
        {
            var viewModel = new SettingInfoContextViewModel
            {
                BotGroupItems = MMSelectListItem.GetBotGroupItems(),
                TimeTypeItems = MMSelectListItem.GetEnumItems<TimeType>(),
                SettingGroupItems = MMSelectListItem.GetEnumItemsDefaultNull<SettingGroup>(SelectEnum.All),
            };
            return View(viewModel);
        }

        public override ActionResult GetGridViewResult(BotBetParam searchParam)
        {
            PagedResultModel<BotParameterViewModel> model = new PagedResultModel<BotParameterViewModel>();
            var result = _rep.Value.GetSettingInfoContext(searchParam);
            if (result != null)
            {
                model = MMHelpers.MapPagedResultModel<SettingInfoContext, BotParameterViewModel>(result);
                model = SetTimerValueText(model, searchParam.LotteryPatchType);
            }
            ViewBag.LotteryPatchType = searchParam.LotteryPatchType;
            return PartialView(model);
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.BotParameter;

        protected override IActionResult GetInsertView()
        {
            var viewModel = new BotParameterViewModel
            {
                LotteryPatchTypeItems = MMSelectListItem.GetEnumItems<LotteryPatchType>(),
                BotGroupItems = MMSelectListItem.GetEnumItems<BotGroup>(),
                TimeTypeItems = MMSelectListItem.GetEnumItems<TimeType>(),
                SettingGroupItems = MMSelectListItem.GetEnumItems<SettingGroup>(),
            };
            return GetEditView(viewModel);
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            string[] parmms = keyContent.Split(",");
            string id = parmms[0];
            string lotteryPatchTypeString = parmms[1];
            SettingInfoContext response = _rep.Value.GetSettingInfoContextDetail(Convert.ToInt16(id), Convert.ToInt16(lotteryPatchTypeString));
            var model = MMHelpers.MapModel<SettingInfoContext, BotParameterViewModel>(response);
            model.LotteryPatchType = Convert.ToInt16(lotteryPatchTypeString);
            model.LotteryPatchTypeItems = MMSelectListItem.GetEnumItems<LotteryPatchType>();
            model.BotGroupItems = MMSelectListItem.GetEnumItems<BotGroup>();
            model.TimeTypeItems = MMSelectListItem.GetEnumItems<TimeType>();
            model.SettingGroupItems = MMSelectListItem.GetEnumItems<SettingGroup>();
            model.OriginalLotteryPatchType = Convert.ToInt16(lotteryPatchTypeString);
            return GetEditView(model);
        }

        protected override BaseReturnModel DoInsert(BotParameterInput model)
        {
            string errorMsg = ValidateInput(model);
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);

            var result = _rep.Value.CreateSettingInfoContext(model);
            if (!result)
                return new BaseReturnModel(ReturnCode.SystemError);

            #region 日志记录

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                {
                    new RecordCompareParam
                        {
                            Title = "系列",
                            NewValue = model.LotteryPatchTypeText
                        },
                        new RecordCompareParam
                        {
                            Title = "组别",
                            NewValue = model.GroupIdText
                        },
                        new RecordCompareParam
                        {
                            Title = "时间区段",
                            NewValue = model.TimeTypeText
                        },
                         new RecordCompareParam
                        {
                            Title = "投注分类",
                            NewValue = model.SettingGroupIdText
                        },
                        new RecordCompareParam
                        {
                            Title = "值",
                            NewValue = model.Amount.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "权重",
                            NewValue = model.Rate.ToString()
                        }
                }, ActTypes.Insert);

            CreateOperationLog(string.Format("{0}: {1}", PermissionElement.Insert, compareContent), _permissionKey);

            #endregion 日志记录

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoUpdate(BotParameterInput model)
        {
            string errorMsg = ValidateInput(model);
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);

            SettingInfoContext response = _rep.Value.GetSettingInfoContextDetail(model.Id, model.OriginalLotteryPatchType);
            var source = MMHelpers.MapModel<SettingInfoContext, BotParameterViewModel>(response);
            source.LotteryPatchType = model.OriginalLotteryPatchType;

            if (source.LotteryPatchType == model.LotteryPatchType &&
               source.GroupId == model.GroupId &&
               source.TimeType == model.TimeType &&
               source.Rate == model.Rate &&
               source.SettingGroupId == model.SettingGroupId &&
               source.Amount == model.Amount)
                return new BaseReturnModel(ReturnCode.Success);

            if (model.LotteryPatchType != model.OriginalLotteryPatchType || model.SettingGroupId != response.SettingGroupId || model.GroupId != response.GroupId || model.TimeType != response.TimeType)
            {
                if (_rep.Value.IsLast(model))
                    return new BaseReturnModel(lastWarnMessage);
            }

            var result = _rep.Value.UpdateSettingInfoContext(model);
            if (!result)
                return new BaseReturnModel(ReturnCode.SystemError);

            #region 日志记录

            if (source != null)
            {
                string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                    new RecordCompareParam
                        {
                            Title = "编辑",
                            IsLogTitleValue = true
                        },
                        new RecordCompareParam
                        {
                            Title = "系列",
                            OriginValue = source.LotteryPatchTypeText,
                            NewValue = model.LotteryPatchTypeText
                        },
                        new RecordCompareParam
                        {
                            Title = "组别",
                            OriginValue = source.GroupIdText,
                            NewValue = model.GroupIdText
                        },
                        new RecordCompareParam
                        {
                            Title = "时间区段",
                            OriginValue = source.TimeTypeText,
                            NewValue = model.TimeTypeText
                        },
                         new RecordCompareParam
                        {
                            Title = "投注分类",
                            OriginValue = source.SettingGroupText,
                            NewValue = model.SettingGroupIdText
                        },
                        new RecordCompareParam
                        {
                            Title = "值",
                            OriginValue = source.Amount.ToString(),
                            NewValue = model.Amount.ToString()
                        },
                        new RecordCompareParam
                        {
                            Title = "权重",
                            OriginValue = source.Rate.ToString(),
                            NewValue = model.Rate.ToString()
                        }
                    }, ActTypes.Update);

                if (string.IsNullOrWhiteSpace(compareContent))
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                CreateOperationLog(compareContent, _permissionKey);
            }

            #endregion 日志记录

            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            string[] parmms = keyContent.Split(',');
            string id = parmms[0];
            string lotteryPatchType = parmms[1];

            if (_rep.Value.IsLast(new BotParameterInput { Id = Convert.ToInt32(id), OriginalLotteryPatchType = Convert.ToInt32(lotteryPatchType) }))
                return new BaseReturnModel(lastWarnMessage);
            SettingInfoContext response = _rep.Value.GetSettingInfoContextDetail(Convert.ToInt16(id), Convert.ToInt16(lotteryPatchType));
            var source = MMHelpers.MapModel<SettingInfoContext, BotParameterViewModel>(response);
            var result = _rep.Value.DeleteSettingInfoContext(keyContent);
            if (!result)
                return new BaseReturnModel(ReturnCode.SystemError);

            #region 日志记录

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                    {
                        new RecordCompareParam
                        {
                            Title = "系列",
                            NewValue = source.LotteryPatchTypeText
                        },
                        new RecordCompareParam
                        {
                            Title = "组别",
                            NewValue = source.GroupIdText
                        },
                        new RecordCompareParam
                        {
                            Title = "时间区段",
                            NewValue = source.TimeTypeText
                        },
                         new RecordCompareParam
                        {
                            Title = "投注分类",
                            NewValue = source.SettingGroupText
                        }
                    }, ActTypes.Delete);

            CreateOperationLog(string.Format("{0}: {1}", PermissionElement.Delete, compareContent), _permissionKey);

            #endregion 日志记录

            return new BaseReturnModel(ReturnCode.Success);
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        private PagedResultModel<BotParameterViewModel> SetTimerValueText(PagedResultModel<BotParameterViewModel> model, int lotteryPatchType)
        {
            if (lotteryPatchType == (int)LotteryPatchType.JS)
            {
                foreach (var m in model.ResultList)
                {
                    if (m.GroupId == (int)BotGroup.A)
                    {
                        if (m.TimeType == (int)TimeType.T1)
                        {
                            m.TimerValueText = "5";
                        }
                        else if (m.TimeType == (int)TimeType.T2)
                        {
                            m.TimerValueText = "15";
                        }
                        else if (m.TimeType == (int)TimeType.T3)
                        {
                            m.TimerValueText = "35";
                        }
                        else if (m.TimeType == (int)TimeType.T4)
                        {
                            m.TimerValueText = "45";
                        }
                    }
                    else if (m.GroupId == (int)BotGroup.B)
                    {
                        if (m.TimeType == (int)TimeType.T1)
                        {
                            m.TimerValueText = "5";
                        }
                        else if (m.TimeType == (int)TimeType.T2)
                        {
                            m.TimerValueText = "15";
                        }
                        else if (m.TimeType == (int)TimeType.T3)
                        {
                            m.TimerValueText = "35";
                        }
                        else if (m.TimeType == (int)TimeType.T4)
                        {
                            m.TimerValueText = "45";
                        }
                    }
                    else if (m.GroupId == (int)BotGroup.C)
                    {
                        if (m.TimeType == (int)TimeType.T1)
                        {
                            m.TimerValueText = "5";
                        }
                        else if (m.TimeType == (int)TimeType.T2)
                        {
                            m.TimerValueText = "35";
                        }
                    }
                }
            }
            else
            {
                foreach (var m in model.ResultList)
                {
                    if (m.GroupId == (int)BotGroup.A)
                    {
                        if (m.TimeType == (int)TimeType.T1)
                        {
                            m.TimerValueText = "5";
                        }
                        else if (m.TimeType == (int)TimeType.T2)
                        {
                            m.TimerValueText = "10";
                        }
                        else if (m.TimeType == (int)TimeType.T3)
                        {
                            m.TimerValueText = "30";
                        }
                        else if (m.TimeType == (int)TimeType.T4)
                        {
                            m.TimerValueText = "45";
                        }
                    }
                    else if (m.GroupId == (int)BotGroup.B)
                    {
                        if (m.TimeType == (int)TimeType.T1)
                        {
                            m.TimerValueText = "5";
                        }
                        else if (m.TimeType == (int)TimeType.T2)
                        {
                            m.TimerValueText = "30";
                        }
                        else if (m.TimeType == (int)TimeType.T3)
                        {
                            m.TimerValueText = "45";
                        }
                    }
                    else if (m.GroupId == (int)BotGroup.C)
                    {
                        if (m.TimeType == (int)TimeType.T1)
                        {
                            m.TimerValueText = "5";
                        }
                        else if (m.TimeType == (int)TimeType.T2)
                        {
                            m.TimerValueText = "30";
                        }
                    }
                }
            }
            return model;
        }

        public static string ValidateInput(BotParameterInput model)
        {
            if (model.Rate <= 0)
                return "权重需填入 > 0 之正整数";
            if (model.SettingGroupId == (int)SettingGroup.SmallAmount)
            {
                if (model.Amount < 2 || model.Amount > maxAmount)
                {
                    return $"【{SettingGroup.SmallAmount.GetEnumDescription()}】的值需填入 >= 2，<= {maxAmount}之正整数";
                }
            }
            else if (model.SettingGroupId == (int)SettingGroup.BigAmount)
            {
                if (model.Amount < 2550 || model.Amount > maxAmount)
                {
                    return $"【{SettingGroup.BigAmount.GetEnumDescription()}】的值需填入 >= 2550，<= {maxAmount}之正整数";
                }
            }
            else if (model.SettingGroupId == (int)SettingGroup.SmallCount)
            {
                if (model.Amount < 0)
                {
                    return $"【{SettingGroup.SmallCount.GetEnumDescription()}】的值需填入 >=0之正整数";
                }
            }
            else if (model.SettingGroupId == (int)SettingGroup.BigCount)
            {
                if (model.Amount < 0)
                {
                    return $"【{SettingGroup.BigCount.GetEnumDescription()}】的值需填入 >=0之正整数";
                }
            }

            return string.Empty;
        }
    }
}