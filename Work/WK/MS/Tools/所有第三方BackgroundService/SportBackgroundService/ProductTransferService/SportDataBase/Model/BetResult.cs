using JxBackendService.Common.Extensions;

namespace ProductTransferService.SportDataBase.Model
{
    public class BetResult
    {
        public string last_version_key = string.Empty;

        /// <summary>
        /// 體育方面的注單資料
        /// </summary>
        public List<BetDetails> BetDetails = new List<BetDetails>();

        /// <summary>
        /// 賽馬方面的注單資料
        /// </summary>
        public List<BetHorseDetails> BetHorseDetails = new List<BetHorseDetails>();

        /// <summary>
        /// 真人娛樂方面的注單資料
        /// </summary>
        public List<BetLiveCasinoDetails> BetLiveCasinoDetails = new List<BetLiveCasinoDetails>();

        /// <summary>
        /// 百家練方面的注單資料
        /// </summary>
        public List<BetNumberDetails> BetNumberDetails = new List<BetNumberDetails>();

        /// <summary>
        /// 虛擬體育的注單資料
        /// </summary>
        public List<BetVirtualSportDetails> BetVirtualSportDetails = new List<BetVirtualSportDetails>();

        /// <summary>
        /// 電子遊戲的注單資料
        /// </summary>
        public List<BetCasinoDetails> BetCasinoDetails = new List<BetCasinoDetails>();

        public bool AnyAndNotNull() => BetDetails.AnyAndNotNull() ||
            BetHorseDetails.AnyAndNotNull() ||
            BetLiveCasinoDetails.AnyAndNotNull() ||
            BetNumberDetails.AnyAndNotNull() ||
            BetVirtualSportDetails.AnyAndNotNull() ||
            BetCasinoDetails.AnyAndNotNull();
    }
}