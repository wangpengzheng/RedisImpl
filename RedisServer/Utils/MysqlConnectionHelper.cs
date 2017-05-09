using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RedisServer.Utils
{
    public class MysqlConnectionHelper
    {
        private MySqlConnection mysqlcon = null;

        /// <summary>
        /// Sample data: http://www.mysqltutorial.org/mysql-sample-database.aspx
        /// </summary>
        public MysqlConnectionHelper()
        {
            string M_str_sqlcon = "server=localhost;user id=root;password=root;database=classicmodels"; //根据自己的设置
            mysqlcon = new MySqlConnection(M_str_sqlcon);
            mysqlcon.Open();
        }

        public void DisposeConnection()
        {
            if (mysqlcon != null)
            {
                mysqlcon.Dispose();
                mysqlcon.Close();
            }
        }
        
        #region  执行MySqlCommand命令
        /// <summary>
        /// 执行MySqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public void ExecuteSqlcom(string M_str_sqlstr)
        {
            mysqlcon.Open();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            mysqlcom.ExecuteNonQuery();
            mysqlcon.Dispose();
            mysqlcon.Close();
        }
        #endregion

        #region  创建MySqlDataReader对象
        /// <summary>
        /// 创建一个MySqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回MySqlDataReader对象</returns>
        public MySqlDataReader ReadBySql(string M_str_sqlstr)
        {
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            MySqlDataReader mysqlread = mysqlcom.ExecuteReader(CommandBehavior.CloseConnection);

            return mysqlread;
        }
        #endregion
    }
}
