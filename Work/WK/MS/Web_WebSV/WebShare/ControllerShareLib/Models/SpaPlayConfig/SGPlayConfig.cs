using JxLottery.Models.Lottery.PlayTypeRadio.SG;

namespace ControllerShareLib.Models.SpaPlayConfig;

/// <summary>
/// 設定三公玩法
/// </summary>
public class SGPlayConfig : ISpaPlayConfig
{
    private IList<PlayMode<PlayTypeInfo>> playModes;

    public int GameTypeId => (int)JxLottery.Models.Lottery.GameTypeId.SG;
    
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
                                    Fields = JxLottery.Services.BonusService.Calculators.SG.Const
                                        .YinXiangZongHeCalculator.ManPeis.Select(x =>
                                        {
                                            var result = new List<Tuple<string, IList<Tuple<string, decimal[]>>>>();
                                            result.Add(new Tuple<string, IList<Tuple<string, decimal[]>>>(x.Key,
                                                x.Value.Select(item =>
                                                        new Tuple<string, decimal[]>(item.Key, item.Value.Item1))
                                                    .ToList()));

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