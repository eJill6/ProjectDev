using JxBackendService.Model.Util.Export;

namespace JxBackendService.Interface.Service.Util
{
    public interface IExportUtilService
    {
        bool TryConvertPagedResultModelToExportParam(object model, out ExportQueryResultParam exportParam);

        byte[] ExportFullPageResult(ExportFullResultParam exportParam);
    }
}