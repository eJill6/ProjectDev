namespace JxBackendService.Interface.Model.ViewModel
{
    public interface IBaseFrontsideProductAESMenu
    {
        string ProductCode { get; set; }

        string GameCode { get; set; }

        string RemoteCode { get; set; }

        string AESFullImageUrl { get; set; }

        string GameLobbyTypeValue { get; set; }

        string Title { get; set; }
    }

    public interface IBaseFrontsideProductMenu : IBaseFrontsideProductAESMenu
    {
        string FullImageUrl { get; set; }

        bool IsMaintaining { get; set; }
    }

    internal interface IFrontsideProductMenu : IBaseFrontsideProductMenu
    {
        string CardCssClass { get; set; }

        string CardImageName { get; set; }

        string AESCardImageName { get; set; }

        string GameLobbyUrl { get; set; }
    }
}