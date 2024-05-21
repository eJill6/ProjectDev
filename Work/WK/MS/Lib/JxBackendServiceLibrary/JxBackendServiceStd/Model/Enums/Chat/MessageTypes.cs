namespace JxBackendService.Model.Enums.Chat
{
    /// <summary>訊息類別</summary>
    public class MessageType : BaseIntValueModel<MessageType>
    {
        private MessageType()
        {
        }

        /// <summary>文字</summary>
        public static readonly MessageType Text = new MessageType() { Value = 1 };

        /// <summary>圖片</summary>
        public static readonly MessageType Image = new MessageType() { Value = 2 };

        /// <summary>檔案</summary>
        public static readonly MessageType File = new MessageType() { Value = 3 };
    }

    /// <summary>搜尋方向</summary>
    public class SearchDirectionType : BaseIntValueModel<SearchDirectionType>
    {
        private SearchDirectionType()
        { }

        /// <summary>往後撈取新訊息</summary>
        public static readonly SearchDirectionType Forward = new SearchDirectionType() { Value = 1 };

        /// <summary>往前撈取舊訊息</summary>
        public static readonly SearchDirectionType Backward = new SearchDirectionType() { Value = 2 };
    }
}