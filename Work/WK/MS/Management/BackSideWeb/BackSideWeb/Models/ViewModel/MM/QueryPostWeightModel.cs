using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Model.ViewModel.MM
{
    public class QueryPostWeightModel : MMPostWeightBs, IDataKey
    {
        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case 0:
                        return "審核中";
                    case 1:
                        return "展示中";
                    case 2:
                        return "未通過";
                    default:
                        return Status.ToString();
                }
            }
        }
        public string KeyContent => Id.ToString();
    }
}
