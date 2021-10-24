using JxBackendService.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.VIP
{
    public class BaseVIPChangeLogModel
    {
        public string DataSourceCode { get; set; }

        public string DataSourceName { get; set; }

        public string SEQID { get; set; }

        public DateTime CreateDate { get; set; }

        public string MemoJson { get; set; }

        public string Memo { get => MemoJson.ToLocalizationContent(); set => throw new NotImplementedException(); }

        public string CreateDateText => CreateDate.ToFormatDateTimeString();
    }
}
