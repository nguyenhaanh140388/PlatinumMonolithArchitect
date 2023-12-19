namespace Platinum.Core.Utils.Offices
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using Platinum.Core.Extensions;

    /// <summary>
    /// ExcelImportHelper.
    /// </summary>
    public class ExcelExportHelper
    {
        /// <summary>
        /// Reads the excel.
        /// </summary>
        /// <typeparam name="TModel">TModel.</typeparam>
        /// <param name="config">The excel import config.</param>
        /// <returns>ImportExcelResultModel.</returns>
        public static async Task<MemoryStream> ExportExcel<TModel>(IEnumerable<TModel> models, ExcelExportConfig config)
            where TModel : new()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var memoryStream = new MemoryStream();
            using (ExcelPackage package = new ExcelPackage(memoryStream))
            {
                var worksheet = package.Workbook.Worksheets.Add(config.SheetName);

                Dictionary<string, int> dict = new Dictionary<string, int>();
                PropertyInfo[] props = typeof(TModel).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    object[] attrs = prop.GetCustomAttributes(true);
                    foreach (object attr in attrs)
                    {
                        if (attr is ExportExcelSetting headerSetting)
                        {
                            dict.Add(headerSetting.DisplayName, headerSetting.Position);
                            if ((headerSetting.IsNumber || headerSetting.IsDate) && !string.IsNullOrEmpty(headerSetting.FormatString))
                            {
                                worksheet.Column(headerSetting.Position).Style.Numberformat.Format = headerSetting.FormatString;
                            }
                        }
                    }
                }

                // Write Header
                foreach (var disItem in dict)
                {
                    worksheet.Cells[config.HeaderRowPosition, disItem.Value].Value = disItem.Key;
                }

                var addressHeader = worksheet.Cells[config.HeaderRowPosition, dict.First().Value, config.HeaderRowPosition, dict.Last().Value].Address(false, true);

                worksheet.Cells[addressHeader].Style.Font.Bold = true;
                worksheet.Cells[addressHeader].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[addressHeader].Style.Fill.BackgroundColor.SetColor(Color.MediumPurple);
                worksheet.Cells[addressHeader].Style.Font.Color.SetColor(Color.White);

                // Write Data
                var rowDataPosition = config.RowDataPosition == 0 ? config.HeaderRowPosition + 1 : config.RowDataPosition;

                int rowNumber = 1;
                int rowData = rowDataPosition;

                var enumerator = models.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    var currentObject = enumerator.Current; // Want to know Next and Previous values of current value.
                    foreach (PropertyInfo prop in props)
                    {
                        var dataItem = prop.GetValue(currentObject)?.ToString();
                        object[] attrs = prop.GetCustomAttributes(true);
                        foreach (object attr in attrs)
                        {
                            if (attr is ExportExcelSetting headerSetting)
                            {
                                if (headerSetting.DisplayName == "Index")
                                {
                                    worksheet.Cells[rowData, headerSetting.Position].Value = rowNumber;
                                }
                                else
                                {
                                    if (decimal.TryParse(dataItem, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
                                    {
                                        worksheet.Cells[rowData, headerSetting.Position].Value = result;
                                    }
                                    else if (DateTime.TryParse(dataItem, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                                    {
                                        worksheet.Cells[rowData, headerSetting.Position].Value = date;
                                    }
                                    else
                                    {
                                        worksheet.Cells[rowData, headerSetting.Position].Value = dataItem;
                                    }
                                }
                            }
                        }
                    }

                    rowNumber++;
                    rowData++;
                }

                if (config.AutoFit)
                {
                    // Autofit with minimum size for the column.
                    double minimumSize = 10;
                    worksheet.Cells.AutoFitColumns(minimumSize);

                    // Autofit with minimum and maximum size for the column.
                    double maximumSize = 50;
                    worksheet.Cells.AutoFitColumns(minimumSize, maximumSize);
                }

                await package.SaveAsync();

                // package.SaveAs(new FileInfo(config.FilePath));
                package.Dispose();
                memoryStream.Position = 0;
            } // package

            return memoryStream;
        }
    }
}
