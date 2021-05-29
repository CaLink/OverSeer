using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.Model
{
    public class PcLoadInfo
    {
        public int CpuLoad { get; set; }
        public List<int> CpuLoadByCore { get; set; }
        public int RamLoad { get; set; }
    }
}
