using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using Funq;
using RedisServer.Tests;
using CommonLib;

namespace RedisServer
{
    //Server
    public class HelloService : Service
    {
        public object Any(Hello req)
        {
            return new HelloResponse { Result = "Hello, " + req.Name + " .Your Id is:" + GetNextNumberTest.GetNextNumber() + " "};
        }
    }

    public class ServerAppHost : AppHostHttpListenerBase
    {
        public ServerAppHost() : base("Test Server", typeof(HelloService).Assembly) { }

        public override void Configure(Container container)
        {
            base.Routes
                .Add<Hello>("/hello")
                .Add<Hello>("/hello/{Name}");

            var redisFactory = new PooledRedisClientManager("localhost:6379");
            container.Register<IRedisClientsManager>(redisFactory);
            var mqHost = new RedisMqServer(redisFactory, retryCount: 2);
            
            //Listens for 'Hello' messages sent with: mqClient.Publish(new Hello { ... })
            mqHost.RegisterHandler<Hello>(base.ExecuteMessage);
            mqHost.Start(); //Starts listening for messages
        }
    }
}
