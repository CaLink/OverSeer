using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonServicePart.Model
{
    [Serializable]
    class PcLoadInfo
    {
        int cpuLoad;
        public int CpuLoad { get=>cpuLoad; set { cpuLoad = value; } }
        List<int> cpuLoadByCore;
        public List<int> CpuLoadByCore { get => cpuLoadByCore; set { cpuLoadByCore = value; } }
        int ramLoad;
        public int RamLoad { get=>ramLoad; set { ramLoad = value; } }

        public PcLoadInfo()
        {
            CpuLoadByCore = new List<int>();
        }
    }
}
