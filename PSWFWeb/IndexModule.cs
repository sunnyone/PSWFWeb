using System.Collections.Generic;
using Nancy.Security;

namespace PSWFWeb
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            this.RequiresAuthentication();

            Get["/"] = parameters =>
            {
                return View["Index"];
            };
        }
    }
}