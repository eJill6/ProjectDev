using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class TelegramChatGroup : BaseValueModel<long, TelegramChatGroup>
    {
        private TelegramChatGroup() { }

        public static TelegramChatGroup Production = new TelegramChatGroup() { Value = -1001274075068 };
        public static TelegramChatGroup Testing = new TelegramChatGroup() { Value = -1001351931197 };        
    }
}
