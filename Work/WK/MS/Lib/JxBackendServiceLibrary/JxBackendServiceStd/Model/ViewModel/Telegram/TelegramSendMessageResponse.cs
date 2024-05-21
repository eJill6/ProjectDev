namespace JxBackendService.Model.ViewModel.Telegram
{

    public class TelegramSendMessageResponse
    {
        public bool Ok { get; set; }
        public Result Result { get; set; }
    }

    public class From
    {
        public long Id { get; set; }
        public bool Is_bot { get; set; }
        public string First_name { get; set; }
        public string Username { get; set; }
    }

    public class Chat
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }

    public class Result
    {
        public long Message_id { get; set; }
        public From From { get; set; }
        public Chat Chat { get; set; }
        public long Date { get; set; }
        public string Text { get; set; }
    }


}
