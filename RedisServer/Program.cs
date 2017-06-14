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
using CommonLib;

namespace RedisServer
{
    class Program
    {
        /// <summary>
        /// http://docs.servicestack.net/redis-mq
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Test Operation
            // GetNextNumberTest.GetNextNumber("Client 1");

            // Start Services, with webservices.
            StartRedisServices();

            // Start simple MQ Service
            //StartSimpleService();

            // Connection Test
            //ReadSQLTest();

            //Prevent server from exiting (when running in ConsoleApp)
            Console.ReadLine();
        }

        public static void StartSimpleService()
        {
            var redisFactory = new PooledRedisClientManager("localhost:6379");
            var mqHost = new RedisMqServer(redisFactory, retryCount: 2);

            //Server - MQ Service Impl:
            //mqHost.RegisterHandler<Hello>(m =>
            //    new HelloResponse { Result = "Hello, " + m.GetBody().Name + " .Your Id is:" + GetNextNumberTest.GetNextNumber() + " " });

            // Only execute hello message without response.
            mqHost.RegisterHandler<Hello>(m => GetNextNumberTest.GetNextNumber(m.GetBody().Name));

            mqHost.Start();

            Console.WriteLine("Server running. Press enter to terminate...");
        }

        static void StartRedisServices()
        {
            var serverURL = "http://localhost:1401/";
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
