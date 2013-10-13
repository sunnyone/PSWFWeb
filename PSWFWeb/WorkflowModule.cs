using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Security;

namespace PSWFWeb
{
    class WorkflowModel
    {
        public List<string> WorkflowList { get; set; }
        public string CurrentWorkflowName { get; set; }
        public bool IsCurrentWorkflowStarted { get; set; }
        public string CurrentWorkflowStartResult { get; set; }
    }

    public class WorkflowModule : NancyModule
    {
        public WorkflowModule() : base("/workflow")
        {
            this.RequiresAuthentication();

            Get["/"] = List;
            Get["/detail/{name}"] = Detail;

            Post["/start/{name}"] = Start;
        }

        private dynamic List(dynamic parameters)
        {
            var workflowList = PSWFWebUtil.ListWorkflowScripts();

            var model = new WorkflowModel()
            {
                WorkflowList = workflowList
            };

            return View["Workflow/List"].WithModel(model);
        }

        private void validateWorkflowName(string name)
        {
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            if (name.ToCharArray().Any(c => invalidChars.Contains(c) || c == '.'))
            {
                throw new ArgumentException("name is invalid for a workflow name.");
            }
        }

        private dynamic Detail(dynamic parameters)
        {
            var name = parameters.name;
            validateWorkflowName(name);

            var workflowList = PSWFWebUtil.ListWorkflowScripts();

            var model = new WorkflowModel()
            {
                WorkflowList = workflowList,
                CurrentWorkflowName = name,
            };

            return View["Workflow/Detail"].WithModel(model);
        }

        private dynamic Start(dynamic parameters)
        {
            string name = parameters.name;
            validateWorkflowName(name);

            var workflowList = PSWFWebUtil.ListWorkflowScripts();

            string result;

            string scriptPath = System.IO.Path.Combine(PSWFWebConfig.Current.WorkflowScriptsDirectory, name + ".ps1");
            if (!System.IO.File.Exists(scriptPath))
            {
                throw new ArgumentException(string.Format("The file {0} doesn't exist.", scriptPath));
            }

            string scriptFullPath = System.IO.Path.GetFullPath(scriptPath);

            try
            {
                result = PSWFWebUtil.StartWorkflow(scriptFullPath, name);
            }
            catch (Exception ex)
            {
                result = string.Format("Failed to start workflow '{0}': {1}", name, ex.ToString());
            }

            var model = new WorkflowModel()
            {
                WorkflowList = workflowList,
                CurrentWorkflowName = name,
                IsCurrentWorkflowStarted = true,
                CurrentWorkflowStartResult = result
            };

            return View["Workflow/Detail"].WithModel(model);
        }
    }
}
