using BackSideWeb.Controllers.Base;
using BackSideWeb.Helpers;
using BackSideWeb.Model.Entity.MM;
using BackSideWeb.Model.Param.MM;
using BackSideWeb.Model.ViewModel.MM;
using BackSideWeb.Models.ViewModel;
using Castle.Core.Internal;
using JxBackendService.Common.Extensions;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.BackSideWeb.Permission;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace BackSideWeb.Controllers
{
    public class BannerController : BaseCRUDController<QueryBannerParam, BannerInputModel, BannerInputModel>
    {
        protected override string[] PageJavaScripts => new string[]
{
            "business/publicitySettings/bannerSearchService.min.js"
};

        protected override string ClientServiceName => "bannerSearchService";
        protected override string ClientEditSingleRowServiceName => "bannerEditSingleRowService"; //為了檔案上傳需要重寫蒐集formData

        private readonly PermissionKeyDetail _permissionKey = PermissionKeyDetail.Banner;

        public override ActionResult GetGridViewResult(QueryBannerParam searchParam)
        {
            var result = GetResult().Datas.Where(r => r.LocationType == (int)searchParam.LocationType).OrderBy(a => Convert.ToInt32(a.Sort)).ThenByDescending(a => a.CreateDate).ToList();
            PagedResultModel<QueryBannerModel> model = new PagedResultModel<QueryBannerModel>();
            model.ResultList = result;
            model.PageSize = searchParam.PageSize;
            model.TotalCount = model.ResultList.Count();
            model.PageNo = searchParam.PageNo;
            return PartialView(model);
        }

        public ApiResult<QueryBannerModel>? GetResult()
        {
            var result = MMClientApi.GetApi<QueryBannerModel>("Banner", "List");
            return result;
        }

        protected override PermissionKeys GetPermissionKey() => PermissionKeys.Banner;

        protected override IActionResult GetInsertView()
        {
            int count = 0;
            try
            {
                count = GetResult().Datas.Count;
            }
            catch (Exception ex)
            {
                count = 0;
            }
            //if (count >= 10)
            //{
            //    return this.Content("Banner已达上限");
            //}
            BannerInputModel model = new BannerInputModel
            {
                MMBannerBs = new MMBannerBs
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                },
                IsActiveListItem = MMSelectListItem.GetIsActiveItems(),
                LinkTypeItem = MMSelectListItem.GetLinkTypeItems(),
                AreaTypeItem = MMSelectListItem.GetAreaTypeItems(),
                GameTypeItem = MMSelectListItem.GetGameTypeItems(),
                InsideTypeItem = MMSelectListItem.GetInsideTypeItems(),
                LocationTypeItem = MMSelectListItem.GetLocationTypeItems(),
            };
            return GetEditView(model);
        }

        protected override IActionResult GetUpdateView(string keyContent)
        {
            var response = MMClientApi.GetSingleApi<MMBannerBs>("Banner", "Detail", keyContent);
            var media = response.Datas.Media;
            BannerInputModel viewModel = new BannerInputModel
            {
                IsActiveListItem = MMSelectListItem.GetIsActiveItems(),
                MMBannerBs = response.Datas,
                IsActiveSelected = Convert.ToInt16(response.Datas.IsActive),
                ShowFileName = !string.IsNullOrEmpty(media.FullMediaUrl) ? Path.GetFileName(media.FullMediaUrl) : string.Empty,
                LinkTypeItem = MMSelectListItem.GetLinkTypeItems(),
                LinkTypeSelected = response.Datas.LinkType,
                AreaTypeItem = MMSelectListItem.GetAreaTypeItems(),
                GameTypeItem = MMSelectListItem.GetGameTypeItems(),
                InsideTypeItem = MMSelectListItem.GetInsideTypeItems(),
                LocationTypeItem = MMSelectListItem.GetLocationTypeItems(),
            };
            return GetEditView(viewModel);
        }

        protected override BaseReturnModel DoInsert(BannerInputModel model)
        {
            string errorMsg = ValidateInput(model, "Insert");
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);
            MMBannerBs bs = ConvertToMMBannerBs(model, "Insert");
            var result = MMClientApi.PostApi("Banner", "Create", bs);
            string errorMessage = GetErrorMessageFromResult(result);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new BaseReturnModel(errorMessage);
            }

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Insert,
                    IsLogTitleValue = true
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.Banner,
                    OriginValue = model.MMBannerBs.Title,
                    IsLogTitleValue = true
                },
            }, ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            CreateOperationLog(compareContent, _permissionKey);

            return new BaseReturnModel();
        }

        private MMBannerBs ConvertToMMBannerBs(BannerInputModel model, string type)
        {
            var file = model.File;
            var banner = model.MMBannerBs;

            var bs = new MMBannerBs
            {
                Id = type == "Update" ? banner.Id : string.Empty,
                Title = banner.Title,
                Sort = banner.Sort,
                CreateDate = type == "Update" ? banner.CreateDate : DateTime.Now,
                StartDate = banner.StartDate,
                EndDate = banner.EndDate,
                ModifyDate = type == "Update" ? DateTime.Now : null,
                CreateUser = type == "Update" ? banner.CreateUser : ((JxBackendService.Model.Param.User.BackSideWebUser)EnvLoginUser.LoginUser).UserName,
                ModifyUser = type == "Update" ? ((JxBackendService.Model.Param.User.BackSideWebUser)EnvLoginUser.LoginUser).UserName : string.Empty,
                IsActive = Convert.ToBoolean(model.IsActiveSelected),
                Type = 0,
                LinkType = Convert.ToByte(model.LinkTypeSelected),
                RedirectUrl = banner.RedirectUrl ?? string.Empty,
                Media = null,
                LocationType = Convert.ToByte(banner.LocationType)
            };

            // file不为null代表有新增或编辑檔案
            if (file != null)
            {
                bs.Media = new Media
                {
                    Id = type == "Update" ? banner.Media.Id : string.Empty,
                    FileUrl = string.Empty,
                    MediaType = 0,
                    SourceType = 0,
                    RefId = type == "Update" ? banner.Media.RefId : string.Empty,
                    CreateDate = DateTime.Now,
                    ModifyDate = type == "Update" ? DateTime.Now : null,
                    FullMediaUrl = type == "Update" ? banner.Media.FullMediaUrl : string.Empty,
                    Bytes = file.ToBytes(),
                    FileName = file.FileName
                };
            }

            return bs;
        }

        protected override BaseReturnModel DoUpdate(BannerInputModel model)
        {
            string errorMsg = ValidateInput(model, "Update");
            if (!string.IsNullOrEmpty(errorMsg))
                return new BaseReturnModel(errorMsg);
            //获取没修改前数据
            var source = MMClientApi.GetSingleApi<MMBannerBs>("Banner", "Detail", model.MMBannerBs.Id);

            MMBannerBs bs = ConvertToMMBannerBs(model, "Update");
            var result = MMClientApi.PostApi("Banner", "Update", bs);
            string errorMessage = GetErrorMessageFromResult(result);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new BaseReturnModel(errorMessage);
            }

            if (source != null)
            {
                string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
                {
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Banner,
                        OriginValue = source.Datas?.Title,
                        IsLogTitleValue = true
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.StartTime,
                        OriginValue = source.Datas?.StartDateText,
                        NewValue = model.MMBannerBs.StartDateText
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.EndTime,
                        OriginValue = source.Datas?.EndDateText,
                        NewValue = model.MMBannerBs.EndDateText
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Title,
                        OriginValue = source.Datas?.Title,
                        NewValue = model.MMBannerBs.Title
                    },
                   new RecordCompareParam
                    {
                       Title = DisplayElement.File,
                       OriginValue = !string.IsNullOrEmpty(source.Datas?.Media.FullMediaUrl) ? Path.GetFileName(source.Datas?.Media.FullMediaUrl) : string.Empty,
                       NewValue = model.ShowFileName
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.RedirectURL,
                        OriginValue = source.Datas?.RedirectUrl,
                        NewValue = model.MMBannerBs.RedirectUrl
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Sort,
                        OriginValue = source.Datas?.Sort,
                        NewValue = model.MMBannerBs.Sort
                    },
                    new RecordCompareParam
                    {
                        Title = DisplayElement.Show,
                        OriginValue = source.Datas?.IsActive == true ? "显示" : "隐藏",
                        NewValue =  model.IsActiveSelected == 1 ? "显示" : "隐藏",
                    },
                }, ActTypes.Update);

                if (compareContent.IsNullOrEmpty())
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }
                CreateOperationLog(compareContent, _permissionKey);
            }

            return new BaseReturnModel();
        }

        private string GetErrorMessageFromResult(ApiResult<MMBannerBs> result)
        {
            if (result.IsSuccess == false)
            {
                //if (result.Code == "E10003")
                //{
                //    return "排序不可重复";
                //}else
                if (result.Code == "E00002")
                {
                    return "無效的參數";
                }
                else if (result.Code == "E10001")
                {
                    return "開啟後結束時間不得小於現在時間";
                }
                else
                {
                    return $"失敗。{result.Message}";
                }
            }

            return string.Empty;
        }

        protected override BaseReturnModel DoDelete(string keyContent)
        {
            var source = MMClientApi.GetSingleApi<MMBannerBs>("Banner", "Detail", keyContent);

            var result = MMClientApi.PostApiWithParams("Banner", "Delete", new Dictionary<string, string>
            {
                {"seqId",keyContent }
            });

            string compareContent = GetOperationCompareContent(new List<RecordCompareParam>
            {
                new RecordCompareParam
                {
                    Title = DisplayElement.Delete,
                    IsLogTitleValue = true
                },
                new RecordCompareParam
                {
                    Title = DisplayElement.Banner,
                    OriginValue = source.Datas?.Title,
                    IsLogTitleValue = true
                },
            }, ActTypes.Update);

            if (compareContent.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            CreateOperationLog(compareContent, _permissionKey);
            return new BaseReturnModel(result.Message);
        }

        private IActionResult GetEditView<T>(T model)
        {
            return View("Edit", model);
        }

        public static string ValidateInput(BannerInputModel model, string type)
        {
            string result = string.Empty;
            var locationType = model.MMBannerBs.LocationType;
            var startDate = model.MMBannerBs.StartDate;
            var endDate = model.MMBannerBs.EndDate;
            var title = model.MMBannerBs.Title;
            var file = model.File;
            var sort = model.MMBannerBs.Sort;
            var redirectUrl = model.MMBannerBs.RedirectUrl;
            if (locationType <= 0 || locationType == null)
                return "请选择前台位置";
            //if (startDate == null)
            //    return "开始时间不可为空";
            //if (endDate == null)
            //    return "结束时间不可为空";
            //if (!MMValidate.IsCorrectStartDateToEndDate(startDate, endDate))
            //    return "结束时间小於开始时间请修正";
            if (string.IsNullOrEmpty(title))
                return "标题不可为空";
            if (!MMValidate.IsChineseOrEnglishOrNumber(title) || title.Length > 20)
                return "标题仅允许中、英、数最多20字";
            if (type == "Insert")
            {
                return ValidateImageFile(file, locationType.Value);
            }

            if (type == "Update")
            {
                if (file == null && string.IsNullOrWhiteSpace(model.ShowFileName))
                {
                    return "檔案不可为空";
                }
                if (file != null)
                {
                    return ValidateImageFile(file, locationType.Value);
                }
            }

            if (sort == null)
                return "排序不可为空";
            if (!MMValidate.IsExistIntRange(sort, 1, 9999))
                return "排序仅允许数字 1 ~ 9999";
            //if (string.IsNullOrEmpty(redirectUrl))
            //    return "转导网址不可为空";
            return result;
        }

        private static string ValidateImageFile(IFormFile file, int locationType)
        {
            if (file == null)
                return "檔案不可为空";

            if (!MMValidate.IsImageFileValid(file))
                return "檔案仅允许 jpg 或 png 格式";

            if (file.Length > (1 * 1024 * 1024))
            {
                return "档案大小1MB以下。";
            }

            if (locationType == 1 && !IsImageSizeValid(file, 68, 60))
            {
                return "避免变形，请制作 68*60 固定 2倍或以上比例的图片。档案格式仅允许 png。档案大小1MB以下。";
            }

            if (locationType == 2 && !IsImageSizeValid(file, 355, 112))
            {
                return "避免变形，请制作 355*112 或同比例的图片。档案格式允许 jpg 或 png。档案大小1MB以下。";
            }

            if (locationType == 3 && !IsImageSizeValid(file, 355, 150))
            {
                return "避免变形，请制作 355*150 或同比例的图片。档案格式允许 jpg 或 png。档案大小1MB以下。";
            }

            return string.Empty; // 表示验证通过
        }

        private static bool IsImageSizeValid(IFormFile file, int maxWidth, int maxHeight)
        {
            using (var imageStream = file.OpenReadStream())
            using (var image = Image.Load(imageStream))
            {
                int width = image.Width;
                int height = image.Height;

                // 计算宽度和高度是否为指定 maxWidth 和 maxHeight 的倍数
                bool isWidthValid = width % maxWidth == 0;
                bool isHeightValid = height % maxHeight == 0;

                return isWidthValid && isHeightValid;
            }
        }
    }
}