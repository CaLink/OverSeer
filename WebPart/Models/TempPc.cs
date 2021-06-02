using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class TempPc
    {
        public int id { get; set; }
        public string GUID { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
    }
}