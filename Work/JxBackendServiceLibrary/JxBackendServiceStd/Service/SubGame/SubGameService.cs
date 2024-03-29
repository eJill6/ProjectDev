﻿using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Net;

namespace JxBackendService.Service.SubGame
{
    public abstract class SubGameService : BaseService, ISubGameService
    {
        private readonly IGameLobbyListService _imGameListService;

        private readonly IGameAppSettingService _gameAppSettingService;

        protected IGameAppSettingService GameAppSettingService => _gameAppSettingService;

        protected abstract GameLobbyType GameLobbyType { get; }

        public SubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _gameAppSettingService = DependencyUtil.ResolveKeyed<IGameAppSettingService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _imGameListService = ResolveJxBackendService<IGameLobbyListService>();
        }

        public List<GameLobbyInfo> GetVisibleGameList()
        {
            return _imGameListService.GetActiveGameLobbyList(GameLobbyType);
        }
    }

    public abstract class IMOneSubGameService : SubGameService, IIMOneSubGameService
    {
        private static readonly string s_jackpotListRelativeUrl = "Casino/GetJackpotList";

        protected IMOneSubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected abstract string ConvertToAmount(string response);

        protected abstract IIMOneAppSetting GetProductAppSetting();

        public string GetJackpotAmount()
        {
            string response = GetRemoteJackpotListResult(GetProductAppSetting());

            return ConvertToAmount(response);
        }

        private string GetRemoteJackpotListResult(IIMOneAppSetting appSetting)
        {
            var requestModel = new IMJackpotListRequestModel
            {
                ProductWallet = appSetting.ProductWallet,
                MerchantCode = appSetting.MerchantCode,
            };

            string fullUrl = new Uri(new Uri(appSetting.ServiceUrl), s_jackpotListRelativeUrl).ToString();
            BaseReturnDataModel<string> returnDataModel = DoPostRequest(fullUrl, requestModel.ToJsonString());

            if (!returnDataModel.IsSuccess)
            {
                return 0.ToString();
            }

            return returnDataModel.DataModel;
        }

        private BaseReturnDataModel<string> DoPostRequest(string fullUrl, string requestBody)
        {
            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();

            string apiResult = httpWebRequestUtilService.GetResponse(
                new WebRequestParam()
                {
                    Purpose = $"SubGameService請求",
                    Method = HttpMethod.Post,
                    Url = fullUrl,
                    Body = requestBody,
                    ContentType = HttpWebRequestContentType.Json,
                    IsResponseValidJson = true
                },
            out HttpStatusCode httpStatusCode);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }

        private BaseReturnDataModel<string> ConverToApiReturnDataModel(HttpStatusCode httpStatusCode, string apiResult)
        {
            ReturnCode returnCode = ReturnCode.Success;

            if (httpStatusCode == HttpStatusCode.OK)
            {
                return new BaseReturnDataModel<string>(returnCode, apiResult);
            }

            returnCode = ReturnCode.HttpStatusCodeNotOK;
            var messageArgs = new string[] { ((int)httpStatusCode).ToString() };

            var returnDataModel = new BaseReturnDataModel<string>(returnCode, messageArgs)
            {
                DataModel = apiResult
            };

            return returnDataModel;
        }
    }
}