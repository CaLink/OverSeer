using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class PcDriveM
    {
        //public int ID { get; set; }
        //public int PcId { get; set; }
        public string Drive { get; set; }
        public string  DriveType { get; set; }
        public string FileSystem { get; set; }
        public int AvailabeSpace { get; set; }
        public int TotalSize { get; set; }


        public static implicit operator PcDriveM(PcDrive from)
        {
            return new PcDriveM
            {
                Drive = from.Drive,
                FileSystem = from.FileSystem,
                AvailabeSpace = from.AvailabeSpace,
                TotalSize = from.TotalSize
            };
        }
    }
}