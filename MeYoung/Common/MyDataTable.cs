using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;

namespace Common
{
    public class MyDataTable
    {
        #region 执行DataTable中的查询返回新的DataTable
        /// <summary>
        /// 执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static DataTable GetNewDataTable(DataTable dt, string condition)
        {
            System.Data.DataTable newdt = new System.Data.DataTable();
            newdt = dt.Clone();

            DataRow[] rows = dt.Select(condition);
            foreach (DataRow row in rows)
            {
                newdt.Rows.Add(row.ItemArray);
            }
            return newdt;

        }
        #endregion

        #region 从DataTable中取出置顶行，返回新的DataTable
        /// <summary>
        /// 从DataTable中取出置顶行，返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="start">起始位置 从0开始</param>
        /// <param name="count">获取条数</param>
        /// <returns></returns>
        public static DataTable GetNewDataTable(DataTable dt, int start, int count)
        {
            int dtcount = dt.Rows.Count;
            DataTable dt_Ctrl = new DataTable();
            dt_Ctrl = dt.Clone();

            for (int i = 0; i < count && start + i < dtcount; i++)
            {
                dt_Ctrl.ImportRow(dt.Rows[start + i]);
            }
            return dt_Ctrl;
        }
        #endregion

        #region 根据列名 获取数据
        /// <summary>
        /// 根据列名 获取第一行数据
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rows">行号  0 到 n-1</param>
        /// <param name="ColumnsName">列名</param>
        /// <returns></returns>
        public static string GetR0ByCol(DataTable dt, int rows, string ColumnsName)
        {
            string retval = "";
            if (dt != null && dt.Rows.Count > rows && dt.Rows[rows][ColumnsName] != null && !string.IsNullOrEmpty(dt.Rows[rows][ColumnsName].ToString()))
            {
                retval = dt.Rows[rows][ColumnsName].ToString();
            }
            return retval;
        }
        #endregion

        #region 根据列名 获取第一行数据
        /// <summary>
        /// 根据列名 获取第一行数据
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="ColumnsName"></param>
        /// <returns></returns>
        public static string GetR0ByCol(DataSet ds, string ColumnsName)
        {
            return GetR0ByCol(ds.Tables[0], 0, ColumnsName);
        }
        #endregion

        #region 根据列名 获取第一行数据
        /// <summary>
        /// 根据列名 获取第一行数据
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="ColumnsName">列名</param>
        /// <returns></returns>
        public static string GetR0ByCol(DataTable dt, string ColumnsName)
        {
            return GetR0ByCol(dt, 0, ColumnsName);
        }

        /// <summary>
        /// 根据列名 获取第一行数据
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="ColumnsName">列名</param>
        /// <returns></returns>
        public static int GetR0ByCol_i(DataTable dt, string ColumnsName)
        {
            string rets = GetR0ByCol(dt, 0, ColumnsName);
            if (rets == "")
            {
                rets = "0";
            }
            return Convert.ToInt32(rets);


        }
        /// <summary>
        /// 根据列名 获取 数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i">第几行</param>
        /// <param name="ColumnsName">列名</param>
        /// <returns></returns>
        public static int GetR0ByCol_i(DataTable dt, int i, string ColumnsName)
        {
            string rets = GetR0ByCol(dt, i, ColumnsName);
            if (rets == "")
            {
                rets = "0";
            }
            return Convert.ToInt32(rets);


        }
        #endregion

        #region Compute 运算
        /// <summary>
        /// Compute 运算
        /// 例：("sum([KeyDays])", " ApprovalResult = 1 ")
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="expression">列名</param>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        public static int GetCompute(DataTable dt, string expression, string filter)
        {
            int reti = 0;
            if (filter == "") filter = " 1 = 1 ";
            if (dt == null || dt.Compute(expression, filter) is DBNull)
            {
                reti = 0;
            }
            else
            {
                reti = Convert.ToInt32(dt.Compute(expression, filter).ToString());
            }
            return reti;
        }


        public static decimal GetComputed(DataTable dt, string expression, string filter)
        {
            decimal retd = 0;
            if (filter == "") filter = " 1 = 1 ";
            if (dt == null || dt.Compute(expression, filter) is DBNull)
            {
                retd = 0;
            }
            else
            {
                retd = Convert.ToDecimal(dt.Compute(expression, filter).ToString());
            }
            return retd;
        }

        public static string GetComputes(DataTable dt, string expression, string filter)
        {
            string rets = "";
            if (filter == "") filter = " 1 = 1 ";
            if (dt == null || dt.Compute(expression, filter) is DBNull)
            {
                rets = "";
            }
            else
            {
                rets = dt.Compute(expression, filter).ToString();
            }
            return rets;
        }

        #endregion

        #region DataTable转换为XML字符串
        /// <summary>
        /// DataTable转换为XML字符串
        /// </summary>
        /// <param name="dt">要转换的DataTable</param>
        /// <returns>xml字符串</returns>
        public static string DataTableToXml(DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1)
            {
                return String.Empty;
            }
            dt.TableName = "RetXmls";

            //杨栋修改 解决 datatable里的DateTime字段转换xml时格式问题。
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].DataType.ToString() == "System.DateTime")
                {
                    string cname = dt.Columns[i].ColumnName;
                    dt.Columns.Add(cname + "_", typeof(System.String));
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        dt.Rows[j][cname + "_"] = dt.Rows[j][i] is DBNull ? "1900-01-01 00:00:00" : Convert.ToDateTime(dt.Rows[j][i]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    dt.Columns.Remove(cname);
                    dt.Columns[cname + "_"].ColumnName = cname;
                    i--;
                }
            }

            XmlTextWriter writer = null;
            try
            {
                var stream = new MemoryStream();
                writer = new XmlTextWriter(stream, Encoding.UTF8);
                dt.WriteXml(writer);
                var count = (int)stream.Length;
                var arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                var utf = new UTF8Encoding();
                return utf.GetString(arr).Trim().Replace("<DocumentElement>", "").Replace("</DocumentElement>", "").Replace("<DocumentElement />", "").Replace("<NewDataSet>", "").Replace("</NewDataSet>", "");
            }
            catch (Exception e)
            {
                Common.FileCopy.SaveTxtLog("ConvertStreamToByteBuffer-" + e.Message);
                return String.Empty;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }
        #endregion

       
    }
}
