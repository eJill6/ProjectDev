﻿using JxBackendService.Model.ViewModel;

namespace JxBackendService.Model
{
    public class SendTelegramParam
    {
        /// <summary>
        /// 實際API Url或返代網址
        /// </summary>
        public string ApiUrl { get; set; }

        public EnvironmentUser EnvironmentUser { get; set; }

        public string Message { get; set; }
    }
}