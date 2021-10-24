using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.Route
{
    public class RouteVersion : BaseStringValueModel<RouteVersion>
    {
        public string ViewFolderRootPath { get; private set; }

        public string ContentFolderRootUrl { get; private set; }

        public string BundleVirtualRootPath { get; private set; }

        private RouteVersion() { }

        public static readonly RouteVersion Default = new RouteVersion()
        {
            Value = "Default",
            ViewFolderRootPath = $"/Views/{GlobalVariables.MerchantFolderCode}/",   //挖洞填入商戶路徑
            ContentFolderRootUrl = $"/Content/{GlobalVariables.MerchantFolderCode}/",
            BundleVirtualRootPath = $"~/bundles/{GlobalVariables.MerchantFolderCode}/",
        };

        //public static readonly RouteVersion V2 = new RouteVersion()
        //{
        //    Value = "V2",
        //    ViewFolderRootPath = "/Views/{0}/V2/",
        //    ContentFolderRootUrl = "/Content/{0}/V2/",
        //    BundleVirtualRootPath = "~/bundles/{0}/V2/",
        //};
    }
}
