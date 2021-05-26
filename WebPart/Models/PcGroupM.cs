using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class PcGroupM
    {
        public int id { get; set; }
        public string Name { get; set; }
        public List<PcM> PcMs { get; set; }

        public PcGroupM()
        {
            PcMs = new List<PcM>();
        }

        public static implicit operator PcGroupM(PcGroup from)
        {
            return new PcGroupM
            {
                id = from.id,
                Name = from.Name,

            };
        }

        
    }
}