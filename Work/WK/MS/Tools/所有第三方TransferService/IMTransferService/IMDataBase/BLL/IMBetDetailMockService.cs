using IMDataBase.Enums;
using IMDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using System;
using System.Collections.Generic;

namespace IMDataBase.BLL
{
    public class IMBetDetailMockService : IIMBetDetailService
    {
        public BetLogResult<List<BetResult>> GetRemoteBetDetail(IMApiParamModel model)
        {
            switch (model.ProductCode)
            {
                case ProductCode.ESportsBulls:
                    return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;

                case ProductCode.EsportsVirtual:
                    return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;

                case ProductCode.PKTenLegends:
                    return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;

                default:
                    throw new NotSupportedException();
            }
        }

        private BetLogResult<List<BetResult>> GetSuccessResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.Success,
                Result = new List<BetResult>()
            };

            long betIdByTime = DateTime.Now.ToUnixOfTime();

            result.Result.Add(new BetResult()
            {
                //PlayerId = "jxD_69778",
                PlayerId = "ctsD_3",
                WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                StakeAmount = "100",
                WinLoss = "10",
                BetId = betIdByTime.ToString(),
                IsSettled = 1,
                IsCancelled = 0,
                OddsType = IMOneHandicap.EURO.Value,
                DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.5" } }
            });

            betIdByTime++;

            result.Result.Add(new BetResult()
            {
                //PlayerId = "jxD_69778",
                PlayerId = "ctsD_3",
                WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                StakeAmount = "100",
                WinLoss = "10",
                BetId = betIdByTime.ToString(),
                IsSettled = 1,
                IsCancelled = 0,
                OddsType = IMOneHandicap.EURO.Value,
                DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.65" } }
            });

            betIdByTime++;

            result.Result.Add(new BetResult()
            {
                //PlayerId = "jxD_69778",
                PlayerId = "ctsD_3",
                WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                StakeAmount = "100",
                WinLoss = "10",
                BetId = betIdByTime.ToString(),
                IsSettled = 1,
                IsCancelled = 0,
                OddsType = IMOneHandicap.EURO.Value,
                DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
            });

            betIdByTime++;

            result.Result.Add(new BetResult()
            {
                //PlayerId = "jxD_69778",
                PlayerId = "ctsD_3",
                WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                StakeAmount = "100",
                WinLoss = "-10",
                BetId = betIdByTime.ToString(),
                IsSettled = 1,
                IsCancelled = 0,
                OddsType = IMOneHandicap.EURO.Value,
                DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
            });

            betIdByTime++;

            result.Result.Add(new BetResult()
            {
                //PlayerId = "jxD_69778",
                PlayerId = "ctsD_3",
                WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                StakeAmount = "100",
                WinLoss = "0",
                BetId = betIdByTime.ToString(),
                IsSettled = 1,
                IsCancelled = 0,
                OddsType = IMOneHandicap.EURO.Value,
                DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
            });

            return result;
        }

        private BetLogResult<List<BetResult>> GetNoDataResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.NoDataFound,
                Result = new List<BetResult>()
            };

            return result;
        }

        private BetLogResult<List<BetResult>> GetFailedResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.SystemHasFailedYourRequest,
                Result = new List<BetResult>()
            };

            return result;
        }
    }
}