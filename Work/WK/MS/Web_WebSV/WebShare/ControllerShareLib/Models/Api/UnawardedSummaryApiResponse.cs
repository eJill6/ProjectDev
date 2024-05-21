namespace ControllerShareLib.Models.Api;

public class UnawardedSummaryDataRowModel
{
    public string PlayType { get; set; }
        
    public string Number { get; set; }
        
    public decimal Price { get; set; }
        
    public DateTime CreateTime { get; set; }
        
    public int Count { get; set; }
}

public class UnawardedSummaryOddsModel
{
    public string Value { get; set; }
    public string TotalAmount { get; set; }
}

public class UnawardedSummaryNumberOddsModel
{
    public string Category { get; set; }
    public int Count { get; set; }
    public List<UnawardedSummaryOddsModel> Values { get; set; }
}

public class UnawardedSummaryApiResponse
{
    public UnawardedSummaryApiResponse(List<UnawardedSummaryNumberOddsModel> numberOdds)
    {
        NumberOdds = numberOdds;
    }

    public List<UnawardedSummaryNumberOddsModel> NumberOdds { get; set; } 
}