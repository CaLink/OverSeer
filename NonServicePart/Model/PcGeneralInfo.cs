using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NonServicePart.Model
{
    [Serializable]
    class PcGeneralInfo
    {
        string cpu;
        public string Cpu { get => cpu; set { cpu = value; } }
        int cores;
        public int Cores { get=> cores; set { cores = value; } }
        int logicalProcessors;
        public int LogicalProcessors { get=> logicalProcessors; set { logicalProcessors = value; } }
        string socket;
        public string Socket { get => socket; set { socket = value; } }
        ulong ram;
        public ulong Ram { get { return ram; } set { ram = (ulong)Math.Round(value / 1048576.0); }}
        string systemName;
        public string SystemName { get=> systemName; set { systemName = value; } }
        string osArchitecture;
        public string OsArchitecture { get => osArchitecture; set { osArchitecture = value; } }
        string osVersion;
        public string OsVersion { get=> osVersion; set {osVersion = value; } }
    }
}
