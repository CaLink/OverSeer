using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class Proc
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Cpu { get; set; }
        public ulong Ram { get; set; }

    }
}