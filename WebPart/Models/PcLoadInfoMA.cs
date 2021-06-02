using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class PcLoadInfoMA
    {
        public int CpuLoad { get; set; }
        public List<int> CpuLoadByCore { get; set; }
        public int RamLoad { get; set; }
    }
}