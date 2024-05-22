using JxBackendService.Common.Util;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using JxBackendService.Service.ThirdPartyTransfer;
using ProductTransferService.SportDataBase.Model;
using System;

namespace ProductTransferService.SportDataBase.Merchant
{
    public class SportBetDetailMSLService : SportBetDetailMerchantService
    {
        public SportBetDetailMSLService()
        {
        }

        protected override bool IsWriteRemoteContentToOtherMerchant => false;

        protected override long GetBackupBetLogFileSeq()
        {
            //不用複製投注資料
            throw new NotImplementedException();
        }

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(SportApiParamModel apiParam)
        {
            string lastVersionKey = apiParam.LastVersionKey;

            if (!long.TryParse(lastVersionKey, out long lastFileSeq)) //這邊用時間戳記
            {
                return null;
            }

            RequestAndResponse requestAndResponse = BetLogFileService.GetBetLogContent(Product, lastFileSeq);

            if (requestAndResponse == null || requestAndResponse.RequestBody.IsNullOrEmpty())
            {
                RemoteFileSetting.HasNewRemoteFile = false;

                return new BetLogResponseInfo() { ApiResult = CreateEmptyResult().ToJsonString() }; //避免後續發生null ref error
            }

            RemoteFileSetting.HasNewRemoteFile = true;

            return new BetLogResponseInfo()
            {
                ApiResult = requestAndResponse.ResponseContent,
                RemoteFileSeq = requestAndResponse.RequestBody
            };
        }

        private ApiResult<BetResult> CreateEmptyResult()
        {
            var apiBetResult = new ApiResult<BetResult>
            {
                Data = new BetResult()
                {
                    last_version_key = "0"
                }
            };

            return apiBetResult;
        }
    }
}