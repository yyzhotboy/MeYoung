using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// Utility 的摘要说明。
    /// </summary>
    public class Utility
    {

        #region 数据转换
        /// <summary>
        /// 返回对象obj的String值,obj为null时返回空值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>字符串。</returns>
        public static string ToObjectString(object obj)
        {
            return null == obj ? String.Empty : obj.ToString();
        }

        /// <summary>
        /// 将对象转换为数值(Int32)类型,转换失败返回-1。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Int32数值。</returns>
        public static int ToInt(object obj)
        {
            try
            {
                return int.Parse(ToObjectString(obj));
            }
            catch
            { return -1; }
        }

        /// <summary>
        /// 将对象转换为数值(Int32)类型。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>Int32数值。</returns>
        public static int ToInt(object obj, int returnValue)
        {
            try
            {
                return int.Parse(ToObjectString(obj));
            }
            catch
            { return returnValue; }
        }
        /// <summary>
        /// 将对象转换为数值(Long)类型,转换失败返回-1。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Long数值。</returns>
        public static long ToLong(object obj)
        {
            try
            {
                return long.Parse(ToObjectString(obj));
            }
            catch
            { return -1L; }
        }
        /// <summary>
        /// 将对象转换为数值(Long)类型。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>Long数值。</returns>
        public static long ToLong(object obj, long returnValue)
        {
            try
            {
                return long.Parse(ToObjectString(obj));
            }
            catch
            { return returnValue; }
        }
        /// <summary>
        /// 将对象转换为数值(Decimal)类型,转换失败返回-1。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Decimal数值。</returns>
        public static decimal ToDecimal(object obj)
        {
            try
            {
                return decimal.Parse(ToObjectString(obj));
            }
            catch
            { return -1M; }
        }

        /// <summary>
        /// 将对象转换为数值(Decimal)类型。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>Decimal数值。</returns>
        public static decimal ToDecimal(object obj, decimal returnValue)
        {
            try
            {
                return decimal.Parse(ToObjectString(obj));
            }
            catch
            { return returnValue; }
        }
        /// <summary>
        /// 将对象转换为数值(Double)类型,转换失败返回-1。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Double数值。</returns>
        public static double ToDouble(object obj)
        {
            try
            {
                return double.Parse(ToObjectString(obj));
            }
            catch
            { return -1; }
        }

        /// <summary>
        /// 将对象转换为数值(Double)类型。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>Double数值。</returns>
        public static double ToDouble(object obj, double returnValue)
        {
            try
            {
                return double.Parse(ToObjectString(obj));
            }
            catch
            { return returnValue; }
        }
        /// <summary>
        /// 将对象转换为数值(Float)类型,转换失败返回-1。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Float数值。</returns>
        public static float ToFloat(object obj)
        {
            try
            {
                return float.Parse(ToObjectString(obj));
            }
            catch
            { return -1; }
        }

        /// <summary>
        /// 将对象转换为数值(Float)类型。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>Float数值。</returns>
        public static float ToFloat(object obj, float returnValue)
        {
            try
            {
                return float.Parse(ToObjectString(obj));
            }
            catch
            { return returnValue; }
        }
        /// <summary>
        /// 将对象转换为数值(DateTime)类型,转换失败返回Now。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>DateTime值。</returns>
        public static DateTime ToDateTime(object obj)
        {
            try
            {
                DateTime dt = DateTime.Parse(ToObjectString(obj));
                if (dt > DateTime.MinValue && DateTime.MaxValue > dt)
                    return dt;
                return DateTime.Now;
            }
            catch
            { return DateTime.Now; }
        }

        /// <summary>
        /// 将对象转换为数值(DateTime)类型。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>DateTime值。</returns>
        public static DateTime ToDateTime(object obj, DateTime returnValue)
        {
            try
            {
                DateTime dt = DateTime.Parse(ToObjectString(obj));
                if (dt > DateTime.MinValue && DateTime.MaxValue > dt)
                    return dt;
                return returnValue;
            }
            catch
            { return returnValue; }
        }
        /// <summary>
        /// 从Boolean转换成byte,转换失败返回0。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Byte值。</returns>
        public static byte ToByteByBool(object obj)
        {
            string text = ToObjectString(obj).Trim();
            if (text == string.Empty)
                return 0;
            else
            {
                try
                {
                    return (byte)(text.ToLower() == "true" ? 1 : 0);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 从Boolean转换成byte。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>Byte值。</returns>
        public static byte ToByteByBool(object obj, byte returnValue)
        {
            string text = ToObjectString(obj).Trim();
            if (text == string.Empty)
                return returnValue;
            else
            {
                try
                {
                    return (byte)(text.ToLower() == "true" ? 1 : 0);
                }
                catch
                {
                    return returnValue;
                }
            }
        }
        /// <summary>
        /// 从byte转换成Boolean,转换失败返回false。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Boolean值。</returns>
        public static bool ToBoolByByte(object obj)
        {
            try
            {
                string s = ToObjectString(obj).ToLower();
                return s == "1" || s == "true" ? true : false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 从byte转换成Boolean。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>Boolean值。</returns>
        public static bool ToBoolByByte(object obj, bool returnValue)
        {
            try
            {
                string s = ToObjectString(obj).ToLower();
                return s == "1" || s == "true" ? true : false;
            }
            catch
            {
                return returnValue;
            }
        }
        #endregion

        #region 数据判断
        /// <summary>
        /// 判断文本obj是否为空值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Boolean值。</returns>
        public static bool IsEmpty(string obj)
        {
            return ToObjectString(obj).Trim() == String.Empty ? true : false;
        }

        /// <summary>
        /// 判断对象是否为正确的日期值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Boolean。</returns>
        public static bool IsDateTime(object obj)
        {
            try
            {
                DateTime dt = DateTime.Parse(ToObjectString(obj));
                if (dt > DateTime.MinValue && DateTime.MaxValue > dt)
                    return true;
                return false;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 判断对象是否为正确的Int32值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Int32值。</returns>
        public static bool IsInt(object obj)
        {
            try
            {
                int.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 判断对象是否为正确的Long值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Long值。</returns>
        public static bool IsLong(object obj)
        {
            try
            {
                long.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 判断对象是否为正确的Float值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Float值。</returns>
        public static bool IsFloat(object obj)
        {
            try
            {
                float.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 判断对象是否为正确的Double值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Double值。</returns>
        public static bool IsDouble(object obj)
        {
            try
            {
                double.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 判断对象是否为正确的Decimal值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Decimal值。</returns>
        public static bool IsDecimal(object obj)
        {
            try
            {
                decimal.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }
        #endregion

        #region 数据操作
        /// <summary>
        /// 去除字符串的所有空格。
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>字符串</returns>
        public static string StringTrimAll(string text)
        {
            string _text = ToObjectString(text);
            string returnText = String.Empty;
            char[] chars = _text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].ToString() != string.Empty)
                    returnText += chars[i].ToString();
            }
            return returnText;
        }

        /// <summary>
        /// 去除数值字符串的所有空格。
        /// </summary>
        /// <param name="numricString">数值字符串</param>
        /// <returns>String</returns>
        public static string NumricTrimAll(string numricString)
        {
            string text = ToObjectString(numricString).Trim();
            string returnText = String.Empty;
            char[] chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].ToString() == "+" || chars[i].ToString() == "-" || IsDouble(chars[i].ToString()))
                    returnText += chars[i].ToString();
            }
            return returnText;
        }

        /// <summary>
        /// 在数组中查找匹配对象类型
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="obj">对象</param>
        /// <returns>Boolean</returns>
        public static bool ArrayFind(Array array, object obj)
        {
            bool b = false;
            foreach (object obj1 in array)
            {
                if (obj.Equals(obj1))
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        /// <summary>
        /// 在数组中查找匹配字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="obj">对象</param>
        /// <param name="unUpLower">是否忽略大小写</param>
        /// <returns>Boolean</returns>
        public static bool ArrayFind(Array array, string obj, bool unUpLower)
        {
            bool b = false;
            foreach (string obj1 in array)
            {
                if (!unUpLower)
                {
                    if (obj.Trim().Equals(obj1.ToString().Trim()))
                    {
                        b = true;
                        break;
                    }
                }
                else
                {
                    if (obj.Trim().ToUpper().Equals(obj1.ToString().Trim().ToUpper()))
                    {
                        b = true;
                        break;
                    }
                }
            }
            return b;
        }
        /// <summary>
        /// 替换字符串中的单引号。
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <returns>String</returns>
        public static string ReplaceInvertedComma(string inputString)
        {
            return inputString.Replace("'", "''");
        }

        

        /// <summary>
        /// 判断两个字节数组是否具有相同值.
        /// </summary>
        /// <param name="bytea">字节1</param>
        /// <param name="byteb">字节2</param>
        /// <returns>Boolean</returns>
        public static bool CompareByteArray(byte[] bytea, byte[] byteb)
        {
            if (null == bytea || null == byteb)
            {
                return false;
            }
            else
            {
                int aLength = bytea.Length;
                int bLength = byteb.Length;
                if (aLength != bLength)
                    return false;
                else
                {
                    bool compare = true;
                    for (int index = 0; index < aLength; index++)
                    {
                        if (bytea[index].CompareTo(byteb[index]) != 0)
                        {
                            compare = false;
                            break;
                        }
                    }
                    return compare;
                }
            }
        }


        /// <summary>
        /// 日期智能生成。
        /// </summary>
        /// <param name="inputText">字符串</param>
        /// <returns>DateTime</returns>
        public static string BuildDate(string inputText)
        {
            try
            {
                return DateTime.Parse(inputText).ToShortDateString();
            }
            catch
            {
                string text = NumricTrimAll(inputText);
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string day = DateTime.Now.Day.ToString();
                int length = text.Length;
                if (length == 0)
                    return String.Empty;
                else
                {
                    if (length <= 2)
                        day = text;
                    else if (length <= 4)
                    {
                        month = text.Substring(0, 2);
                        day = text.Substring(2, length - 2);
                    }
                    else if (length <= 6)
                    {
                        year = text.Substring(0, 4);
                        month = text.Substring(4, length - 4);
                    }
                    else if (length > 6)
                    {
                        year = text.Substring(0, 4);
                        month = text.Substring(4, 2);
                        day = text.Substring(6, length - 6);
                    }
                    try
                    {
                        return DateTime.Parse(year + "-" + month + "-" + day).ToShortDateString();
                    }
                    catch
                    {
                        return String.Empty;
                    }
                }
            }
        }



        /// <summary>
        /// 检查文件是否真实存在。
        /// </summary>
        /// <param name="path">文件全名（包括路径）。</param>
        /// <returns>Boolean</returns>
        public static bool IsFileExists(string path)
        {
            try
            {
                return File.Exists(path);
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 检查目录是否真实存在。
        /// </summary>
        /// <param name="path">目录路径.</param>
        /// <returns>Boolean</returns>
        public static bool IsDirectoryExists(string path)
        {
            try
            {
                return Directory.Exists(Path.GetDirectoryName(path));
            }
            catch
            { return false; }
        }

        /// <summary>
        /// 查找文件中是否存在匹配行。
        /// </summary>
        /// <param name="fi">目标文件.</param>
        /// <param name="lineText">要查找的行文本.</param>
        /// <param name="lowerUpper">是否区分大小写.</param>
        /// <returns>Boolean</returns>
        public static bool FindLineTextFromFile(FileInfo fi, string lineText, bool lowerUpper)
        {
            bool b = false;
            try
            {
                if (fi.Exists)
                {
                    StreamReader sr = new StreamReader(fi.FullName);
                    string g = "";
                    do
                    {
                        g = sr.ReadLine();
                        if (lowerUpper)
                        {
                            if (ToObjectString(g).Trim() == ToObjectString(lineText).Trim())
                            {
                                b = true;
                                break;
                            }
                        }
                        else
                        {
                            if (ToObjectString(g).Trim().ToLower() == ToObjectString(lineText).Trim().ToLower())
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    while (sr.Peek() != -1);
                    sr.Close();
                }
            }
            catch
            { b = false; }
            return b;
        }


        /// <summary>
        /// 判断父子级关系是否正确。
        /// </summary>
        /// <param name="table">数据表。</param>
        /// <param name="columnName">子键列名。</param>
        /// <param name="parentColumnName">父键列名。</param>
        /// <param name="inputString">子键值。</param>
        /// <param name="compareString">父键值。</param>
        /// <returns></returns>
        public static bool IsRightParent(DataTable table, string columnName, string parentColumnName, string inputString, string compareString)
        {
            ArrayList array = new ArrayList();
            SearchChild(array, table, columnName, parentColumnName, inputString, compareString);
            return array.Count == 0;
        }

        // 内部方法
        private static void SearchChild(ArrayList array, DataTable table, string columnName, string parentColumnName, string inputString, string compareString)
        {
            DataView view = new DataView(table);
            view.RowFilter = parentColumnName + "='" + ReplaceInvertedComma(inputString.Trim()) + "'";//找出所有的子类。
            //查找表中的数据的ID是否与compareString相等，相等返回 false;不相等继续迭代。
            for (int index = 0; index < view.Count; index++)
            {
                if (Utility.ToObjectString(view[index][columnName]).ToLower() == compareString.Trim().ToLower())
                {
                    array.Add("1");
                    break;
                }
                else
                {
                    SearchChild(array, table, columnName, parentColumnName, Utility.ToObjectString(view[index][columnName]), compareString);
                }
            }
        }

        #endregion

        #region 日期

        /// <summary>
        /// 格式化日期类型，返回字符串
        /// </summary>
        /// <param name="dtime">日期</param>
        /// <param name="s">日期年月日间隔符号</param>
        /// <returns></returns>
        public static String Fomatdate(DateTime dtime, String s)
        {
            String datestr = "";
            datestr = dtime.Year.ToString() + s + dtime.Month.ToString().PadLeft(2, '0') + s + dtime.Day.ToString().PadLeft(2, '0');
            return datestr;
        }

        /// <summary>
        /// 返回日期差
        /// </summary>
        /// <param name="sdmin">开始日期</param>
        /// <param name="sdmax">结束日期</param>
        /// <returns>日期差：负数为失败</returns>
        public static int Datediff(DateTime sdmin, DateTime sdmax)
        {
            try
            {
                double i = 0;
                while (sdmin.AddDays(i) < sdmax)
                {
                    i++;
                }
                return Utility.ToInt(i);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 返回日期差
        /// </summary>
        /// <param name="sdmin">开始日期</param>
        /// <param name="sdmax">结束日期</param>
        /// <returns>日期差：负数为失败</returns>
        public static int Datediff(String sdmin, String sdmax)
        {
            try
            {
                DateTime dmin;
                DateTime dmax;
                dmin = DateTime.Parse(sdmin);
                dmax = DateTime.Parse(sdmax);
                double i = 0;
                while (dmin.AddDays(i) < dmax)
                {
                    i++;
                }
                return Utility.ToInt(i);
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// 比较两个日期，如果开始大于结束返回true
        /// </summary>
        /// <param name="p_strMinDate"></param>
        /// <param name="p_strMaxDate"></param>
        /// <returns></returns>
        public static bool CompareDate(string p_strMinDate, string p_strMaxDate)
        {
            if (p_strMinDate.Length > 0 && p_strMaxDate.Length > 0)
            {
                if (DateTime.Parse(p_strMinDate) > DateTime.Parse(p_strMaxDate))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 计算两个日期的时间间隔,返回的是时间间隔如果大于60秒则返回多少分多少秒，如果小于60秒，则返回秒
        /// </summary>
        /// <param name="DateTime1">第一个日期和时间</param>
        /// <param name="DateTime2">第二个日期和时间</param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string retVal = string.Empty;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                int minutes = ts.Minutes+1;
                int seconds = ts.Seconds+1;
                if (minutes > 1)
                {
                    retVal = minutes + "分钟";
                }
                else
                {
                    retVal = seconds + "秒";
                }
            }
            catch
            {

            }
            return retVal;
        }
        #endregion

        #region 转换用户输入

        /// <summary>
        /// 将用户输入的字符串转换为可换行、替换Html编码、无危害数据库特殊字符、去掉首尾空白、的安全方便代码。
        /// </summary>
        /// <param name="inputString">用户输入字符串</param>
        public static string ConvertStr(string inputString)
        {
            string retVal = inputString;
            //retVal=retVal.Replace("&","&amp;"); 
            retVal = retVal.Replace("\"", "&quot;");
            retVal = retVal.Replace("<", "&lt;");
            retVal = retVal.Replace(">", "&gt;");
            retVal = retVal.Replace(" ", "&nbsp;");
            retVal = retVal.Replace("  ", "&nbsp;&nbsp;");
            retVal = retVal.Replace("\t", "&nbsp;&nbsp;");
            retVal = retVal.Replace("\r", "<br>");
            return retVal;
        }

        public static string InputText(string inputString)
        {
            string retVal = inputString;
            retVal = ConvertStr(retVal);
            retVal = retVal.Replace("[url]", "");
            retVal = retVal.Replace("[/url]", "");
            return retVal;
        }


        /// <summary>
        /// 将html代码显示在网页上
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string OutputText(string inputString)
        {
            string retVal = System.Web.HttpUtility.HtmlDecode(inputString);
            retVal = retVal.Replace("<br>", "");
            retVal = retVal.Replace("&amp", "&;");
            retVal = retVal.Replace("&quot;", "\"");
            retVal = retVal.Replace("&lt;", "<");
            retVal = retVal.Replace("&gt;", ">");
            retVal = retVal.Replace("&nbsp;", " ");
            retVal = retVal.Replace("&nbsp;&nbsp;", "  ");
            return retVal;
        }

        public static string ToUrl(string inputString)
        {
            string retVal = inputString;
            retVal = ConvertStr(retVal);
            return Regex.Replace(retVal, @"\[url](?<x>[^\]]*)\[/url]", @"<a href=""$1"" target=""_blank"">$1</a>", RegexOptions.IgnoreCase);
        }

        public static string GetSafeCode(string str)
        {
            str = str.Replace("'", "");
            str = str.Replace(char.Parse("34"), ' ');
            str = str.Replace(";", "");
            return str;
        }

        #endregion


        //#region 弹出框

        ///// <summary>
        ///// 服务器端弹出alert对话框
        ///// </summary>
        ///// <param name="str_Message">提示信息,例子："请输入您姓名!"</param>
        ///// <param name="page">Page类</param>
        //public static void Alert(string str_Message,Page page)
        //{
        //    page.RegisterStartupScript("","<script>alert('"+str_Message+"');</script>");
        //}
        ///// <summary>
        ///// 服务器端弹出alert对话框
        ///// </summary>
        ///// <param name="str_Ctl_Name">获得焦点控件Id值,比如：txt_Name</param>
        ///// <param name="str_Message">提示信息,例子："请输入您姓名!"</param>
        ///// <param name="page">Page类</param>
        //public static void Alert(string str_Ctl_Name,string str_Message,Page page)
        //{
        //    page.RegisterStartupScript("","<script>alert('"+str_Message+"');document.forms(0)."+str_Ctl_Name+".focus(); document.forms(0)."+str_Ctl_Name+".select();</script>");
        //}
        ///// <summary>
        ///// 服务器端弹出confirm对话框,该函数有个弊端,必须放到响应事件的最后,目前没有妥善解决方案
        ///// </summary>
        ///// <param name="str_Message">提示信息,例子："您是否确认删除!"</param>
        ///// <param name="btn">隐藏Botton按钮Id值,比如：btn_Flow</param>
        ///// <param name="page">Page类</param>
        //public static void Confirm(string str_Message,string btn,Page page)
        //{
        //    page.RegisterStartupScript("","<script> if (confirm('"+str_Message+"')==true){document.forms(0)."+btn+".click();}</script>");
        //}
        ///// <summary>
        /////  服务器端弹出confirm对话框,询问用户准备转向相应操作，包括“确定”和“取消”时的操作
        ///// </summary>
        ///// <param name="str_Message">提示信息，比如："成功增加数据,单击\"确定\"按钮填写流程,单击\"取消\"修改数据"</param>
        ///// <param name="btn_Redirect_Flow">"确定"按钮id值</param>
        ///// <param name="btn_Redirect_Self">"取消"按钮id值</param>
        ///// <param name="page">Page类</param>
        //public static void Confirm(string str_Message,string btn_Redirect_Flow,string btn_Redirect_Self,Page page)
        //{
        //    page.RegisterStartupScript("","<script> if (confirm('"+str_Message+"')==true){document.forms(0)."+btn_Redirect_Flow+".click();}else{document.forms(0)."+btn_Redirect_Self+".click();}</script>");
        //}

        //#endregion


        /// <summary>
        /// 设置绑定到DataGrid的DataTable的记录行数，如不够则添加空行
        /// </summary>
        /// <param name="myDataTable">数据表</param>
        /// <param name="intPageCount">DataGrid分页时每页行数</param>
        public static void SetTableRows(DataTable myDataTable, int intPageCount)
        {
            try
            {
                int intTemp = myDataTable.Rows.Count % intPageCount;
                if ((myDataTable.Rows.Count == 0) || (intTemp != 0))
                {
                    for (int i = 0; i < (intPageCount - intTemp); i++)
                    {
                        DataRow myDataRow = myDataTable.NewRow();
                        myDataTable.Rows.Add(myDataRow);
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        public static string GetGuid(string guid)
        {
            return guid.Replace("-", "");
        }

        public static string ReadConfig(string filePath)
        {
            return System.Configuration.ConfigurationManager.AppSettings[filePath];
        }

        #region   字符串长度区分中英文截取
        /// <summary>   
        /// 截取文本，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="length">需计算长度的字符串</param>
        /// <returns>string</returns>
        public static string GetSubString(string str, int length)
        {
            string temp = str;
            int j = 0;
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                {
                    j += 2;
                }
                else
                {
                    j += 1;
                }
                if (j <= length)
                {
                    k += 1;
                }
                if (j > length)
                {
                    return temp.Substring(0, k) + "...";
                }
            }
            return temp;
        }
        /// <summary>
        /// 年比较大小,如果开始大于结束，返回true
        /// </summary>
        /// <param name="p_strMinYear"></param>
        /// <param name="p_strMaxYear"></param>
        /// <returns>如果开始大于结束，返回true</returns>
        public static bool YearCompareValue(string p_strMinYear, string p_strMaxYear)
        {
            if (p_strMinYear.Length > 0 && p_strMaxYear.Length > 0)
            {
                int iMinYear = int.Parse(p_strMinYear);
                int iMaxYear = int.Parse(p_strMaxYear);
                if (iMinYear > iMaxYear)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 按用户输入的日时分转换成小时
        /// </summary>
        /// <param name="p_iInput"></param>
        /// <param name="p_iType">1天数，2小时，3分钟</param>
        /// <returns></returns>
        public static decimal TimeConvertToHour(int p_iInput, int p_iType)
        {
            decimal retVal=0;
            switch (p_iType)
            { 
                case 1:
                    retVal = p_iInput * 24;
                    break;
                case 2:
                    retVal = p_iInput;
                    break;
                case 3:
                    retVal = (decimal)p_iInput / 60;
                    break;
                default:
                    retVal = -1;
                    break;
            }
            return retVal;
        }
        /// <summary>
        /// 根据小时的值转换为天数或小时或分钟
        /// </summary>
        /// <param name="p_iInput"></param>
        /// <returns></returns>
        public static int HourConvertToTime(decimal p_iInput)
        {
            int retVal = 0;
            if (p_iInput < 1)
            {
                retVal = Convert.ToInt32(p_iInput * 60);
            }
            else
            {
                if (p_iInput >= 1 && p_iInput <= 23)
                {
                    retVal = Convert.ToInt32(p_iInput);
                }
                else
                {
                    retVal = Convert.ToInt32(p_iInput/24);
                }
            }
            return retVal;
        }

        /// <summary>
        /// 根据小时的值转换为天数或小时或分钟
        /// </summary>
        /// <param name="p_iInput"></param>
        /// <returns></returns>
        public static string HourConvertToStrTime(decimal p_iInput)
        {
            string retVal = string.Empty;
            if (p_iInput == -1) //-1为永不过期
            {
                retVal = "永久";
            }
            else
            {
                if (p_iInput < 1)
                {
                    retVal = Convert.ToInt32(p_iInput * 60) + "分钟";
                }
                else
                {
                    if (p_iInput >= 1 && p_iInput <= 23)
                    {
                        retVal = Convert.ToInt32(p_iInput)+"小时";
                    }
                    else
                    {
                        retVal = Convert.ToInt32(p_iInput / 24)+"天";
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// 根据小时的值转换为时间
        /// </summary>
        /// <param name="p_iInput"></param>
        /// <returns></returns>
        public static DateTime HourConvertToDateTime(decimal p_iInput)
        {
            DateTime retVal = DateTime.Now;
            if (p_iInput == -1)  //-1为永不过期,加20年
            {
                retVal = DateTime.Now.AddYears(20);
            }
            else
            {
                retVal=DateTime.Now.AddHours(Convert.ToDouble(p_iInput));
                //if (p_iInput < 1)
                //{
                //    retVal = DateTime.Now.AddMinutes(Convert.ToInt32(p_iInput * 60));
                //}
                //else
                //{
                //    if (p_iInput >= 1 && p_iInput <= 23)
                //    {
                //        retVal = DateTime.Now.AddHours(Convert.ToInt32(p_iInput));
                //    }
                //    else
                //    {
                //        retVal = DateTime.Now.AddDays(Convert.ToInt32(p_iInput / 24));
                //    }
                //}
            }
            return retVal;
        }
        /// <summary>
        /// 实现发表的时间显示为几个月,几天前,几小时前,几分钟前,或几秒前
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }

        }    
        #endregion
    }
}
