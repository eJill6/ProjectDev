using System.Collections.Generic;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Util
{
    public class LocalizationSentence
    {
        /// <summary>
        /// resource namespace fullname EX: typeof(PageElement).FullName
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// resource 對應 key EX: nameof(DBContentElement.VIPReceivedBirthdayGiftMoney)
        /// </summary>
        public string ResourcePropertyName { get; set; }

        /// <summary>
        /// 參數值
        /// </summary>
        public List<string> Args { get; set; }

        /// <summary>
        /// 可顯示資料的App代碼列表
        /// 對應值在<see cref="JxApplication.AppCodeForLocalizationParam"/>
        /// </summary>
        public List<int> Apps { get; set; }
    }

    public class LocalizationParam
    {
        /// <summary>
        /// 多組句子分隔符號 EX: ||
        /// </summary>
        public string SplitOperator { get; set; }

        /// <summary>
        /// 多個句子組合
        /// </summary>
        public List<LocalizationSentence> LocalizationSentences { get; set; }
    }
}