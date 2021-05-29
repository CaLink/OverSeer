using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.Model
{
    public class PcDrive
    {
        public string Drive { get; set; }
        public string DriveType { get; set; }
        public string FileSystem { get; set; }
        public int AvailabeSpace { get; set; }
        public int TotalSize { get; set; }
    }
}
