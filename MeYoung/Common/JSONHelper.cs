using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;

namespace Common
{
    /// <summary>
    /// FileName: JSONHelper.cs
    /// Corporation:
    /// Description:JSON格式数据转换助手类
    /// 1.将List<T>类型的数据转换为JSON格式
    /// 2.将T类型对象转换为JSON格式对象
    /// 3.将JSON格式对象转换为T类型对象
    /// </summary>
    public static class JSONHelper
    {
        static StringBuilder json = new StringBuilder();
        /// <summary>
        /// 转换对象为JSON格式数据
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>字符格式的JSON数据</returns>
        public static string GetJSON<T>(object obj)
        {
            string result = String.Empty;
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    result = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// 转换List<T>的数据为JSON格式
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="vals">列表值</param>
        /// <returns>JSON格式数据</returns>
        public static string JSON<T>(List<T> vals)
        {
            System.Text.StringBuilder st = new System.Text.StringBuilder();
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

                foreach (T city in vals)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        s.WriteObject(ms, city);
                        st.Append(System.Text.Encoding.UTF8.GetString(ms.ToArray()));
                        st.Append(",");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return st.ToString().Trim(',');
        }
        /// <summary>
        /// JSON格式字符转换为List<T>类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<T> ParseFormByJsonToList<T>(string jsonStr)
        {
            T obj = Activator.CreateInstance<T>();
            using (System.IO.MemoryStream ms =
            new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonStr)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<T>));
                return (List<T>)serializer.ReadObject(ms);
            }
        }
        /// <summary>
        /// JSON格式字符转换为T类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ParseFormByJson<T>(string jsonStr)
        {
            T obj = Activator.CreateInstance<T>();
            using (System.IO.MemoryStream ms =
            new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonStr)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }
        /// <summary>
        /// 将dataset转换为json combobox适用
        /// </summary>
        /// <param name="ds">ds</param>
        /// <returns></returns>
        public static string Dtb2Json(DataSet ds)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            System.Collections.ArrayList dic = new System.Collections.ArrayList();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                dic.Add(drow);

            }
            //var griddata = new { Rows = dic };
            //序列化  
            return jss.Serialize(dic);
        }


        /// <summary>
        /// 将dataTable转换为json tree适用
        /// </summary>
        /// <param name="dt">所要传的DataTable</param>
        /// <param name="parentid">parentID（父ID）</param>
        /// <param name="parentname">parentName（父Name）</param>
        /// <param name="childid">childID（子ID）</param>
        /// <param name="childname">childName(子Name)</param>
        /// <returns></returns>
        public static String getJson(DataTable dt, string parentid, string parentname, string childid, string childname)
        {
            json = new StringBuilder();
            string str = "";
            string str1 = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!str.Contains(dt.Rows[i][parentid].ToString() + ","))
                {
                    str += dt.Rows[i][parentid].ToString() + ",";
                    Dtb2JsonTree(dt, dt.Rows[i][parentid].ToString(), dt.Rows[i][parentname].ToString(), parentid, childid, childname);
                }
            }
            str1 = json.ToString();
            //string str1 = Dtb2JsonTree(dt, value,text);
            return ("[" + str1 + "]").Replace(",]", "]");
        }

        private static void Dtb2JsonTree(DataTable dt, string value, string text, string parentid, string childid, string childname)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            if (hasChild(dt, value, parentid, childid))
            {
                json.Append("{\"id\":\"");
                json.Append(value + "\"");
                json.Append(",\"text\":\"");
                json.Append(text + "\"");
                json.Append(",\"children\":[");
                DataRow[] childList = getChildList(dt, value, parentid, childid);
                foreach (DataRow dr in childList)
                {
                    Dtb2JsonTree(dt, dr[childid].ToString(), dr[childname].ToString(), parentid, childid, childname);
                }
                json.Append("]},");
            }
            else
            {
                json.Append("{\"id\":\"");
                json.Append(value + "\"");
                json.Append(",\"text\":\"");
                json.Append(text + "\"");
                json.Append("},");
            }
            //序列化  
        }


        private static bool hasChild(DataTable dt, string node, string parentid, string childid)
        {
            return getChildList(dt, node, parentid, childid).Length > 0 ? true : false;
        }

        private static DataRow[] getChildList(DataTable dt, string node, string parentid, string childid)
        {  //得到子节点列表   
            DataRow[] dataRows = dt.Select("");
            if (node.Length > 0)
                dataRows = dt.Select(parentid + "=" + node + " and " + childid + " is not null");
            return dataRows;
        }
    }
}
