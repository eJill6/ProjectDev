using JxBackendService.Interface.Model.Common;

namespace JxBackendService.Interface.Model.MiseLive.Request
{
    public interface IMiseLiveRequest
    {
        long Ts { get; set; }

        string Sign { get; set; }
    }

    public interface IMiseLiveSaltRequest : IMiseLiveRequest
    {
        string Salt { get; set; }
    }

    public interface IMiseLiveAppRequest
    {
        string App { get; }
    }

    /// <summary>
    /// service參數用,不可指定Salt,Ts
    /// </summary>
    public interface IMiseLiveRequestParam
    {
    }

    public interface IMiseLiveUserColumn
    {
        int UserId { get; set; }
    }
}