using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ServicePart
{
    public partial class OverSeerService : ServiceBase
    {
        int eventID = 1;

        PcInfo pcInfo;
        ProcessInfo pInfo;
        Bitmap bmp;
        
        public OverSeerService()
        {
            InitializeComponent();

            //Работа с эвентами
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists("OverSeerServ"))
            {
                EventLog.CreateEventSource("OverSeerServ", "OverSeerServLog");
            }
            eventLog1.Source = "OverSeerServ";
            eventLog1.Log = "OverSeerServLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry($"{DateTime.Now}\nITS ALIVE"); // Стартуем

            GetInfo();
            GetProccessInfo();

            //TimeToStart(null, null);

            /*
            timer =  new Timer(); //Настройка таймера
            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(this.TimeToStart);
            timer.Start();
            */
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry($"{DateTime.Now}\nMurder"); // !Стартуем
        }

        private void TimeToStart(object sender, ElapsedEventArgs e)
        {
            /*
            string mail = $"{DateTime.Now}\n";
            process = Process.GetProcesses().ToList();

            process.ForEach(x => mail += $"{x.Id}, {x.ProcessName}\n");
            mail += "-------\n";



            eventLog1.WriteEntry(mail, EventLogEntryType.Information, eventID++);
            */
        }

        void GetInfo()
        {
            pcInfo = new PcInfo();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    pcInfo.CpuName = queryObj["Name"].ToString();
                    pcInfo.Cores = int.Parse(queryObj["NumberOfCores"].ToString());
                    pcInfo.LogicalProcessors = int.Parse(queryObj["NumberOfLogicalProcessors"].ToString());
                    pcInfo.SocketName = queryObj["SocketDesignation"].ToString();
                    pcInfo.SystemName = queryObj["SystemName"].ToString();

                }
            }
            catch (Exception e)
            {

                eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Error, eventID++);
            }

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    pcInfo.OSArchitecture = queryObj["OSArchitecture"].ToString();
                    pcInfo.OSVersion = queryObj["Caption"].ToString();
                    pcInfo.Ram = ulong.Parse(queryObj["TotalVisibleMemorySize"].ToString());

                }
            }
            catch (Exception e)
            {

                eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Error, eventID++);
            }

            try
            {

                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    Disk disk = new Disk();

                    disk.Drive = d.Name;
                    disk.DriveType = d.DriveType.ToString();
                    if (d.IsReady == true)
                    {
                        disk.FileSystem = d.DriveFormat;
                        disk.AvailabeSpace = d.TotalFreeSpace;
                        disk.TotalSize = d.TotalSize;
                    }
                    pcInfo.Drives.Add(disk);
                }


            }
            catch (Exception e)
            {

                eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Error, eventID++);
            }

            eventLog1.WriteEntry(pcInfo.GetMessage(), EventLogEntryType.Information, eventID++);

        }


        void GetProccessInfo()
        {
            pInfo = new ProcessInfo();
            pInfo.ProcessList = new List<Proc>();
            pInfo.CpuLoadByCore = new List<int>();

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_PerfProc_Process");
                Proc newProc = new Proc();

                foreach (ManagementObject queryObj in searcher.Get())
                {

                    newProc.ID = int.Parse(queryObj["IDProcess"].ToString());
                    newProc.Name = queryObj["Name"].ToString();
                    newProc.Cpu = int.Parse(queryObj["PercentProcessorTime"].ToString());
                    newProc.Ram = long.Parse(queryObj["WorkingSet"].ToString());

                    pInfo.ProcessList.Add(newProc);
                }
                

            }
            catch (ManagementException e)
            {
                eventLog1.WriteEntry("An error occurred while querying for WMI data: " + e.Message,EventLogEntryType.Error,eventID++);
            }


            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    pInfo.CpuLoadByCore.Add(int.Parse(queryObj["PercentProcessorTime"].ToString()));
                }
            }
            catch (ManagementException e)
            {
                eventLog1.WriteEntry("An error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, eventID++);

            }


            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_OperatingSystem");

                foreach (ManagementObject item in searcher.Get())
                {
                    long free = long.Parse(item["FreePhysicalMemory"].ToString());
                    long total = long.Parse(item["TotalVisibleMemorySize"].ToString());
                    decimal temp = ((decimal)total - (decimal)free) / (decimal)total;
                    pInfo.TotalRamLoad = (int)(Math.Round(temp, 2) * 100);
                }


            }
            catch (ManagementException e)
            {

                eventLog1.WriteEntry("An error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, eventID++);

            }

            eventLog1.WriteEntry(pInfo.GetMessage(),EventLogEntryType.Information,eventID++);

        }

        void GetScreen()
        {

            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp as Image);
            g.CopyFromScreen(0, 0, 0, 0, bmp.Size);

        }

    }
}
