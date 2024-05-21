using AutoMapper;
using JxBackendService.Handlers;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Param.BackSide;
using JxBackendService.Model.ViewModel.BackSide;
using Management.Web.Modules.Common.Helpers;
using Management.Web.Modules.SystemSettings.LotteryInfo;
using Management.Web.Service;
using Microsoft.AspNetCore.Mvc;
using Serenity.Data;
using Serenity.Reporting;
using Serenity.Services;
using Serenity.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using MyRow = Management.SystemSettings.LotteryInfoRow;

namespace Management.SystemSettings.Endpoints
{
    [Route("Services/SystemSettings/LotteryInfo/[action]")]
    [ConnectionKey(typeof(MyRow)), ServiceAuthorize(typeof(MyRow))]
    public class LotteryInfoController : ServiceEndpoint
    {
        private IApiService _apiService;
        public LotteryInfoController(IApiService apiService)
        {
            _apiService= apiService;
        }
       [HttpPost, AuthorizeCreate(typeof(MyRow))]
        public SaveResponse Create(IUnitOfWork uow, SaveRequest<MyRow> request,
            [FromServices] ILotteryInfoSaveHandler handler)
        {
            return handler.Create(uow, request);
        }

        public BaseResponse Update(LotteryInfoUpdateParam param)
        {
            BaseResponse response = new BaseResponse();
            var result = _apiService.UpdateLotteryInfo(param.items);
            if (string.IsNullOrEmpty(result.Message))
                response.Success = true;
            else
                response.Message=result.Message;
            return response;
        }
        public BaseResponse UpdateLotteryStatus(ShareModel model)
        {
            BaseResponse response = new BaseResponse();
            var request = MapTool.MapToModel<ShareModel, BackSideModel>(model);
            var result = _apiService.UpdateLotteryStatus(request);
            if (string.IsNullOrEmpty(result.Message))
                response.Success = true;
            else
                response.Message = result.Message;
            return response;
        }

        public BaseResponse UpdatePlayTypeStatus(ShareModel model)
        {
            BaseResponse response = new BaseResponse();
            var request = MapTool.MapToModel<ShareModel, BackSideModel>(model);
            var result = _apiService.UpdatePlayTypeStatus(request);
            if (string.IsNullOrEmpty(result.Message))
                response.Success = true;
            else
                response.Message = result.Message;
            return response;
        }

        [HttpPost, AuthorizeDelete(typeof(MyRow))]
        public DeleteResponse Delete(IUnitOfWork uow, DeleteRequest request,
            [FromServices] ILotteryInfoDeleteHandler handler)
        {
            return handler.Delete(uow, request);
        }

        [HttpPost]
        public RetrieveResponse<MyRow> Retrieve(IDbConnection connection, RetrieveRequest request,
            [FromServices] ILotteryInfoRetrieveHandler handler)
        {
            return handler.Retrieve(connection, request);
        }

        [HttpPost, AuthorizeList(typeof(MyRow))]
        public ListResponse<MyRow> List(ListRequest request)
        {
            ListResponse<MyRow> response = new ListResponse<MyRow>();            
            var data = _apiService.GetLotteryInfoDatas(new BackSideModel {Value= request.EqualityFilter["SortType"].ToString() }).DataModel;
            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<LotteryInfoResult, MyRow>());
            var mapper = config.CreateMapper();
            response.Entities = mapper.Map<List<MyRow>>(data);
            response.TotalCount = response.Entities.Count;
            return response;
            //return handler.List(connection, request);
        }
        [HttpPost, AuthorizeList(typeof(MyRow))]
        public ListResponse<PlayTypeInfo> GetPlayTypeInfo(ShareModel model)
        {
            ListResponse<PlayTypeInfo> response = new ListResponse<PlayTypeInfo>();
            var param = MapTool.MapToModel<ShareModel, BackSideModel>(model);
            var data = _apiService.GetPlayTypeInfo(param).DataModel;
            response.Entities = data;
            return response;
        }

        //[HttpPost, AuthorizeList(typeof(MyRow))]
        //public FileContentResult ListExcel(IDbConnection connection, ListRequest request,
        //    [FromServices] ILotteryInfoListHandler handler,
        //    [FromServices] IExcelExporter exporter)
        //{
        //    var data = List(connection, request, handler).Entities;
        //    var bytes = exporter.Export(data, typeof(Columns.LotteryInfoColumns), request.ExportColumns);
        //    return ExcelContentResult.Create(bytes, "LotteryInfoList_" +
        //        DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture) + ".xlsx");
        //}
    }
}