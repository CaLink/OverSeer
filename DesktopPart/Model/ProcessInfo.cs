using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.Model
{
    public struct ProcessInfo
    {
        public int CpuTotal { get; set; }
        public List<int> CpuLoadByCore { get; set; }
        public int TotalRamLoad { get; set; }
        //public int TotalRamLoad { get; set; }
        public List<Proc> ProcessList { get; set; }

    }

    public struct Proc
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public string User { get; set; }
        public int Cpu { get; set; }
        public ulong Ram { get; set; }
        //public string Description { get; set; }

    }
}
