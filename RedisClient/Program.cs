using System;
using System.Threading;
using Funq;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using ServiceStack.Testing;
using CommonLib;
using RedisServer.Utils;

namespace RedisClient
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i < 20; i++)
            {
                Thread t = new Thread(new ThreadStart(DoSomething));
                t.Name = "TestThread" + i;
                t.Start();
            }

            Console.WriteLine("Client Finish");
            Console.ReadLine(); //Prevent self-hosted Console App from exiting
        }

        static void DoSomething()
        {
            var redisFactory = new PooledRedisClientManager("localhost:6379");
            var mqServer = new RedisMqServer(redisFactory, retryCount: 2);

            #region  Client - MQ Service Impl:

            ////Listens for 'HelloResponse' returned by the 'Hello' Service
            //mqServer.RegisterHandler<HelloResponse>(m => {

            //    var insertStr = "Received: " + m.GetBody().Result;
            //    string insertCommand = string.Format("insert into concurrent(Time,Log) values('{0}','{1}')",
            //        DateTime.Now.ToString("hh:mm:ss"), insertStr);

            //    MysqlConnectionHelper sqlHelper = new MysqlConnectionHelper();
            //    sqlHelper.ExecuteSqlcom(insertCommand);


            //    Console.WriteLine("Received: " + m.GetBody().Result);
            //    // See comments below
            //    // m.Options = (int)MessageOption.None;
            //    return null;
            //});

            //or to call an existing service with:
            //mqServer.RegisterHandler<HelloResponse>(m =>   
            //    this.ServiceController.ExecuteMessage(m));

            mqServer.Start(); //Starts listening for messages

            #endregion
            
            using (var mqClient = mqServer.CreateMessageQueueClient())
            { 
                mqClient.Publish(new Hello { Name = "Client 1" });
            }

            Console.WriteLine("Client running.  Press any key to terminate...");
        }
        
    }
}
