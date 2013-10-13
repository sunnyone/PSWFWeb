using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PSWFWeb
{
    public class PSWFWebConfig
    {
        public string Uri { get; set; }

        public string WorkflowScriptsDirectory { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // just a holder
        public static PSWFWebConfig Current { get; set; }
    }
}
