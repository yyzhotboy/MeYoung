using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace System.Data
{
    /// <summary>
    /// 工厂类
    /// </summary>
    sealed class DBFactory
    {
        private static volatile DBFactory singleFactory = null;
        private static readonly object obj = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        private DBFactory()
        { }

        /// <summary>
        /// 获得DBFactory类的实例
        /// </summary>
        /// <returns>DBFactory类实例</returns>
        public static DBFactory NewDBFactory()
        {
            if (singleFactory == null)
            {
                lock (obj)
                {
                    if (singleFactory == null)
                    {
                        singleFactory = new DBFactory();
                    }
                }
            }
            return singleFactory;
        }
        /// <summary>
        /// 创建简单工厂实例
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns>IDBFactory</returns>
        public IDBFactory CreateFactory(string dbType)
        {
            IDBFactory db = null;
            switch (dbType)
            {
                case "sql":
                    db = new SqlFactory();
                    break;
                case "oledb":
                    db = new OleDbFactory();
                    break;
                case "oracle":
                    db = new OleDbFactory();
                    break;
                case "access":
                    db = new OleDbFactory();
                    break;
            }
            return db;
        }



    }
}
