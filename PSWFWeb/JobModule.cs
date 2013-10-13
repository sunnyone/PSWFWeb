using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Security;

namespace PSWFWeb
{
    class JobModel
    {
        public List<JobData> JobList { get; set; }
        public JobData CurrentJob { get; set; }

        public string JobOutput { get; set; }

        public string Output { get; set; }
    }

    public class JobModule : NancyModule
    {
        public JobModule() : base("/job")
        {
            this.RequiresAuthentication();

            Get["/"] = List;
            Get["/detail/{name}"] = Detail;

            Post["/stop/{name}"] = Stop;
            Post["/suspend/{name}"] = Suspend;
            Post["/resume/{name}"] = Resume;
            Post["/remove/{name}"] = Remove;
        }

        private dynamic List(dynamic parameters)
        {
            var jobList = PSWFWebUtil.GetJobs();
            var model = new JobModel()
            {
                JobList = jobList
            };
            return View["List"].WithModel(model);
        }

        private JobModel createModelForJob(string jobName, string output)
        {
            var jobList = PSWFWebUtil.GetJobs();

            var currentJob = jobList.First(x => x.Name == jobName);

            var model = new JobModel()
            {
                JobList = jobList,
                CurrentJob = currentJob,
                Output = output
            };

            return model;
        }

        private dynamic Detail(dynamic parameters)
        {
            var jobName = parameters.name;

            string output = PSWFWebUtil.GetJobOutput(jobName);
            JobModel model = createModelForJob(jobName, output);
            return View["Detail"].WithModel(model);
        }

        private dynamic Stop(dynamic parameters)
        {
            var jobName = parameters.name;

            // do action at first
            string output = PSWFWebUtil.StopJob(jobName);
 
            JobModel model = createModelForJob(jobName, output);
            return View["Detail"].WithModel(model);
        }

        private dynamic Suspend(dynamic parameters)
        {
            var jobName = parameters.name;

            // do action at first
            string output = PSWFWebUtil.SuspendJob(jobName);

            JobModel model = createModelForJob(jobName, output);
            return View["Detail"].WithModel(model);
        }

        private dynamic Resume(dynamic parameters)
        {
            var jobName = parameters.name;

            // do action at first
            string output = PSWFWebUtil.ResumeJob(jobName);

            JobModel model = createModelForJob(jobName, output);
            return View["Detail"].WithModel(model);
        }

        private dynamic Remove(dynamic parameters)
        {
            var jobName = parameters.name;

            // do action at first
            PSWFWebUtil.RemoveJob(jobName);

            return Response.AsRedirect("/job");
        }
    }
}
