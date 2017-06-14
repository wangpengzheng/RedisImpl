using CommonLib;
using ServiceStack.Redis;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisKeyValueCommonOperation
{
    class RedisCommonOperation
    {
        public static void SetObject()
        {
            using (var redis = new RedisClient("172.19.18.44:6379"))
            //using (var redis = new RedisClient("localhost:6379"))
            {
                var redisUsers = redis.As<Person>();

                var rick = new Person() { id = redisUsers.GetNextSequence(), name = "Rick @code with internet" };
                var becky = new Person() { id = redisUsers.GetNextSequence(), name = "becky @ becky.com" };

                redisUsers.Store(rick);
                redisUsers.Store(becky);

                Console.WriteLine("Save finished");
                Console.ReadLine();
            }
        }

        public static void GetObject()
        {
            //using (var redis = new RedisClient("172.19.18.44:6379"))
            using (var redis = new RedisClient("localhost:6379"))
            {
                var redisUsers = redis.As<Person>();
                
                var allThePeople = redisUsers.GetAll();

                Console.WriteLine("Object as below,");
                Console.WriteLine(allThePeople.Dump());
                Console.ReadLine();
            }
        }

        public static void GetObjectPerformanceTest()
        {
            using (var redis = new RedisClient("172.19.18.44:6379"))
            //using (var redis = new RedisClient("localhost:6379"))
            {
                var redisUsers = redis.As<Person>();

                DateTime startTime = DateTime.Now;
                var testCount = 100;

                for (int i = 0; i < testCount; i++)
                {
                    var allThePeople = redisUsers.GetAll();
                }

                DateTime endTime = DateTime.Now;

                var diffTotal = endTime.Subtract(startTime);
                diffTotal = new TimeSpan(diffTotal.Ticks / testCount);

                Console.WriteLine("Get object Time: {0}:{1}:{2}s:{3}mm", diffTotal.Hours, diffTotal.Minutes, diffTotal.Seconds, diffTotal.Milliseconds);
            }
        }
    }
}
