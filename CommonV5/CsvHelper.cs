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
    public class CsvHelper
    {
        /// <summary>
        /// 将datatable写入csv
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DataTableToCsv(DataTable dt, string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        string colText = "";
                        string rowText = "";
                        // 获取列名
                        foreach (DataColumn colItem in dt.Columns)
                        {
                            colText += colItem.ToString() + ",";
                        }
                        colText = colText.Substring(0, colText.Length - 1);// 去除最后一个逗号
                        sw.WriteLine(colText);
                        foreach (DataRow rowItem in dt.Rows)
                        {
                            rowText = "";
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                rowText += rowItem[i].ToString() + ",";
                            }
                            rowText = rowText.Substring(0, rowText.Length - 1);
                            sw.WriteLine(rowText);
                        }
                    }
                    Log.Debug($"读取{filePath}-ok");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 读取csv到datatable
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static DataTable CsvToDataTable(string filepath)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                {
                    DataTable dt = new DataTable();
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        bool isFrist = true;
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (isFrist)
                            {
                                isFrist = false;
                                string[] tableHead = line.Split(',');
                                for (int i = 0; i < tableHead.Length; i++)
                                {
                                    DataColumn dc = new DataColumn(tableHead[i]);
                                    dt.Columns.Add(dc);
                                }
                            }
                            else
                            {
                                string[] aryLine = line.Split(',');
                                DataRow dr = dt.NewRow();
                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    dr[i] = aryLine[i];
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return null;
            }

        }
    }
}
