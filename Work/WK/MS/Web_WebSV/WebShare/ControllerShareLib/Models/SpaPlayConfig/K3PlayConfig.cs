using JxLottery.Models.Lottery.PlayTypeRadio.K3;
using Constants = JxLottery.Services.BonusService.Calculators.K3.Constants;

namespace ControllerShareLib.Models.SpaPlayConfig
{
    public class K3PlayConfig : ISpaPlayConfig
    {
        private IList<PlayMode<PlayTypeInfo>> playModes;

        /// <inheritdoc cref="ISpaPlayConfig.GameTypeId"/>
        public int GameTypeId => (int)JxLottery.Models.Lottery.GameTypeId.K3;

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
                                                { "两面", "4-1" },
                                                { "总和", "7-3" },
                                                { "单骰", "3-2" },
                                                { "对子", "3-2" },
                                                { "豹子", "4-2" },
                                            },
                                            Fields = Constants.YinXiangZongHe.ManPeis.Select(x =>
                                            {
                                                var result = new List<Tuple<string, IList<Tuple<string, decimal>>>>();
                                                var count = x.Value.Values.Count / 2;
                                                if (x.Value.Values.Count % 2 > 0)
                                                {
                                                    count++;
                                                }
                                                result.Add(new Tuple<string, IList<Tuple<string, decimal>>>(x.Key, x.Value.Take(count).Select(item => new Tuple<string, decimal>(item.Key, item.Value.Item1)).ToList()));
                                                result.Add(new Tuple<string, IList<Tuple<string, decimal>>>(x.Key, x.Value.Skip(count).Select(item => new Tuple<string, decimal>(item.Key, item.Value.Item1)).ToList()));
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