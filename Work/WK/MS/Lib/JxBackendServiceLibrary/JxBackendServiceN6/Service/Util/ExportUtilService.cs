using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Util.Export;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace JxBackendServiceN6.Service.Util
{
    public class ExportUtilService : IExportUtilService
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, List<ExportColumnSetting>> s_typeSettingMap = new ConcurrentDictionary<RuntimeTypeHandle, List<ExportColumnSetting>>();

        public bool TryConvertPagedResultModelToExportParam(object model, out ExportQueryResultParam exportParam)
        {
            try
            {
                exportParam = ConvertPagedResultModelToExportParam(model);

                return true;
            }
            catch (Exception ex)
            {
                var logUtilService = DependencyUtil.ResolveService<ILogUtilService>().Value;
                logUtilService.Error(ex);

                exportParam = new ExportQueryResultParam();

                return false;
            }
        }

        private ExportQueryResultParam ConvertPagedResultModelToExportParam(object model)
        {
            ExportQueryResultParam exportParam = new ExportQueryResultParam();
            exportParam.QueryResultModelType = model.GetType().GenericTypeArguments.Single();

            if (model.GetModelValue(nameof(PagedResultModel<object>.ResultList)) is IEnumerable modelItems)
            {
                int count = 0;

                foreach (object item in modelItems)
                {
                    count++;

                    if (count > exportParam.QueryResultLimitCount)
                    {
                        exportParam.HasResultExceedLimit = true;
                        break;
                    }

                    exportParam.QueryResult.Add(item);
                }
            }

            return exportParam;
        }

        public byte[] ExportFullPageResult(ExportFullResultParam exportParam)
        {
            string dataCount = GetDataCount(exportParam.PageGrid);
            AppendAdditionalData(exportParam.PageAdditionalData, dataCount);

            string[,] gridData = ProcessGridData(exportParam.PageGrid);

            return ExportToBytes(exportParam, gridData);
        }

        private string GetDataCount(ExportQueryResultParam pageGrid)
        {
            if (pageGrid.HasResultExceedLimit)
            {
                return "资料大于10万笔，请缩小查询范围重新查询";
            }

            return pageGrid.QueryResult.Count.ToString();
        }

        private void AppendAdditionalData(List<string> pageAdditionalData, string dataCount)
        {
            pageAdditionalData.Add($"汇出时间 : {DateTime.Now.ToFormatDateTimeString()}");
            pageAdditionalData.Add($"总笔数 : {dataCount}");
        }

        private string[,] ProcessGridData(ExportQueryResultParam pageGrid)
        {
            List<ExportColumnSetting> columnSettings = GetColumnSettings(pageGrid.QueryResultModelType);
            int rowCount = pageGrid.QueryResult.Count + 1; // +1 for header
            int columnCount = columnSettings.Count;

            string[,] gridData = new string[rowCount, columnCount];

            for (int column = 0; column < columnCount; column++)
            {
                PropertyInfo propertyInfo = columnSettings[column].ColumnProperty;
                int row = 0;
                gridData[row, column] = columnSettings[column].HeaderName;
                row++;

                foreach (object? item in pageGrid.QueryResult)
                {
                    gridData[row, column] = propertyInfo.GetValue(item).ToNonNullString();
                    row++;
                }
            }

            return gridData;
        }

        private List<ExportColumnSetting> GetColumnSettings(Type queryResultModelType)
        {
            return s_typeSettingMap.GetAssignValueOnce(
                queryResultModelType.TypeHandle,
                () =>
                {
                    List<PropertyInfo> columnProperties = ResolveColumnProperties(queryResultModelType);

                    return ConvertToColumnSetting(columnProperties);
                });
        }

        private List<PropertyInfo> ResolveColumnProperties(Type modelType)
        {
            PropertyInfo[] modelTypeProperties = modelType.GetProperties();

            List<PropertyInfo> exportSettingProperties = modelTypeProperties
                .Where(p => p.GetCustomAttributes(true).Any(a => a is ExportAttribute)).ToList();

            if (exportSettingProperties.Count > 0)
            {
                return exportSettingProperties;
            }

            return modelTypeProperties.ToList();
        }

        private List<ExportColumnSetting> ConvertToColumnSetting(List<PropertyInfo> columnProperties)
        {
            List<ExportColumnSetting> exportColumnSettings = new List<ExportColumnSetting>();

            foreach (PropertyInfo columnProperty in columnProperties)
            {
                ExportAttribute? exportAttribute = columnProperty.GetCustomAttribute<ExportAttribute>();

                if (exportAttribute == null)
                {
                    exportAttribute = new ExportAttribute();
                }

                string? headerName = exportAttribute.GetNameByResourceInfo();

                if (headerName.IsNullOrEmpty())
                {
                    DisplayAttribute? displayAttribute = columnProperty.GetCustomAttribute<DisplayAttribute>();

                    if (displayAttribute != null)
                    {
                        headerName = displayAttribute.GetName();
                    }
                    else
                    {
                        headerName = columnProperty.Name;
                    }
                }

                var setting = new ExportColumnSetting
                {
                    HeaderName = headerName,
                    Sort = exportAttribute.Sort,
                    ColumnProperty = columnProperty
                };

                exportColumnSettings.Add(setting);
            }

            return exportColumnSettings.OrderBy(s => s.Sort).ThenByDescending(s => s.HeaderName).ToList();
        }

        private byte[] ExportToBytes(ExportFullResultParam exportParam, string[,] gridData)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using ExcelPackage excelPackage = new ExcelPackage();

            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add(exportParam.PageTitle);

            // 標題
            int excelRow = 1;
            sheet.Cells[excelRow, 1].Value = exportParam.PageTitle;
            sheet.Cells[excelRow, 1].Style.Font.Bold = true;
            excelRow += 2;

            // 附加資訊
            foreach (string additionalData in exportParam.PageAdditionalData)
            {
                sheet.Cells[excelRow, 1].Value = additionalData;

                ExcelRange mergedCells = sheet.Cells[excelRow, 1, excelRow, 4]; //1-4是合併格數, 大概抓的, 若有跑版可調
                mergedCells.Merge = true;
                mergedCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                excelRow++;
            }

            excelRow++;

            // 資料表
            int gridStartRow = excelRow;
            int gridRowCount = gridData.GetLength(0);
            int gridColumnCount = gridData.GetLength(1);

            for (int gridRow = 0; gridRow < gridRowCount; gridRow++, excelRow++)
            {
                for (int gridColumn = 0, excelColumn = 1; gridColumn < gridColumnCount; gridColumn++, excelColumn++)
                {
                    sheet.Cells[excelRow, excelColumn].Value = gridData[gridRow, gridColumn];
                }
            }

            if (exportParam.PageGrid.HasResultExceedLimit)
            {
                sheet.Cells[excelRow, 1].Value = "资料汇出不完整";
                excelRow++;
            }

            // 畫表格框
            ExcelRange gridRange = sheet.Cells[gridStartRow, 1, excelRow - 1, gridColumnCount];
            ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
            System.Drawing.Color borderColor = System.Drawing.Color.Black;
            gridRange.Style.Border.Top.Style = borderStyle;
            gridRange.Style.Border.Top.Color.SetColor(borderColor);
            gridRange.Style.Border.Left.Style = borderStyle;
            gridRange.Style.Border.Left.Color.SetColor(borderColor);
            gridRange.Style.Border.Right.Style = borderStyle;
            gridRange.Style.Border.Right.Color.SetColor(borderColor);
            gridRange.Style.Border.Bottom.Style = borderStyle;
            gridRange.Style.Border.Bottom.Color.SetColor(borderColor);

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            return excelPackage.GetAsByteArray();
        }
    }
}