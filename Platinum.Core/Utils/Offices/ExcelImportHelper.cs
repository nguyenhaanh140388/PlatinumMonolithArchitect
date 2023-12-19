namespace Platinum.Core.Utils.Offices
{
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// ExcelImportHelper.
    /// </summary>
    public class ExcelImportHelper
    {

        public static List<string> AllSheetNames(Stream file, Func<ExcelWorksheet, bool> predicate)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                return package.Workbook.Worksheets
                    .Where(predicate)
                    .Select(x => x.Name)
                    .ToList();
            }
        }

        public static int GetWorksheetPositionID(Stream file, Func<ExcelWorksheet, bool> predicate)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(file))
            {
                return package.Workbook.Worksheets
                    .Where(predicate)
                    .Select(x => x.Index)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Reads the excel.
        /// </summary>
        /// <typeparam name="TModel">TModel.</typeparam>
        /// <param name="excelImportConfig">The excel import config.</param>
        /// <returns>ImportExcelResultModel.</returns>
        public static ImportExcelResultModel<TModel> ReadExcel<TModel>(ExcelImportConfig excelImportConfig)
            where TModel : new()
        {
            var readExcelResult = new ImportExcelResultModel<TModel>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(excelImportConfig.File))
            {
                var worksheet = package.Workbook.Worksheets[excelImportConfig.SheetPosition];
                int totalRows = worksheet.Dimension.Rows;
                int totalColumn = worksheet.Dimension.Columns;

                PropertyInfo[] props = typeof(TModel).GetProperties();

                if (excelImportConfig.HasHeader)
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    foreach (PropertyInfo prop in props)
                    {
                        object[] attrs = prop.GetCustomAttributes(true);
                        foreach (object attr in attrs)
                        {
                            if (attr is ImportExcelSetting headerSetting && headerSetting.HeaderName != "Index")
                            {
                                dict.Add(headerSetting.HeaderName, headerSetting.Position);
                            }
                        }
                    }

                    // Validate Header
                    foreach (var disItem in dict)
                    {
                        if (!worksheet.Cells[excelImportConfig.HeaderRowPosition, disItem.Value].Text.Equals(disItem.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            readExcelResult.IsTemplateValid = false;
                            break;
                        }
                    }
                }

                var rowDataPosition = excelImportConfig.RowDataPosition == 0 ? excelImportConfig.HeaderRowPosition + 1 : excelImportConfig.RowDataPosition;

                // Read data
                if (readExcelResult.IsTemplateValid || !excelImportConfig.HasHeader)
                {
                    for (int i = rowDataPosition; i <= totalRows; i++)
                    {
                        TModel dataItem = new TModel();
                        int numOfCol = 0;
                        int emptyCellCount = 0;
                        foreach (PropertyInfo prop in props)
                        {
                            object[] attrs = prop.GetCustomAttributes(true);
                            foreach (object attr in attrs)
                            {
                                if (attr is ImportExcelSetting headerSetting)
                                {
                                    if (headerSetting.HeaderName == "Index")
                                    {
                                        prop.SetValue(dataItem, i);
                                    }
                                    else
                                    {
                                        numOfCol++;
                                        string cellValue = worksheet.Cells[i, headerSetting.Position].Value?.ToString()?.Trim();
                                        object valueResult = null;

                                        if (prop.PropertyType == typeof(decimal))
                                        {
                                            if (decimal.TryParse(cellValue, out decimal result))
                                            {
                                                valueResult = result;
                                            }
                                        }
                                        else if (prop.PropertyType == typeof(bool))
                                        {
                                            if (bool.TryParse(cellValue, out bool result))
                                            {
                                                valueResult = result;
                                            }
                                        }
                                        else if (prop.PropertyType == typeof(DateTime))
                                        {
                                            if (DateTime.TryParse(cellValue, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                                            {
                                                valueResult = result;
                                            }
                                        }
                                        else if (prop.PropertyType == typeof(int))
                                        {
                                            if (int.TryParse(cellValue, out int result))
                                            {
                                                valueResult = result;
                                            }
                                        }
                                        else if (prop.PropertyType == typeof(float))
                                        {
                                            if (float.TryParse(cellValue, out float result))
                                            {
                                                valueResult = result;
                                            }
                                        }
                                        else if (prop.PropertyType == typeof(Guid))
                                        {
                                            if (Guid.TryParse(cellValue, out Guid result))
                                            {
                                                valueResult = result;
                                            }
                                        }
                                        //else if (prop.PropertyType == typeof(Image))
                                        //{
                                        //    if (float.TryParse(cellValue, out float result))
                                        //    {
                                        //        var pic = worksheet.Drawings["pic_001"] as ExcelPicture;
                                        //        valueResult = pic.Image;
                                        //    }
                                        //}
                                        else
                                        {
                                            valueResult = cellValue;
                                        }
                                        //if (headerSetting.IsReadText)
                                        //{
                                        //    cellValue = worksheet.Cells[i, headerSetting.Position].Text?.ToString()?.Trim();
                                        //}
                                        //else
                                        //{

                                        //}

                                        prop.SetValue(dataItem, valueResult);
                                        if (cellValue == null)
                                        {
                                            emptyCellCount++;
                                        }
                                    }
                                }
                            }
                        }

                        if (numOfCol != emptyCellCount)
                        {
                            readExcelResult.ResultData.Add(dataItem);
                        }
                    }
                }

                // Validate existing data
                if (readExcelResult.ResultData.Count == 0)
                {
                    readExcelResult.DataIsEmpty = true;
                }
            }

            return readExcelResult;
        }
    }
}
