using RedisServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WinSWithRedis
{
    [RunInstaller(true)]
    public partial class RedisService : ServiceBase
    {
        public EventLogHelper ehelper;

        // Windows Service Step by Step Learn: https://msdn.microsoft.com/en-us/library/zt39148a(v=vs.110).aspx
        public RedisService()
        {
            InitializeComponent();

            ehelper = new EventLogHelper();
        }

        protected override void OnStart(string[] args)
        {
            ehelper.eventLogForService.WriteEntry("In OnStart", EventLogEntryType.Warning);
            
            StartRedisServices();
        }

        /// <summary>
        /// Log Event when service stop
        /// </summary>
        protected override void OnStop()
        {
            ehelper.eventLogForService.WriteEntry("In onStop.", EventLogEntryType.Error);
        }

        /// <summary>
        /// Demo 1, Start a redis services
        /// </summary>
        public void StartRedisServices()
        {
            var serverURL = "http://localhost:1400/";
            var serverAppHost = new ServerAppHost();
            serverAppHost.Init();
            serverAppHost.Start(serverURL);
            Console.WriteLine("Server running. Server url is: {0}  Press enter to terminate...", serverURL);
        }

        /// <summary>
        /// Demo 2, Start a regular services
        /// </summary>
        public void StartRegularTaskServices()
        {
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            ehelper.eventLogForService.WriteEntry("Monitoring the System", EventLogEntryType.Information, 10);
        }
    }
}
