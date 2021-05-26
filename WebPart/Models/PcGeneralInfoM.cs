using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class PcGeneralInfoM
    {
        //public int PcID { get; set; }
        public string Cpu { get; set; }
        public int Cores { get; set; }
        public int LogicalProcessors { get; set; }
        public string Socket { get; set; }
        public int Ram { get; set; }
        public string SystemName { get; set; }
        public string OsArchitecture { get; set; }
        public string OsVersion { get; set; }


        public static implicit operator PcGeneralInfoM(PcGeneralInfo from)
        {
            return new PcGeneralInfoM
            {
                Cpu = from.Cpu,
                Cores = from.Cores,
                LogicalProcessors = from.LogicalProcessors,
                Socket = from.Socket,
                Ram = from.Ram,
                SystemName = from.SystemName,
                OsArchitecture = from.OsArchitecture,
                OsVersion = from.OsVersion
            };
        }

    }
}