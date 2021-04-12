using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePart
{
    public struct ProcessInfo
    {
        public List<int> CpuLoadByCore { get; set; }
        public int TotalRamLoad { get; set; }
        public List<Proc> ProcessList { get; set; }


        public string GetMessage()
        {
            string message = "";

            ProcessList.ForEach(x => message += x.ID + "  " + x.Name + "\t" + x.Cpu + "\t" + x.Ram + "\n");
            CpuLoadByCore.ForEach(x => message += x + "\t");
            message += "\nTotalRam\t" + TotalRamLoad;
            
            return message;
        }


    }

    public struct Proc
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public string User { get; set; }
        public int Cpu { get; set; }
        public long Ram { get; set; }
        //public string Description { get; set; }

    }
}
