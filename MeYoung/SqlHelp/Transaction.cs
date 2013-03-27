using System;
using System.Collections.Generic;
using System.Text;

namespace System.Data.Transaction
{
    public class Transaction
    {
        #region 字段

        private SqlHelp sqlHelper = null;
        private string dbType = string.Empty;
        private string conString = string.Empty;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Transaction()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStringsName">配置文件中，数据库连接字符串connectionStrings对应的name</param>
        public Transaction(string connectionStringsName)
        {
            conString = connectionStringsName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appSettingsKey_dbType">配置文件中，数据库类型AppSettings对应的key值</param>
        /// <param name="connectionStringsName">配置文件中，数据库连接字符串connectionStrings对应的name</param>
        public Transaction(string appSettingsKey_dbType, string connectionStringsName)
        {
            dbType = appSettingsKey_dbType;
            conString = connectionStringsName;
            sqlHelper = new SqlHelp(appSettingsKey_dbType, connectionStringsName);
        }
        #endregion


        /// <summary>
        /// 获得SqlHelp对象
        /// </summary>
        /// <returns></returns>
        public SqlHelp GetSqlHelp()
        {
            return this.sqlHelper;
        }


        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (string.IsNullOrEmpty(conString) && string.IsNullOrEmpty(dbType))
            {
                sqlHelper = new SqlHelp();
            }
            else if (!string.IsNullOrEmpty(conString) && string.IsNullOrEmpty(dbType))
            {
                sqlHelper = new SqlHelp(conString);
            }
            else if (string.IsNullOrEmpty(conString) && !string.IsNullOrEmpty(dbType))
            {
                sqlHelper = new SqlHelp(dbType, conString);
            }
            sqlHelper.BeginTransaction();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollBack()
        {
            sqlHelper.RollBack();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            sqlHelper.Commit();
        }
    }
}
