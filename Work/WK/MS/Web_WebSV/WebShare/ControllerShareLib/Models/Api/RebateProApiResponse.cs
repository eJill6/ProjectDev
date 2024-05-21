namespace ControllerShareLib.Models.Api;

/// <summary>App Team開規格我們實作</summary>
public class OddsModel
{
    public string Value { get; set; }
    public string Odds { get; set; }
}

public class NumberOddsModel
{
    public string Category { get; set; }
    public List<OddsModel> Values { get; set; }
}

public class RebateProApiResponse
{
    public RebateProApiResponse(List<NumberOddsModel> numberOdds)
    {
        NumberOdds = numberOdds;
    }

    public List<NumberOddsModel> NumberOdds { get; set; } 
}