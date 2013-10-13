using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PSWFWeb
{
    using System;
    using Nancy.Hosting.Self;

    public class PSWFWeb
    {
        public const string ConfigFilename = "config.xml";

        public void LoadConfig()
        {
            string exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string configPath = System.IO.Path.Combine(exeDir, ConfigFilename);
            
            var config = new PSWFWebConfig();

            var serializer = new XmlSerializer(typeof (PSWFWebConfig));
            using (var io = System.IO.File.OpenRead(configPath))
            {
                config = (PSWFWebConfig) serializer.Deserialize(io);
            }

            // load default or error

            if (string.IsNullOrWhiteSpace(config.WorkflowScriptsDirectory))
            {
                config.WorkflowScriptsDirectory = System.IO.Path.Combine(exeDir, "workflows");
            } else if (!System.IO.Path.IsPathRooted(config.WorkflowScriptsDirectory))
            {
                // add relative path from the path of exe, for sure
                config.WorkflowScriptsDirectory = System.IO.Path.Combine(exeDir, config.WorkflowScriptsDirectory);
            }

            PSWFWebConfig.Current = config;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
                {
                    start(cancellationToken);
                }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void start(CancellationToken cancellationToken)
        {
            var uri = new Uri(PSWFWebConfig.Current.Uri);

            using (var host = new NancyHost(uri))
            {
                host.Start();

                cancellationToken.WaitHandle.WaitOne();
            }
        }
    }
}
