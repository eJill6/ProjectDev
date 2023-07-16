namespace JxBackendService.Model.Entity
{
    public class InsertSysSMS
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Content { get; set; }

        public string PhoneNumber { get; set; }

        public int Type { get; set; }
    }
}