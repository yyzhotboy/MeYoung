using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;

namespace System
{
    public static class Static
    {

        /// <summary>
        /// 参数化查询前缀标志
        /// </summary>
        public static string CSCX
        {
            get
            {
                string s = "";
                string dbType = ConfigurationManager.AppSettings["dbType"].ToLower().Trim();
                if (dbType.Equals("sql"))
                    s = "@";
                else if (dbType.Equals("oracle"))
                    s = ":";
                return s;
            }
        }

    }
}
