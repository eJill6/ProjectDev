using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Entity.Game.Lottery
{
    public class PlayTypeInfo
    {
        [Key]
        public int PlayTypeID { get; set; }

        public string PlayTypeName { get; set; }

        public int LotteryID { get; set; }

        public int Priority { get; set; }

        public int Status { get; set; }

        public int UserType { get; set; }
    }
}