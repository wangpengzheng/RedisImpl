using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack.Text;
using CommonLib;
using ServiceStack.Messaging.Redis;
using RedisImpl.Demo;
using RedisKeyValueCommonOperation;

namespace RedisImpl
{
    class Program
    {
        static void Main(string[] args)
        {
            //PerformanceTest.ReadConfigFromDatabaseByOrgService();

            //PerformanceTest.ReadConfigFromDatabaseByWebAPI();

            //PerformanceTest.SetReadConifgFromRedis();

            //RedisCommonOperation.GetObjectPerformanceTest();

            DistributedLockWithRedis.StartCounter();

            Console.ReadLine();
        }
    }
}
