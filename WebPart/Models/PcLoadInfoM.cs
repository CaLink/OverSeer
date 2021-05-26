using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class PcLoadInfoM
    {
        public int PcID { get; set; }
        public int CpuLoad { get; set; }
        public string CpuLoadByCore { get; set; }
        public int RamLoad { get; set; }
    }
}