using JxLottery.Models.Lottery.PlayTypeRadio.NuiNui;
using Constants = JxLottery.Services.BonusService.Calculators.NuiNui.Const;


namespace ControllerShareLib.Models.SpaPlayConfig
{
    /// <summary>
    /// 設定牛牛玩法
    /// </summary>
    public class NuiNuiPlayConfig : ISpaPlayConfig
    {
        private IList<PlayMode<PlayTypeInfo>> playModes;

        public int GameTypeId => (int)JxLottery.Models.Lottery.GameTypeId.NuiNui;

        private readonly IDictionary<string, int> _skipCount
            = new Dictionary<string, int>()
            {
                { "蓝方牛", 6 },
                { "红方牛", 6 },
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
                                            return new SpaPlayConfig.Field()
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