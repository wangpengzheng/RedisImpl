using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSWithRedis
{
    public class EventLogHelper
    {
        public System.Diagnostics.EventLog eventLogForService;
        private int eventId = 1;

        public EventLogHelper()
        {
            eventLogForService = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLogForService.Source = "MySource";
            eventLogForService.Log = "MyNewLog";
        }
    }
}
