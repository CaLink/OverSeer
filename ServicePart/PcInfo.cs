﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePart
{
    public class PcInfo
    {
        public string CpuName { get; set; }
        public int Cores { get; set; }
        public int LogicalProcessors { get; set; }
        public string SocketName { get; set; }
        public ulong Ram { get; set; }

        public string SystemName { get; set; }
        public string OSArchitecture { get; set; }
        public string OSVersion { get; set; }

        public List<Disk> Drives { get; set;}


        public PcInfo()
        {
            CpuName = "Woops";
            Cores = -1;
            LogicalProcessors = -1;
            SocketName = "Woops";
            Ram = ulong.MaxValue;
            SystemName = "Woops";
            OSArchitecture = "Woops";
            OSVersion = "Woops";
            Drives = new List<Disk>();
        }


        public string GetMessage()
        {
            string message = "";

            message += $"CpuName:\t\t{CpuName}\n";
            message += $"Cores:\t\t\t{Cores}\n";
            message += $"LogicalProcessors:\t{LogicalProcessors}\n";
            message += $"SocketName:\t\t{SocketName}\n";
            message += $"RamSize:\t\t\t{Ram}\n";
            message += $"----------\n";
            message += $"SystemName:\t\t{SystemName}\n";
            message += $"OSArchitecture:\t\t{OSArchitecture}\n";
            message += $"OSVersion:\t\t{OSVersion}\n";
            message += $"----------\n";


            foreach (var item in Drives)
            {
                message += $"Drive:\t\t\t{item.Drive}\n";
                message += $"DriveType:\t\t{item.DriveType}\n";
                message += $"FileSystem:\t\t{item.FileSystem}\n";
                message += $"AvailabeSpace:\t\t{item.AvailabeSpace}\n";
                message += $"TotalSize:\t\t{item.TotalSize}\n";
                message += $"----------\n";

            }

            return message;
        }

    }


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
