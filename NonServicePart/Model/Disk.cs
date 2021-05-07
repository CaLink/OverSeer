using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonServicePart.Model
{
    public class Disk
    {
        public string Drive { get; set; }
        public string DriveType { get; set; }
        public string FileSystem { get; set; }
        public long AvailabeSpace { get; set; }
        public long TotalSize { get; set; }


        public Disk()
        {
            Drive = "Woops";
            DriveType = "Woops";
            FileSystem = "Woops";
            AvailabeSpace = -1;
            TotalSize = -1;

        }
    }
}
