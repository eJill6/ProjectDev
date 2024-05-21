using JxBackendService.Model.Enums;

namespace BackSideWeb.Models.Enums
{
    public class LayoutType : BaseStringValueModel<LayoutType>
    {
        private LayoutType(string value)
        {
            Value = value;
        }

        public static readonly LayoutType Base = new LayoutType("_BaseLayout");

        public static readonly LayoutType ReadSingleRow = new LayoutType("_ReadSingleRowLayout");

        public static readonly LayoutType EditSingleRow = new LayoutType("_EditSingleRowLayout");

        public static readonly LayoutType Default = new LayoutType("_Layout");

        public static readonly LayoutType SearchGrid = new LayoutType("_SearchGridLayout");

        public static readonly LayoutType Popup = new LayoutType("_PopupLayout");

        public static readonly LayoutType GameCenterManage = new LayoutType("~/Views/GameManage/_GameCenterManageLayout.cshtml");

        public static readonly LayoutType OpenSearchGridPage = new LayoutType("_OpenSearchGridPage");
    }
}