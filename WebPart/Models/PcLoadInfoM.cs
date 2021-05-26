using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class PcLoadInfoM
    {
        public int CpuLoad { get; set; }
        public List<int> CpuLoadByCore { get; set; }
        public int RamLoad { get; set; }


        public static implicit operator PcLoadInfoM(PcLoadInfo from)
        {
            return new PcLoadInfoM
            {
                CpuLoad = from.CpuLoad,
                CpuLoadByCore = CpuLoadConvertor(from.CpuLoadByCore),
                RamLoad = from.RamLoad
            };
        }


        static List<int> CpuLoadConvertor(string txt)
        {
            List<int> temp = new List<int>();
            txt.Split('/').ToList().ForEach(x=>temp.Add(int.Parse(x)));

            return temp;
        }

    }
}