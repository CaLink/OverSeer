using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.Model
{
    public class Process
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string User { get; set; }
        public int Cpu { get; set; }
        public int Ram { get; set; }
        public string Description { get; set; } 
    }
}
