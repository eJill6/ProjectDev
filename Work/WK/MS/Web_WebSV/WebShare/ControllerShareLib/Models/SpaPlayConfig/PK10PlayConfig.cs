using JxLottery.Models.Lottery.PlayTypeRadio.PK10;
using Constants = JxLottery.Services.BonusService.Calculators.PK10.Const;

namespace ControllerShareLib.Models.SpaPlayConfig
{
    /// <summary>
    /// PK10玩法設定
    /// </summary>
    public class PK10PlayConfig : ISpaPlayConfig
    {
        private IList<PlayMode<PlayTypeInfo>> playModes;

        /// <inheritdoc cref="ISpaPlayConfig.GameTypeId"/>
        public int GameTypeId => (int)JxLottery.Models.Lottery.GameTypeId.PK10;

        private static readonly IDictionary<string, int> _skipCount
        = new Dictionary<string, int>()
        {
            { "冠军与两面", 4 },
            { "冠亚和", 6 },
            { "冠军特殊", 4 },
            { "冠军", 5 },
        };

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
                                                var result = new List<Tuple<string, IList<Tuple<string, decimal>>>>();
                                                if (_skipCount.ContainsKey(x.Key))
                                                {
                                                    var rowCount = x.Value.Count / _skipCount[x.Key];
                                                    if (x.Value.Count % _skipCount[x.Key] > 0)
                                                    {
                                                        rowCount++;
                                                    }
                                                    var skip = 0;
                                                    for(var count = 0; count < rowCount; count++)
                                                    {
                                                        result.Add(new Tuple<string, IList<Tuple<string, decimal>>>(x.Key, x.Value.Skip(skip).Take(_skipCount[x.Key]).Select(item => new Tuple<string, decimal>(item.Key, item.Value.Item1)).ToList()));
                                                        skip += _skipCount[x.Key];
                                                    }
                                                }
                                                else
                                                {
                                                    result.Add(new Tuple<string, IList<Tuple<string, decimal>>>(x.Key, x.Value.Select(item => new Tuple<string, decimal>(item.Key, item.Value.Item1)).ToList()));
                                                }
                                                return result;
                                            }).SelectMany(x => x, (list, item) =>
                                            {
                                                return new Field()
                                                {
                                                    Prompt = item.Item1,
                                                    Numbers = item.Item2.Select(x => x.Item1).ToList<object>()
                                                };
                                            }).ToList()
                                        }
                                  }
                              }
                        }
                    }
                 };
            }

            return playModes;
        }
    }
}