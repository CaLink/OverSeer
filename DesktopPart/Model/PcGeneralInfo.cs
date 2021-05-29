using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.Model
{
    public class PcGeneralInfo
    {
        public string Cpu { get; set; }
        public int Cores { get; set; }
        public int LogicalProcessors { get; set; }
        public string Socket { get; set; }
        public int Ram { get; set; }
        public string SystemName { get; set; }
        public string OsArchitecture { get; set; }
        public string OsVersion { get; set; }
    }
}
