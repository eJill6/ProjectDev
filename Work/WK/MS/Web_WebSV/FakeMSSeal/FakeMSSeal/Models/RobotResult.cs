namespace FakeMSSeal.Models
{
    public class RobotResult : ResultModel<Robot[]>
    {
    }

    public class Robot
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
        public decimal Point { get; set; }
        public decimal Amount { get; set; }
    }
}