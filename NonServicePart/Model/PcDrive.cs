using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonServicePart.Model
{
    [Serializable]
    class PcDrive
    {
        string drive;
        public string Drive { get=>drive; set { drive = value; } }
        string driveType;
        public string DriveType { get=> driveType; set { driveType = value; } }
        string fileSystem;
        public string FileSystem { get=>fileSystem; set { fileSystem = value; } }
        long availabeSpace;
        public long AvailabeSpace { get { return availabeSpace; } set { availabeSpace = (long)Math.Round(value / 1073741824.0); } }
        long totalSize;
        public long TotalSize { get { return totalSize; } set { totalSize = (long)Math.Round(value / 1073741824.0); } }

        public PcDrive()
        {
            Drive = "Woops";
            DriveType = "Woops";
            FileSystem = "Woops";
            AvailabeSpace = 0;
            TotalSize = 0;
        }

    }
}
