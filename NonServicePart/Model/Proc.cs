using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonServicePart.Model
{
    public struct Proc
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public string User { get; set; }
        public int Cpu { get; set; }
        ulong ram;
        public ulong Ram { get {return ram; } set {ram = (ulong)Math.Round(value/ 1048576.0); } }
        //public string Description { get; set; }

    }
}
