using JxBackendService.Common.Util;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JxBackendService.Common
{
    public class ExportTool
    {
        private readonly int _defaultColumnWidth = 20;

        public ExportTool()
        {
        }

        public byte[] ExportToExcel<T>(
            List<ExcelColumnFormat> colFormats,
            List<T> rowDatas,
            string sheetName = "Sheet1",
            Func<ExcelWorksheet, int> fillHeader = null,
            Action<ExcelWorksheet, int> fillFooter = null)
        {
            var stream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(stream))
            {
                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add(sheetName);
                int lastHeaderRowIndex = 0;

                if (fillHeader != null)
                {
                    lastHeaderRowIndex = fillHeader.Invoke(sheet);
                }

                int columnIndex = 1;
                int gridStartRowIndex = lastHeaderRowIndex + 1;

                foreach (ExcelColumnFormat format in colFormats)
                {
                    if (!format.Visibility)
                    {
                        continue;
                    }

                    int rowIndex = gridStartRowIndex;

                    sheet.Cells[rowIndex, columnIndex].Value = format.ExcelColumnHeaderName;

                    PropertyInfo propertyInfo = typeof(T).GetProperty(format.DataPropertyName);

                    foreach (T data in rowDatas)
                    {
                        sheet.Cells[++rowIndex, columnIndex].Value = propertyInfo.GetValue(data);
                    }

                    columnIndex++;
                }

                if (fillFooter != null)
                {
                    //header資料列+清單標題列+清單
                    int totalRowCount = lastHeaderRowIndex + 1 + rowDatas.Count;
                    fillFooter.Invoke(sheet, totalRowCount);
                }

                sheet.Cells[gridStartRowIndex, 1, gridStartRowIndex, colFormats.Count].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheet.Cells[gridStartRowIndex, 1, gridStartRowIndex, colFormats.Count].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                sheet.DefaultColWidth = _defaultColumnWidth;
                sheet.View.FreezePanes(gridStartRowIndex + 1, 1);

                excelPackage.Save();
            }

            return stream.ToArray();
        }
    }

    public class ExcelColumnFormat
    {
        public ExcelColumnFormat(string excelColumnHeaderName, string dataPropertyName)
        {
            ExcelColumnHeaderName = excelColumnHeaderName;
            DataPropertyName = dataPropertyName;
        }

        public ExcelColumnFormat(string excelColumnHeaderName, string dataPropertyName, bool visibility)
        {
            ExcelColumnHeaderName = excelColumnHeaderName;
            DataPropertyName = dataPropertyName;
            Visibility = visibility;
        }

        /// <summary>
        /// 導出Excel後, 資料列的標題
        /// </summary>
        public string ExcelColumnHeaderName { get; set; }

        /// <summary>
        /// 來源資料的值的屬性名
        /// </summary>
        public string DataPropertyName { get; set; }

        /// <summary>
        /// 資料列是否顯示
        /// </summary>
        public bool Visibility { get; set; } = true;
    }
}