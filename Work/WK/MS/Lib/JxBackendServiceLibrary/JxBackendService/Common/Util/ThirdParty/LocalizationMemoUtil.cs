using JxBackendService.Model.Enums;
using JxBackendService.Model.Util;
using JxBackendService.Resource.Element;
using System.Collections.Generic;

namespace JxBackendService.Common.Util.ThirdParty
{
    public static class LocalizationMemoUtil
    {
        public static LocalizationParam CreateLocalizationParam(WagerType wagerType, PlatformHandicap platformHandicap, string allDetailContent, string playId)
        {
            var localizationSentences = new List<LocalizationSentence>();

            var localizationParam = new LocalizationParam
            {
                SplitOperator = ",",
                LocalizationSentences = localizationSentences
            };

            if (wagerType == WagerType.Single)
            {
                localizationSentences.Add(new LocalizationSentence()
                {
                    ResourcePropertyName = nameof(DBContentElement.WagerSingle)
                });
            }
            else
            {
                localizationSentences.Add(new LocalizationSentence()
                {
                    ResourcePropertyName = nameof(DBContentElement.WagerCombo)
                });
            }

            if (platformHandicap != null)
            {
                localizationSentences.Add(new LocalizationSentence()
                {
                    ResourceName = typeof(ThirdPartyGameElement).FullName,
                    ResourcePropertyName = nameof(ThirdPartyGameElement.SomeHandicap),
                    Args = new List<string>() { platformHandicap.Name }
                });
            }

            var args = new List<string>
            {
                allDetailContent.ToString(),
                playId
            };

            localizationSentences.Add(new LocalizationSentence()
            {
                ResourceName = typeof(ThirdPartyGameElement).FullName,
                ResourcePropertyName = nameof(ThirdPartyGameElement.CommonSportMemoContent),
                Args = args
            });

            return localizationParam;
        }
    }
}