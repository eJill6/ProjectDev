using IdGen;
using IMBGDataBase.Merchant;
using IMDataBase.Common;
using IMDataBase.Enums;
using IMDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using UnitTestN6;

namespace UnitTestProject
{
    public class IMBetDetailMSLMockService : IMBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMApiParamModel apiParam)
        {
            //return MockDataUtil.GetFromTpBetDetailApiResult(apiParam);
            return MockDataUtil.GetMockRemoteBetDetailApiResult(apiParam);
        }
    }

    public class MockDataUtil
    {
        public static BetLogResponseInfo GetMockRemoteBetDetailApiResult(IMApiParamModel apiParam)
        {
            if (apiParam.ProductCode == IMProductCode.ESportsBulls)
            {
                return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;
            }
            else if (apiParam.ProductCode == IMProductCode.EsportsVirtual)
            {
                return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;
            }
            else if (apiParam.ProductCode == IMProductCode.PKTenLegends)
            {
                return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;
            }

            throw new NotSupportedException();
        }

        private static BetLogResponseInfo GetSuccessResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.Success,
                Result = new List<BetResult>()
            };

            TestUtil.DoPlayerJobs(                
                recordCount: 2000,
                job: (playerId, betId) =>
                {
                    result.Result.Add(new BetResult()
                    {
                        PlayerId = playerId,
                        WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                        SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                        StakeAmount = "100",
                        WinLoss = "10",
                        BetId = CreateBetId(),
                        IsSettled = 1,
                        IsCancelled = 0,
                        OddsType = IMOneHandicap.EURO.Value,
                        DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.5" } }
                    });

                    result.Result.Add(new BetResult()
                    {
                        PlayerId = playerId,
                        WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                        SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                        StakeAmount = "100",
                        WinLoss = "10",
                        BetId = CreateBetId(),
                        IsSettled = 1,
                        IsCancelled = 0,
                        OddsType = IMOneHandicap.EURO.Value,
                        DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.65" } }
                    });

                    result.Result.Add(new BetResult()
                    {
                        PlayerId = playerId,
                        WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                        SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                        StakeAmount = "100",
                        WinLoss = "10",
                        BetId = CreateBetId(),
                        IsSettled = 1,
                        IsCancelled = 0,
                        OddsType = IMOneHandicap.EURO.Value,
                        DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
                    });

                    result.Result.Add(new BetResult()
                    {
                        PlayerId = playerId,
                        WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                        SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                        StakeAmount = "100",
                        WinLoss = "-10",
                        BetId = CreateBetId(),
                        IsSettled = 1,
                        IsCancelled = 0,
                        OddsType = IMOneHandicap.EURO.Value,
                        DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
                    });

                    result.Result.Add(new BetResult()
                    {
                        PlayerId = playerId,
                        WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                        SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                        StakeAmount = "100",
                        WinLoss = "0",
                        BetId = CreateBetId(),
                        IsSettled = 1,
                        IsCancelled = 0,
                        OddsType = IMOneHandicap.EURO.Value,
                        DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
                    });
                });

            return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
        }

        private static string CreateBetId() => TestUtil.CreateId().ToString();

        private static BetLogResult<List<BetResult>> GetNoDataResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.NoDataFound,
                Result = new List<BetResult>()
            };

            return result;
        }

        private static BetLogResult<List<BetResult>> GetFailedResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.SystemHasFailedYourRequest,
                Result = new List<BetResult>()
            };

            return result;
        }

        public static BetLogResponseInfo GetFromTpBetDetailApiResult(IMApiParamModel apiParam)
        {
            var betLogResult = new BetLogResult<List<BetResult>>();
            string apiResult = ApiClient.GetBetLog(apiParam);

            if (!string.IsNullOrEmpty(apiResult))
            {
                betLogResult = apiResult.Deserialize<BetLogResult<List<BetResult>>>();
                //No Data Found
                if (betLogResult.Code == (int)APIErrorCode.NoDataFound)
                {
                    return new BetLogResponseInfo() { ApiResult = betLogResult.ToJsonString() };
                }

                // 大於1頁5000筆
                if (betLogResult.Pagination != null && betLogResult.Pagination.TotalPage > 1)
                {
                    for (int i = 2; i <= betLogResult.Pagination.TotalPage; i++)
                    {
                        apiParam.Page = i;
                        apiResult = ApiClient.GetBetLog(apiParam);
                        BetLogResult<List<BetResult>> pagedResult = apiResult.Deserialize<BetLogResult<List<BetResult>>>();
                        betLogResult.Result.AddRange(pagedResult.Result);
                    }
                }
            }

            return new BetLogResponseInfo() { ApiResult = betLogResult.ToJsonString() };
        }
    }
}