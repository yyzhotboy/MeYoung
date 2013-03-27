using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;

namespace Common
{
    public class Excel
    {
        /// <summary>
        /// 读取excel文件到 dataset 要求是xls文件
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static DataSet ExcelToDS(string Path)
        {
            DataSet ds = null;
            OleDbConnection conn = null;
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=\"Excel 8.0;IMEX=1\"";
                conn = new OleDbConnection(strConn);
                conn.Open();
                string strExcel = "";
                OleDbDataAdapter myCommand = null;
                strExcel = "select * from [sheet1$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return ds;
            //获取非默认的工作表名 sheet1
            //DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            //string tableName = schemaTable.Rows[0][2].ToString().Trim();  
        }




        /// <summary>
        /// dataset 生成excel
        /// 杨栋 2012-3-13
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="oldds"></param>
        public static void DSToExcel(string Path, DataSet oldds)
        {
            OleDbConnection objConn = null;
            try
            {
                objConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties=Excel 8.0;");
                using (objConn)
                {
                    objConn.Open();
                    using (OleDbCommand objCmd = new OleDbCommand())
                    {
                        string tablecolumnssql = "";
                        for (int i = 0; i < oldds.Tables[0].Columns.Count; i++)
                        {
                            tablecolumnssql += "[" + oldds.Tables[0].Columns[i].ColumnName.Replace("[", "【").Replace("]", "】") + "] char(255),";
                        }
                        tablecolumnssql = tablecolumnssql.Trim(',');
                        //创建excel
                        objCmd.Connection = objConn;
                        objCmd.CommandText = "CREATE TABLE Sheet1 (" + tablecolumnssql + ")";
                        objCmd.ExecuteNonQuery();
                        //objCmd.CommandText = "Insert into tablename (FirstName, LastName)" +
                        //    " values ('王', '二')";
                        //objCmd.ExecuteNonQuery();
                        //更新数据
                        string strCom = "select * from [Sheet1$]";
                        OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, objConn);
                        System.Data.OleDb.OleDbCommandBuilder builder = new OleDbCommandBuilder(myCommand);
                        //QuotePrefix和QuoteSuffix主要是对builder生成InsertComment命令时使用。    
                        builder.QuotePrefix = "[";
                        //获取insert语句中保留字符（起始位置）    
                        builder.QuoteSuffix = "]";
                        //获取insert语句中保留字符（结束位置）    
                        DataSet newds = new DataSet();
                        myCommand.Fill(newds, "Table1");
                        for (int i = 0; i < oldds.Tables[0].Rows.Count; i++)
                        {
                            //在这里不能使用ImportRow方法将一行导入到news中，
                            //因为ImportRow将保留原来DataRow的所有设置(DataRowState状态不变)。
                            //在使用ImportRow后newds内有值，但不能更新到Excel中因为所有导入行的DataRowState!=Added    
                            DataRow nrow = newds.Tables["Table1"].NewRow();
                            for (int j = 0; j < newds.Tables[0].Columns.Count; j++)
                            {
                                nrow[j] = oldds.Tables[0].Rows[i][j];
                            }
                            newds.Tables["Table1"].Rows.Add(nrow);
                        }
                        myCommand.Update(newds, "Table1");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                objConn.Close();
            }

        }

        /// <summary>
        /// dataset 生成excel
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="oldds"></param>
        private static void DSToExcel_(string Path, DataSet oldds)
        {
            OleDbConnection myConn = null;
            try
            {
                //先得到汇总Excel的DataSet 主要目的是获得Excel在DataSet中的结构    
                string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ;Data Source =" + Path + "; Extended Properties=Excel 8.0";
                myConn = new OleDbConnection(strCon);
                string strCom = "select * from [Sheet1$]";
                myConn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                System.Data.OleDb.OleDbCommandBuilder builder = new OleDbCommandBuilder(myCommand);
                //QuotePrefix和QuoteSuffix主要是对builder生成InsertComment命令时使用。    
                builder.QuotePrefix = "[";
                //获取insert语句中保留字符（起始位置）    
                builder.QuoteSuffix = "]";
                //获取insert语句中保留字符（结束位置）    
                DataSet newds = new DataSet();
                myCommand.Fill(newds, "Table1");
                for (int i = 0; i < oldds.Tables[0].Rows.Count; i++)
                {
                    //在这里不能使用ImportRow方法将一行导入到news中，
                    //因为ImportRow将保留原来DataRow的所有设置(DataRowState状态不变)。
                    //在使用ImportRow后newds内有值，但不能更新到Excel中因为所有导入行的DataRowState!=Added    
                    DataRow nrow = newds.Tables["Table1"].NewRow();
                    for (int j = 0; j < newds.Tables[0].Columns.Count; j++)
                    {
                        nrow[j] = oldds.Tables[0].Rows[i][j];
                    }
                    newds.Tables["Table1"].Rows.Add(nrow);
                }
                myCommand.Update(newds, "Table1");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                myConn.Close();
            }
        }


        /// <summary>
        /// datatable 转换为excel
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="oldds"></param>
        public static void DTToExcel(string Path, DataTable oldds)
        {
            OleDbConnection objConn = null;
            try
            {
                objConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties=Excel 8.0;");
                using (objConn)
                {
                    objConn.Open();
                    using (OleDbCommand objCmd = new OleDbCommand())
                    {
                        string tablecolumnssql = "";
                        for (int i = 0; i < oldds.Columns.Count; i++)
                        {
                            tablecolumnssql += oldds.Columns[i].ColumnName + " char(255),";
                        }
                        tablecolumnssql = tablecolumnssql.Trim(',');
                        //创建excel
                        objCmd.Connection = objConn;
                        objCmd.CommandText = "CREATE TABLE Sheet1 (" + tablecolumnssql + ")";
                        objCmd.ExecuteNonQuery();
                        //objCmd.CommandText = "Insert into tablename (FirstName, LastName)" +
                        //    " values ('王', '二')";
                        //objCmd.ExecuteNonQuery();
                        //更新数据
                        string strCom = "select * from [Sheet1$]";
                        OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, objConn);
                        System.Data.OleDb.OleDbCommandBuilder builder = new OleDbCommandBuilder(myCommand);
                        //QuotePrefix和QuoteSuffix主要是对builder生成InsertComment命令时使用。    
                        builder.QuotePrefix = "[";
                        //获取insert语句中保留字符（起始位置）    
                        builder.QuoteSuffix = "]";
                        //获取insert语句中保留字符（结束位置）    
                        DataSet newds = new DataSet();
                        myCommand.Fill(newds, "Table1");
                        for (int i = 0; i < oldds.Rows.Count; i++)
                        {
                            //在这里不能使用ImportRow方法将一行导入到news中，
                            //因为ImportRow将保留原来DataRow的所有设置(DataRowState状态不变)。
                            //在使用ImportRow后newds内有值，但不能更新到Excel中因为所有导入行的DataRowState!=Added    
                            DataRow nrow = newds.Tables["Table1"].NewRow();
                            for (int j = 0; j < newds.Tables[0].Columns.Count; j++)
                            {
                                nrow[j] = oldds.Rows[i][j];
                            }
                            newds.Tables["Table1"].Rows.Add(nrow);
                        }
                        myCommand.Update(newds, "Table1");
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                objConn.Close();
            }

        }

        /// <summary>
        /// dataset 生成excel并抛出下载
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="U_ID"></param>
        public static void DataTableToExcelDown(DataTable dt, int U_ID)
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            string filepath = "/files/" + U_ID.ToString() + "/";
            string files = System.Web.HttpContext.Current.Server.MapPath(filepath + filename);
            string TPath = System.Web.HttpContext.Current.Server.MapPath(filepath);
            if (!Directory.Exists(TPath))
            {
                Directory.CreateDirectory(TPath);
            }
            //string sfile = System.Web.HttpContext.Current.Server.MapPath("/files/down.xls");
            //string tpath=System.Web.HttpContext.Current.Server.MapPath(filepath);
            //string tfilename = filename;
            //Common.FileCopy.FilesCopy(sfile, tpath, tfilename);
            DTToExcel(files, dt);
            filedown(files, filename);
        }
        public static void DataTableToExcelDown(DataTable dt, int U_ID, HttpResponse Response)
        {
            DataTableToExcelDown(dt, U_ID);
        }

        public static void filedown(string filePath, string fileName)
        {
            FileInfo fi = new FileInfo(filePath);//excelFile为文件在服务器上的地址
            HttpResponse contextResponse = HttpContext.Current.Response;
            contextResponse.Clear();
            contextResponse.Buffer = true;
            contextResponse.Charset = "GB2312"; //设置了类型为中文防止乱码的出现 
            contextResponse.AppendHeader("Content-Disposition", String.Format("attachment;filename={0}", fileName)); //定义输出文件和文件名 
            contextResponse.AppendHeader("Content-Length", fi.Length.ToString());
            contextResponse.ContentEncoding = Encoding.Default;
            contextResponse.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。 
            if (fi.Length > 0)
            {
                FileStream sr = new FileStream(fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                int size = 1024;//设置每次读取长度。
                for (int i = 0; i < fi.Length / size + 1; i++)
                {
                    byte[] buffer = new byte[size];
                    int length = sr.Read(buffer, 0, size);
                    contextResponse.OutputStream.Write(buffer, 0, length);
                }
                sr.Close();
            }
            else
            {
                contextResponse.WriteFile(fi.FullName);
            }
            contextResponse.Flush();
            fi.Delete();
            contextResponse.End();
        }

    }
}
