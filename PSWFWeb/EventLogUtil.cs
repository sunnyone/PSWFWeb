using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PSWFWeb
{
    public static class EventLogUtil
    {
        private const string eventSourceName = "PSWFWeb";

        public static void CreateEventSourceIfNotExists()
        {
            if (!EventLog.SourceExists(eventSourceName))
                EventLog.CreateEventSource(eventSourceName, "Application");
        }
        
        public static void WriteEventLog(EventLogEntryType type, string message)
        {
            EventLog.WriteEntry(eventSourceName, message, type);
        }
    }
}
