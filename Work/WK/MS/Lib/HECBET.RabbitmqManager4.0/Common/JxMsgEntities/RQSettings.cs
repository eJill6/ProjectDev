namespace JxMsgEntities
{
    using System;

    public class RQSettings
    {
        public const string RQ_DIRECT_TYPE = "direct";
        public const string RQ_FANOUT_TYPE = "fanout";
        public const string RQ_WCF_EXCHANGE = "HECBET_CLIENTS_WITH_WCF";
        public const string RQ_WCF_FANOUT_EXCHANGE = "HECBET_FANOUT";
        public const string RQ_WCF_MAG_FANOUT_EXCHANGE = "HECBET_MAG_FANOUT";
        public const string RQ_WCF_ROUTKEY = "06b5083c-9425-4a7c-b07f-8d5356f871db";

        //站內信[人員]所使用的
        public const string RQ_HEC_DIRECT_MESSAGE = "HEC_DIRECT_MESSAGE";

        //站內信[群聊]所使用的
        public const string RQ_HEC_GROUPS_MESSAGE = "HEC_GROUPS_MESSAGE";

        public const string RQ_HEC_GROUPS_ACTION = "HEC_GROUPS_ACTION";

        public const string RQ_HEC_LOTTERY_INDRAWTIP = "HECBET_LOTTERY_INDRAWTIP";
    }
}