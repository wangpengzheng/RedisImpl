using ConsoleTest.Util;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using RedisKeyValueCommonOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisImpl.Demo
{
    class PerformanceTest
    {
        public static void SetReadConifgFromRedis()
        {
            DateTime startTime = DateTime.Now;

            RedisCommonOperation.SetObject();

            DateTime SetTime = DateTime.Now;

            //RedisCommonOperation.GetObject();
            RedisCommonOperation.GetObjectPerformanceTest();

            DateTime endTime = DateTime.Now;

            var diffSet = SetTime.Subtract(startTime);
            var diffGet = endTime.Subtract(SetTime);
            var diffTotal = endTime.Subtract(startTime);

            // 分段来检查性能问题
            Console.WriteLine("Redis Test");
            Console.WriteLine("Set object time: {0}:{1}:{2}:{3}", diffSet.Hours, diffSet.Minutes, diffSet.Seconds, diffSet.Milliseconds);
            Console.WriteLine("Get object Time: {0}:{1}:{2}:{3}", diffGet.Hours, diffGet.Minutes, diffGet.Seconds, diffGet.Milliseconds);
            Console.WriteLine("Total Time: {0}:{1}:{2}:{3}", diffTotal.Hours, diffTotal.Minutes, diffTotal.Seconds, diffTotal.Milliseconds);
            Console.Read();
        }

        public static void ReadConfigFromDatabaseByOrgService()
        {
            DateTime startTime = DateTime.Now;

            OrganizationServiceProxy orgp = DemoDataProvider.GetOrganizationService();

            DateTime buildConnectionTime = DateTime.Now;

            Entity curAccount = DemoDataProvider.GetTestAccountEntity(orgp);
            //Entity curAccount = DemoDataProvider.GetTestAccountEntityByID(orgp);

            DateTime endTime = DateTime.Now;

            var diffConnection = buildConnectionTime.Subtract(startTime);
            var diffQuery = endTime.Subtract(buildConnectionTime);
            var diffTotal = endTime.Subtract(startTime);
            
            // 分段来检查性能问题
            Console.WriteLine("Orgnization Services Test");
            Console.WriteLine("Establish connection Time: {0}:{1}:{2}:{3}", diffConnection.Hours, diffConnection.Minutes, diffConnection.Seconds, diffConnection.Milliseconds);
            Console.WriteLine("Query Time: {0}:{1}:{2}:{3}", diffQuery.Hours, diffQuery.Minutes, diffQuery.Seconds, diffQuery.Milliseconds);
            Console.WriteLine("Total Time: {0}:{1}:{2}:{3}", diffTotal.Hours, diffTotal.Minutes, diffTotal.Seconds, diffTotal.Milliseconds);
            Console.Read();
        }

        public static void ReadConfigFromDatabaseByWebAPI()
        {
            DateTime startTime = DateTime.Now;
            
            string curAccount = DemoDataProvider.GetTestAccountEntityByID_WebAPI();

            DateTime endTime = DateTime.Now;
            
            var diffTotal = endTime.Subtract(startTime);
            
            // 分段来检查性能问题
            Console.WriteLine("WebAPI Test");
            Console.WriteLine("Total Time: {0}:{1}:{2}:{3}", diffTotal.Hours, diffTotal.Minutes, diffTotal.Seconds, diffTotal.Milliseconds);
            Console.Read();
        }
    }
}
