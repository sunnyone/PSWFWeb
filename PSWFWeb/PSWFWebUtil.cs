using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace PSWFWeb
{
    public static class PSWFWebUtil
    {
        public static List<string> ListWorkflowScripts()
        {
            var dir = PSWFWebConfig.Current.WorkflowScriptsDirectory;

            return Directory.GetFiles(dir, "*.ps1", SearchOption.TopDirectoryOnly)
                .Select(path => Path.GetFileNameWithoutExtension(path))
                .ToList();
        }

        private static PowerShell createPowerShell()
        {
            InitialSessionState iss = InitialSessionState.CreateDefault();
            iss.ImportPSModule(new string[] { "PSWorkflow" });

            return PowerShell.Create(iss);
        }

        public static List<JobData> GetJobs()
        {
            using (var ps = createPowerShell())
            {
                ps.AddCommand("Get-Job");

                ps.AddCommand("Sort-Object");
                ps.AddParameter("Property", new string[] {"PSEndtime", "PSBeginTime"});

                ps.AddCommand("Select-Object");
                ps.AddArgument(new object[] {"Name", "State", "Command", "PSBeginTime", "PSEndTime", "PSJobTypeName"});

                var results = ps.Invoke();
                return results.Where(obj =>
                        obj.Properties["PSJobTypeName"].Value.ToString() == "PSWorkflowJob"
                    ).Select(obj => new JobData()
                    {
                        Name = obj.Properties["Name"].Value.ToString(),
                        State = obj.Properties["State"].Value.ToString(),
                        Command = obj.Properties["Command"].Value.ToString(),
                        PSBeginTime = (DateTime?) obj.Properties["PSBeginTime"].Value,
                        PSEndTime = (DateTime?) obj.Properties["PSEndTime"].Value,
                    }).ToList();
            }
        }

        public static string GetJobOutput(string jobName)
        {
            using (var ps = createPowerShell())
            {
                // TODO: proper call
                ps.AddScript(string.Format("Receive-Job -Keep {0} 2>&1 3>&1 4>&1", jobName));

                // TODO: special handling is required for WarningRecord.
                // see: workflow W { Write-Warning -Message "hoge" } ; W 3>&1 | Out-String
                ps.AddCommand("Out-String");

                var results = ps.Invoke();
                return string.Join("\n", results.Select(obj => obj.ToString()));
            }
        }

        private static string simpleActionToJob(string command, string jobName)
        {
            using (var ps = createPowerShell())
            {
                ps.AddCommand(command);
                ps.AddArgument(jobName);

                ps.AddCommand("Out-String");

                var results = ps.Invoke();
                return string.Join("\n", results.Select(obj => obj.ToString()));
            }
        }

        public static string StopJob(string jobName)
        {
            return simpleActionToJob("Stop-Job", jobName);
        }

        public static string SuspendJob(string jobName)
        {
            return simpleActionToJob("Suspend-Job", jobName);
        }

        public static string ResumeJob(string jobName)
        {
            return simpleActionToJob("Resume-Job", jobName);
        }

        public static string RemoveJob(string jobName)
        {
            return simpleActionToJob("Remove-Job", jobName);
        }

        public static string StartWorkflow(string scriptPath, string workflowName)
        {
            using (var ps = createPowerShell())
            {
                // TODO: custom name
                Guid jobGuid = Guid.NewGuid();
                string jobName = "Job-" + jobGuid.ToString();

                // TODO: too ad-hoc
                string innerScript = string.Format(". \"{0}\"; {1} -AsJob -JobName {2} | Format-List", scriptPath,
                                                   workflowName, jobName);
                string script = "& { " + innerScript + " } 2>&1 3>&1 4>&1 | Out-String";
                ps.AddScript(script);

                var results = ps.Invoke();
                return string.Join("\n", results.Select(obj => obj.ToString()));
            }
        }
    }
}
