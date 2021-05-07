using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonServicePart.Model
{
    public struct ProcessInfo
    {
        public int CpuTotal { get; set; }
        public List<int> CpuLoadByCore { get; set; }
        public int TotalRamLoad { get; set; }
        public List<Proc> ProcessList { get; set; }


        public string GetMessage()
        {
            string message = "";

            ProcessList.ForEach(x => message += x.ID + "  " + x.Name + "\t" + x.Cpu + "\t" + x.Ram + "\n");
            CpuLoadByCore.ForEach(x => message += x + "\t");
            message += "\nTotalRam\t" + TotalRamLoad;
            message += "\nCpuLoad\t" + CpuTotal;


            return message;
        }


    }
}
