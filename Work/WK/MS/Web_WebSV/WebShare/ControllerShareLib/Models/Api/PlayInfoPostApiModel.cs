using ControllerShareLib.Models.Base;

namespace ControllerShareLib.Models.Api;

public class PlayInfoPostApiModel : PlayInfoPostModel
{
    /// <summary>這兩個欄位在api是非必填</summary>
    public new string? PlayTypeName { get; set; }

    public new string? PlayTypeRadioName { get; set; }
}