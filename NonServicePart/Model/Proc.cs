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
        public long Ram { get; set; }
        //public string Description { get; set; }

    }
}
