namespace ProductTransferService.AgDataBase.Model
{
    public class AgFishInfo : BaseAGInfoModel, IComparable
    {
        public int Id { get; set; }

        public string dataType { get; set; }

        public string ProfitLossID { get; set; }

        public string tradeNo { get; set; }

        public string sceneId { get; set; }

        public string playerName { get; set; }

        public string type { get; set; }

        public DateTime SceneStartTime { get; set; }

        public DateTime SceneEndTime { get; set; }

        public string Roomid { get; set; }

        public string Roombet { get; set; }

        public decimal Cost { get; set; }

        public decimal Earn { get; set; }

        public decimal Jackpotcomm { get; set; }

        public decimal transferAmount { get; set; }

        public decimal previousAmount { get; set; }

        public decimal currentAmount { get; set; }

        public string currency { get; set; }

        public string exchangeRate { get; set; }

        public string IP { get; set; }

        public string flag { get; set; }

        public DateTime creationTime { get; set; }

        public string gameCode { get; set; }

        public override bool IsIgnoreAddProfitLoss => type == "3";

        public int CompareTo(object obj)
        {
            int res = 0;
            AgFishInfo sObj = (AgFishInfo)obj;
            if (this.SceneStartTime > sObj.SceneStartTime)
            {
                res = 1;
            }
            else if (this.SceneStartTime < sObj.SceneStartTime)
            {
                res = -1;
            }

            return res;
        }

        public string MiseOrderGameId { get; set; }

        public override string KeyId => tradeNo;

        public override string TPGameAccount => playerName;
    }
}