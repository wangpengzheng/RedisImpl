using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using Funq;
using MySql.Data.MySqlClient;
using RedisServer.Tests;
using RedisServer.Utils;

namespace RedisServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test Operation
            //GetNextNumberTest.GetNextNumber();
            
            // Start Services
            StartRedisServices();

            // Connection Test
            //ReadSQLTest();

            //Prevent server from exiting (when running in ConsoleApp)
            Console.ReadLine();
        }

        static void StartRedisServices()
        {
            var serverURL = "http://localhost:1400/";
            var serverAppHost = new ServerAppHost();
            serverAppHost.Init();
            serverAppHost.Start(serverURL);
            Console.WriteLine("Server running. Server url is: {0}  Press enter to terminate...", serverURL);
        }

        static void ReadSQLTest()
        {
            MysqlConnectionHelper helper = new MysqlConnectionHelper();
            MySqlDataReader reader = helper.ReadBySql("select * from employees");

            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    Console.WriteLine("编号:" + reader.GetInt32(0) + "|姓名:" + reader.GetString(1) + " " + reader.GetString(2) +
                        "|Email:" + reader.GetString(4));
                }
            }

            helper.DisposeConnection();
        }
    }
}
