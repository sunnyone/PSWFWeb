using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSWFWeb
{
    public class JobData
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Command { get; set; }
        public DateTime? PSBeginTime { get; set; }
        public DateTime? PSEndTime { get; set; }
    }
}
