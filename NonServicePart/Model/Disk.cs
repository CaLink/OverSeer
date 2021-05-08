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
        long availabeSpace;
        public long AvailabeSpace { get {return availabeSpace; } set {availabeSpace = (long)Math.Round(value / 1073741824.0); } }
        long totalSize;
        public long TotalSize { get {return totalSize; } set {totalSize = (long)Math.Round(value/ 1073741824.0); } }


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
