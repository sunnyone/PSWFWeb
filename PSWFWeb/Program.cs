using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSWFWeb
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "/password":
                        generateAndOutputPasswordHash(args[1]);
                        break;
                    case "/service":
                        runService();
                        break;
                    default:
                        showUsage();
                        break;
                }
                return;
            }

            runConsole();
        }

        private static void showUsage()
        {
            System.Console.WriteLine("/password PASSWORD  -- Generate password for config.");
            System.Console.WriteLine();

            System.Console.WriteLine("Check http access is permitted with netsh command like 'netsh http show urlacl'");
            System.Console.WriteLine("To permit access, execute command like: 'netsh http add urlacl url=http://127.0.0.1:3579/ user=USERNAME'");
            System.Console.WriteLine("");

            System.Console.WriteLine("To install/uninstall as service, execute this command by administrator: ");

            string location = Assembly.GetExecutingAssembly().Location;
            string[] serviceArgs = new string[] { location, "/service",  };
            string serviceArgsQuoted = string.Join(" ", serviceArgs.Select(x => "\\\"" + x + "\\\""));

            string installCommand = string.Format("sc create PSWFWeb displayname= \"PSWFWeb\" binpath= \"{0}\" depend= Tcpip start= auto obj= YOUR-MACHINE\\YOUR-USERNAME password= YOUR-PASSWORD", serviceArgsQuoted);
            string uninstallCommand = string.Format("sc delete PSWFWeb");
            System.Console.WriteLine("  install: \n" + installCommand);
            System.Console.WriteLine("  uninstall: \n" + uninstallCommand);
        }

        private static void generateAndOutputPasswordHash(string password)
        {
            System.Console.WriteLine("Password hash is:");
            string hash = PasswordUtil.GeneratePasswordHash(password);
            System.Console.WriteLine(hash);
        }

        private static void runConsole()
        {
            var instance = new PSWFWeb();

            try
            {
                instance.LoadConfig();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Failed to load config from {0}: {1}", PSWFWeb.ConfigFilename, ex.ToString());
                return;
            }

            using (var cts = new CancellationTokenSource())
            {
                System.Console.WriteLine("Your application is running on " + PSWFWebConfig.Current.Uri);

                Task task = instance.StartAsync(cts.Token);

                System.Console.WriteLine("To see more options, use /help.");
                System.Console.WriteLine("Press any [Enter] to close the host.");
                System.Console.ReadLine();

                cts.Cancel();
                task.Wait(TimeSpan.FromSeconds(5));
            }
        }

        private static void runService()
        {
            EventLogUtil.CreateEventSourceIfNotExists();

            var service = new PSWFWebService();

            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] { service };
            ServiceBase.Run(servicesToRun);
        }
    }
}
