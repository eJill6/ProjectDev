namespace JxBackendService.Interfaces.Model.Mobile
{
    public interface IMobileApiSignParam
    {
        string Sign { get; set; }

        string Coordinate { get; }

        string Key { get; set; }
    }
}