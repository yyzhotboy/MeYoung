using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Common.JSON
{
    public sealed class JSONHelper
    {
        private static MsJSONSerializer objSerializer;

        static JSONHelper()
        {
            JSONHelper.objSerializer = new MsJSONSerializer();
        }

        public JSONHelper()
        {
        }

        /// <summary>
        /// 对象转JSON
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>JSON格式的字符串</returns>
        public static string ObjectToJSON(object obj)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {
                return jss.Serialize(obj);
            }
            catch (Exception ex)
            {

                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }

        public static string DataTableToJson(DataTable dt)
        {
            if (dt != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("[");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stringBuilder.Append("{");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            stringBuilder.Append(string.Concat("\"", dt.Columns[j].ColumnName.ToString(), "\":", JSONHelper.objSerializer.Serialize(dt.Rows[i][j])));
                            if (j < dt.Columns.Count - 1)
                            {
                                stringBuilder.Append(",");
                            }
                        }
                        stringBuilder.Append("}");
                        if (i < dt.Rows.Count - 1)
                        {
                            stringBuilder.Append(",");
                        }
                    }
                }
                stringBuilder.Append("]");
                return stringBuilder.ToString();
            }
            else
            {
                return "[]";
            }
        }
    }

}
