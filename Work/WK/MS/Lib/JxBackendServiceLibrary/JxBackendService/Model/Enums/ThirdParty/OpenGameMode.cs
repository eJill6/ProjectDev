namespace JxBackendService.Model.Enums.ThirdParty
{
    public class OpenGameMode : BaseStringValueModel<OpenGameMode>
    {
        private OpenGameMode()
        { }

        public static readonly OpenGameMode IFrame = new OpenGameMode() { Value = "IFrame" };

        public static readonly OpenGameMode Redirect = new OpenGameMode() { Value = "Redirect" };
    }

    public class LinkTarget : BaseStringValueModel<LinkTarget>
    {
        private LinkTarget()
        { }

        public static readonly LinkTarget Self = new LinkTarget() { Value = "_self" };

        public static readonly LinkTarget Blank = new LinkTarget() { Value = "_blank" };

        public static readonly LinkTarget LayerOpen = new LinkTarget() { Value = "LayerOpen" }; //注意 這一塊需要特別實作
    }
}