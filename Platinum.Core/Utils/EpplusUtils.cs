using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Anhny010920.Core.Utilities
{
    public static class EpplusUtils
    {
        private static T ReadFromExcel<T>(string path, bool hasHeader = true)
        {
            using (var excelPack = new ExcelPackage())
            {
                //Load excel stream
                using (var stream = File.OpenRead(path))
                {
                    excelPack.Load(stream);
                }

                //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
                var ws = excelPack.Workbook.Worksheets[0];

                //Get all details as DataTable -because Datatable make life easy :)
                DataTable excelasTable = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    //Get colummn details
                    if (!string.IsNullOrEmpty(firstRowCell.Text))
                    {
                        string firstColumn = string.Format("Column {0}", firstRowCell.Start.Column);
                        excelasTable.Columns.Add(hasHeader ? firstRowCell.Text : firstColumn);
                    }
                }
                var startRow = hasHeader ? 2 : 1;
                //Get row details
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, excelasTable.Columns.Count];
                    DataRow row = excelasTable.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                //Get everything as generics and let end user decides on casting to required type
                var generatedType = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(excelasTable));
                return (T)Convert.ChangeType(generatedType, typeof(T));
            }
        }

        #region 读取Excel数据到DataSet
        /// <summary>
        /// 读取Excel数据到DataSet
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static DataSet ReadExcelToDataSet(string filePath)
        {
            DataSet ds = new DataSet("ds");
            DataRow dr;
            object objCellValue;
            string cellValue;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            using (ExcelPackage package = new ExcelPackage())
            {
                package.Load(fs);
                foreach (var sheet in package.Workbook.Worksheets)
                {
                    if (sheet.Dimension == null) continue;
                    var columnCount = sheet.Dimension.End.Column;
                    var rowCount = sheet.Dimension.End.Row;
                    if (rowCount > 0)
                    {
                        DataTable dt = new DataTable(sheet.Name);
                        for (int j = 0; j < columnCount; j++)//设置DataTable列名
                        {
                            objCellValue = sheet.Cells[1, j + 1].Value;
                            cellValue = objCellValue == null ? "" : objCellValue.ToString();
                            dt.Columns.Add(cellValue, typeof(string));
                        }
                        for (int i = 2; i <= rowCount; i++)
                        {
                            dr = dt.NewRow();
                            for (int j = 1; j <= columnCount; j++)
                            {
                                objCellValue = sheet.Cells[i, j].Value;
                                cellValue = objCellValue == null ? "" : objCellValue.ToString();
                                dr[j - 1] = cellValue;
                            }
                            dt.Rows.Add(dr);
                        }
                        ds.Tables.Add(dt);
                    }
                }
            }
            return ds;

        }
        #endregion

        #region 把类转换为Excel
        /// <summary>
        /// 生成Excel
        /// </summary>
        /// <typeparam name="T">实体类没有层级的表状结构</typeparam>
        /// <param name="dataList"></param>
        public static void CreatExcelByClass<T>(string strFilePath, List<T> dataList) where T : class
        {
            Type t = typeof(T);
            FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (File.Exists(strFilePath))
                File.Delete(strFilePath);
            FileInfo newfile = new FileInfo(strFilePath);

            using (ExcelPackage package = new ExcelPackage(newfile))
            {
                // 在工作簿中获得第一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("sheet1");

                int row = 1, col = 1;
                //表头
                fields.ToList().ForEach((n) =>
                {
                    worksheet.Cells[1, col].Value = n.Name;
                    col++;
                });

                worksheet.View.FreezePanes(row + 1, 1); //冻结表头
                int i = 0;
                ////各行数据
                foreach (var item in dataList)
                {
                    int j = 0;
                    foreach (var p in fields)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = p.GetValue(item).ToString();
                        j++;
                    }
                    i++;
                }
                package.Save();
            }
        }
        #endregion
    }
}
