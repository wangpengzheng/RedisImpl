using ConsoleTest.Util;
using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisImpl.Demo
{
    /// <summary>
    /// Achieving High Performance, Distributed Locking with Redis
    /// https://github.com/ServiceStack/ServiceStack.Redis/wiki/RedisLocks
    /// </summary>
    public class DistributedLockWithRedis
    {
        public static void StartCounter()
        {
            //The number of concurrent clients to run
            const int noOfClients = 20;
            var asyncResults = new List<IAsyncResult>(noOfClients);
            for (var i = 1; i <= noOfClients; i++)
            {
                var clientNo = i;
                var actionFn = (Action)delegate
                {
                    var redisClient = new RedisClient(Constant.RedisConnectionIP);
                    using (redisClient.AcquireLock("testlock"))
                    {
                        Console.WriteLine("client {0} acquired lock", clientNo);
                        var counter = redisClient.Get<int>("atomic-counter");

                        //Add an artificial delay to demonstrate locking behaviour
                        Thread.Sleep(100);

                        redisClient.Set("atomic-counter", counter + 1);
                        Console.WriteLine("client {0} released lock, current Counter {1}", clientNo, counter + 1);
                    }
                };

                //Asynchronously invoke the above delegate in a background thread
                asyncResults.Add(actionFn.BeginInvoke(null, null));
            }

            //Wait at most 1 second for all the threads to complete
            asyncResults.WaitAll(TimeSpan.FromSeconds(3));

            //Print out the 'atomic-counter' result
            using (var redisClient = new RedisClient(Constant.RedisConnectionIP))
            {
                var counter = redisClient.Get<int>("atomic-counter");
                Console.WriteLine("atomic-counter after 3 sec: {0}", counter);
            }

            /*Output:
            client 1 acquired lock
            client 1 released lock
            client 3 acquired lock
            client 3 released lock
            client 4 acquired lock
            client 4 released lock
            client 5 acquired lock
            client 5 released lock
            client 2 acquired lock
            client 2 released lock
            atomic-counter after 1sec: 5
            */
        }
    }
}
