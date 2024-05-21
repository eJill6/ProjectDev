using JxLottery.Models.Lottery.PlayTypeRadio.Baccarat;
using Constants = JxLottery.Services.BonusService.Calculators.Baccarat.Const;

namespace ControllerShareLib.Models.SpaPlayConfig
{
    /// <summary>
    /// 設定百家樂玩法
    /// </summary>
    public class BaccaratPlayConfig : ISpaPlayConfig
    {
        private IList<PlayMode<PlayTypeInfo>> playModes;

        public int GameTypeId => (int)JxLottery.Models.Lottery.GameTypeId.Baccarat;

        public IList<PlayMode<PlayTypeInfo>> Get(DateTime? closeDateTime)
        {
            if (playModes == null)
            {
                playModes = new List<PlayMode<PlayTypeInfo>>()
                {
                    new PlayMode<PlayTypeInfo>()
                    {
                        PlayModeId = 1,
                        PlayTypeInfos = new List<PlayTypeInfo>()
                        {
                            new PlayTypeInfo()
                            {
                                PlayTypeEnum = PlayType.YinXiangZongHe.ToString(),
                                BasePlayTypeId = (int)PlayType.YinXiangZongHe,
                                PlayTypeRadioInfos = new List<PlayTypeRadioInfo>()
                                {
                                    new PlayTypeRadioInfo()
                                    {
                                        PlayTypeRadioEnum = PlayTypeRadio.YinXiangZongHe.ToString(),
                                        BasePlayTypeRadioId = (int)PlayTypeRadio.YinXiangZongHe,
                                        ViewType = new Dictionary<string, string>()
                                        {
                                        },
                                        Fields = Constants.YinXiangZongHeCalculator.ManPeis.Select(x =>
                                        {
                                            List<(string Prompt, IList<(string Item, decimal Odds)> Numbers)> result = 
                                                new() { 
                                                    (x.Key, x.Value.Select(item => (item.Key, item.Value.Item1.LastOrDefault())).ToList()) 
                                                };
                                            return result;
                                        }).SelectMany(x => x, (list, item) =>
                                        {
                                            return new Field()
                                            {
                                                Prompt = item.Prompt,
                                                Numbers = item.Numbers.Select(x => x.Item).ToList<object>()
                                            };
                                        }).ToList()
                                    }
                                }
                            },
                        }
                    }
                };
            }

            return playModes;
        }
    }
}