using JxBackendService.Model.Common;
using JxBackendService.Model.Enums.MiseOrder;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Enums;
using ProductTransferService.AgDataBase.DLL;
using ProductTransferService.AgDataBase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestN6;

namespace UnitTestProject
{
    public class ParseAGXmlFileMockService : IParseAGXmlFileService
    {
        public bool ConvertToAGAndFishInfo(AGGameType agGameType, XMLFile xmlFile, out List<AGInfo> agInfos, out List<AgFishInfo> agFishInfos)
        {
            var tempAgInfos = new List<AGInfo>();
            var tempAgFishInfos = new List<AgFishInfo>();

            TestUtil.DoPlayerJobs(
                recordCount: 2000,
                job: (playerId, betId) =>
                {
                    string billNo = betId.ToString();
                    DateTime betTime = DateTime.Now.AddMinutes(-10);

                    tempAgInfos.Add(new AGInfo()
                    {
                        playerName = playerId,
                        billNo = billNo,
                        dataType = "EBR",
                        mainbillno = billNo,
                        beforeCredit = 40000.58m,
                        betAmount = 10070.89m,
                        validBetAmount = 50071.47m,
                        netAmount = 10070.89m,
                        betTime = betTime,
                        recalcuTime = betTime,
                        round = string.Empty,
                        gameType = "IN",
                        playType = "",
                        tableCode = "",
                        gameCode = "",
                        flag = "0",
                        platformType = "AGIN",
                        MiseOrderGameId = MiseOrderGameId.AGXin.SubGameCode,
                    });

                    string tradeNo = TestUtil.CreateId().ToString();

                    tempAgFishInfos.Add(new AgFishInfo()
                    {
                        playerName = playerId,
                        ProfitLossID = $"H_{billNo}",
                        tradeNo = tradeNo,
                        dataType = "HSR",
                        sceneId = tradeNo,
                        type = "1",
                        SceneStartTime = DateTime.Now.AddMinutes(-10),
                        SceneEndTime = DateTime.Now.AddMinutes(-10).AddSeconds(10),
                        Roomid = "1-26",
                        Roombet = "1",
                        Cost = 350.0000m,
                        Earn = 210.0000m,
                        Jackpotcomm = 1.2250m,
                        transferAmount = -138.775m,
                        previousAmount = 0.0m,
                        currentAmount = 0.0m,
                        currency = "CNY",
                        exchangeRate = "1",
                        IP = "",
                        flag = "0",
                        creationTime = DateTime.Now,
                        gameCode = "HMFP",
                        platformType = "HUNTER",
                        MiseOrderGameId = MiseOrderGameId.AGFishing.SubGameCode,
                    });
                });

            agInfos = tempAgInfos;
            agFishInfos = tempAgFishInfos;

            return true;
        }
    }
}