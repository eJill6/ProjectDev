using Serenity.ComponentModel;
using Serenity.Web;
using System;

namespace Management.ProductManagement.Forms
{
    [FormScript("ProductManagement.FrontsideMenu")]
    [BasedOnRow(typeof(FrontsideMenuRow), CheckNames = true)]
    public class FrontsideMenuForm
    {
        public string MenuName { get; set; }
        public string EngName { get; set; }
        public string PicName { get; set; }
        public string ProductCode { get; set; }
        public string GameCode { get; set; }
        public int Type { get; set; }
        public int Sort { get; set; }
        public int AppSort { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}