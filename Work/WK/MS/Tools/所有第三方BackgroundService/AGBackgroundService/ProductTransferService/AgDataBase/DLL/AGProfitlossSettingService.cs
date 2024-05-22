using JxBackendService.Model.Util;
using JxBackendService.Resource.Element;
using ProductTransferService.AgDataBase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JxBackendService.Common.Util;

namespace ProductTransferService.AgDataBase.DLL
{
    public interface IAGProfitlossSettingService
    {
        string[] NoRebateKeywords { get; }

        string CreateAGMemo(AGInfo agInfo);

        string CreateFishMemo(AgFishInfo agFishInfo);
    }

    public class AGProfitlossSettingService : IAGProfitlossSettingService
    {
        private static readonly string[] s_noRebateKeywords = new string[] { "免费", "抽奖", "奖励" };

        public string[] NoRebateKeywords => s_noRebateKeywords;

        public string CreateAGMemo(AGInfo agInfo)
        {
            string dbContentElement = typeof(DBContentElement).FullName;

            LocalizationParam localizationParam = new LocalizationParam()
            {
                SplitOperator = "，",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            if (agInfo.dataType == "EBR" && agInfo.validBetAmount == 0)
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_Game),
                });
            }

            if (!agInfo.PlatformTypeName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_PlatformTypeName),
                    Args = new List<string>() { agInfo.PlatformTypeName, },
                });
            }

            if (!agInfo.RoundName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_RoundName),
                    Args = new List<string>() { agInfo.RoundName, },
                });
            }

            if (!agInfo.GameTypeName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_GameTypeName),
                    Args = new List<string>() { agInfo.GameTypeName, },
                });
            }

            if (!agInfo.PlayTypeName.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_PlayTypeName),
                    Args = new List<string>() { agInfo.PlayTypeName, },
                });
            }

            if (!agInfo.tableCode.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_TableCode),
                    Args = new List<string>() { agInfo.tableCode, },
                });
            }

            if (!agInfo.gameCode.IsNullOrEmpty())
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgBetMemo_GameCode),
                    Args = new List<string>() { agInfo.gameCode, },
                });
            }

            return localizationParam.ToLocalizationJsonString();
        }

        public string CreateFishMemo(AgFishInfo agFishInfo)
        {
            string dbContentElement = typeof(DBContentElement).FullName;

            LocalizationParam localizationParam = new LocalizationParam()
            {
                SplitOperator = "，",
                LocalizationSentences = new List<LocalizationSentence>(),
            };

            if (agFishInfo.type == "1")
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgFishBetMemo1),
                    Args = new List<string>() {
                        agFishInfo.PlatformTypeName,
                        agFishInfo.transferAmount.ToString(),
                        agFishInfo.sceneId,
                        agFishInfo.SceneStartTime.ToFormatDateTimeString(),
                        agFishInfo.SceneEndTime.ToFormatDateTimeString(),
                        agFishInfo.Roomid,
                        agFishInfo.Roombet,
                        agFishInfo.Cost.ToString(),
                        agFishInfo.Earn.ToString(),
                    },
                });
            }
            else if (agFishInfo.type == "2")
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgFishBetMemo2),
                    Args = new List<string>() {
                        agFishInfo.PlatformTypeName,
                        agFishInfo.sceneId,
                        agFishInfo.transferAmount.ToString(),
                    },
                });
            }
            else if (agFishInfo.type == "7")
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgFishBetMemo7),
                    Args = new List<string>() {
                        agFishInfo.PlatformTypeName,
                        agFishInfo.transferAmount.ToString(),
                        agFishInfo.sceneId,
                        agFishInfo.SceneStartTime.ToFormatDateTimeString(),
                        agFishInfo.SceneEndTime.ToFormatDateTimeString(),
                        agFishInfo.Roomid,
                        agFishInfo.Roombet,
                    },
                });
            }
            else if (agFishInfo.type == "3")
            {
                localizationParam.LocalizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = dbContentElement,
                    ResourcePropertyName = nameof(DBContentElement.AgFishBetMemo3),
                });
            }

            return localizationParam.ToLocalizationJsonString();
        }
    }
}