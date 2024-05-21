namespace JxBackendService.Interface.Service.Web.BackSideWeb
{
    public interface IDataKey
    {
        /// <summary>由實作者決定主鍵資料,可以是json, 純值...各種格式</summary>
        string KeyContent { get; }
    }
}