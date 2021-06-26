using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonV5
{
    public class ExcelHelper
    {
        /// <summary>
        /// 读取excel到datatable
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="isColumnHeader">是否有标题头</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath, bool isColumnHeader = true)
        {
            try
            {
                using (FileStream fs = File.OpenRead(filePath))
                {
                    // 版本选择
                    IWorkbook workbook = null;
                    // 2007版本  
                    if (filePath.IndexOf(".xlsx") > 0)
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    // 2003版本  
                    else if (filePath.IndexOf(".xls") > 0)
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    // 其他，报错
                    else
                    {
                        throw new Exception("选择的文件不是excel");
                    }
                    DataTable dataTable = new DataTable();
                    // 获取第一个sheet
                    ISheet sheet = workbook.GetSheetAt(0);
                    int totalRow = sheet.LastRowNum;
                    IRow firstRow = sheet.GetRow(0);
                    // 问题：如果第一行最后一列无数据，那么将无法读取到最后一行，推荐使用标题
                    int cellCount = firstRow.LastCellNum;
                    int startRow;
                    // 处理标题头
                    if (isColumnHeader)
                    {
                        startRow = 1;
                        for (int i = 0; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                DataColumn column = new DataColumn(cell.ToString());
                                dataTable.Columns.Add(column);
                            }
                            else
                            {
                                DataColumn column = new DataColumn("");
                                dataTable.Columns.Add(column);
                            }
                        }
                    }
                    else
                    {
                        startRow = 0;
                        for (int i = 0; i < cellCount; ++i)
                        {
                            DataColumn column = new DataColumn("column" + (i + 1));
                            dataTable.Columns.Add(column);
                        }
                    }
                    //填充行  
                    for (int i = startRow; i <= totalRow; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        DataRow dataRow = dataTable.NewRow();
                        for (int j = 0; j < cellCount; ++j)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell == null)
                            {
                                dataRow[j] = "";
                            }
                            else
                            {
                                dataRow[j] = cell.ToString();
                            }
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                    Log.Information($"读取{filePath}-ok");
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                // 错误写入日志
                Log.Error(ex,ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 将datatable写入excel(xlsx)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath">写入的文件路径</param>
        /// <returns></returns>
        public static bool DataTableToExcel(DataTable dt, string filePath)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    int rowCount = dt.Rows.Count;
                    int columnCount = dt.Columns.Count;
                    ISheet sheet = workbook.CreateSheet();
                    IRow row = sheet.CreateRow(0);
                    ICell cell;
                    for (int i = 0; i < columnCount; i++)
                    {
                        cell = row.CreateCell(i);
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }
                    for (int i = 0; i < rowCount; i++)
                    {
                        row = sheet.CreateRow(i + 1);
                        for (int j = 0; j < columnCount; j++)
                        {
                            cell = row.CreateCell(j);
                            cell.SetCellValue(dt.Rows[i][j].ToString());
                        }
                    }
                    using (FileStream fs = File.OpenWrite(filePath))
                    {
                        workbook.Write(fs);//向打开的这个xls文件中写入数据  
                    }
                }
                Log.Information($"写入{filePath}-ok");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }
    }
}
