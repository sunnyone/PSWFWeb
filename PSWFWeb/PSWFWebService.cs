using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSWFWeb
{
    partial class PSWFWebService : ServiceBase
    {
        public PSWFWebService()
        {
            InitializeComponent();
        }

        private Task task = null;
        private CancellationTokenSource cancellationTokenSource;
        
        protected override void OnStart(string[] args)
        {
            cancellationTokenSource = new CancellationTokenSource();

            var instance = new PSWFWeb();
            try
            {
                instance.LoadConfig();
                task = instance.StartAsync(cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                EventLogUtil.WriteEventLog(EventLogEntryType.Error, 
                    string.Format("Failed to start PSWFWeb: {0}", ex.ToString()));
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (task != null)
                {
                    cancellationTokenSource.Cancel();
                    task.Wait(TimeSpan.FromSeconds(5));
                }
            }
            catch (Exception ex)
            {
                EventLogUtil.WriteEventLog(EventLogEntryType.Error, 
                    string.Format("Failed to stop PSWFWeb: {0}", ex.ToString()));
            }
        }
    }
}
