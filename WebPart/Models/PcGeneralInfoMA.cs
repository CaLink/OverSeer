using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class PcGeneralInfoMA
    {
            public string Cpu { get; set; }
            public int Cores { get; set; }
            public int LogicalProcessors { get; set; }
            public string Socket { get; set; }
            public ulong Ram { get; set ;  }
            public string SystemName { get; set; }
            public string OsArchitecture { get; set; }
            public string OsVersion { get; set; }
        
    }
}