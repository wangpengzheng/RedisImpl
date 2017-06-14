using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RedisServer.Utils;

namespace RedisServer.Tests
{
    class GetNextNumberTest
    {
        public static long GetNextNumber(string name)
        {
            long ret = -1;
            MysqlConnectionHelper helper = new MysqlConnectionHelper();

            Console.WriteLine("Server get the hello message with name :{0}", name);

            //for (int i = 0; i < 20; i++)
            //{
                MySqlDataReader reader = helper.ReadBySql("select TestId from employees where employeeNumber=1002");

                reader.Read();
            if (reader.HasRows)
            {
                // Get number
                ret = reader.GetInt64(0);
                reader.Close();
                // Increase number
                ret += 1;

                // Use number -- Insert to another table
                string insertCommand = string.Format("insert into concurrent(Time,Number) values('{0}',{1})",
                    DateTime.Now.ToString("hh:mm:ss"), ret);
                helper.ExecuteSqlcom(insertCommand);

                // Increase number to record
                helper.ExecuteSqlcom(string.Format("update employees set TestId={0} where employeeNumber=1002", ret));
                helper.DisposeConnection();
            }
            //}

            return ret;
        }
    }
}
