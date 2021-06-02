using NonServicePart.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NonServicePart
{
    public class HeyListen
    {
        int logsID = 1;
        EventLog logs = new EventLog();

        Pc pc = new Pc();

        System.Windows.Forms.Timer time = new System.Windows.Forms.Timer();

        ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name,NumberOfCores,NumberOfLogicalProcessors,SocketDesignation,SystemName FROM Win32_Processor");
        ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("root\\CIMV2", "SELECT OSArchitecture,Caption,TotalVisibleMemorySize FROM Win32_OperatingSystem");
        ManagementObjectSearcher searcher3 = new ManagementObjectSearcher("root\\CIMV2", "SELECT IDProcess,Name,PercentProcessorTime,WorkingSetPrivate FROM Win32_PerfFormattedData_PerfProc_Process");
        ManagementObjectSearcher searcher4 = new ManagementObjectSearcher("root\\CIMV2", "SELECT PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor");
        ManagementObjectSearcher searcher5 = new ManagementObjectSearcher("root\\CIMV2", "SELECT FreePhysicalMemory,TotalVisibleMemorySize FROM Win32_OperatingSystem");
        ManagementObjectSearcher searcher6 = new ManagementObjectSearcher("root\\CIMV2", "SELECT LoadPercentage FROM Win32_Processor");


        public HeyListen()
        {
            if (!EventLog.SourceExists("OverSeerServ"))
            {
                EventLog.CreateEventSource("OverSeerServ", "OverSeerServLog");
            }
            logs.Source = "OverSeerServ";
            logs.Log = "OverSeerServLog";

            logs.WriteEntry($"{DateTime.Now}\nITS ALIVE", EventLogEntryType.Information, logsID++);

            time.Tick += Time_Tick;
            time.Interval = 5000;

            //Прогон одного запроса
            try
            {
                searcher3.Get();
            }
            catch (Exception e)
            {
                logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
            }

            logs.WriteEntry($"{DateTime.Now}\nHey, Listen", EventLogEntryType.Information, logsID++);

            dbInit();

            time.Start();

        }

        private void Time_Tick(object sender, EventArgs e)
        {
            Listen();
        }

        private void dbInit()
        {
            pc = InitPC().Result; // Тут нужно создавать комп? хз пока
            SendGeneralInfo(); // Тут отправляем всю основную инфу о компе

            SendDrive(); // Тут все винты
            // Хз, возможно лучше объединить это все в один класс и отправлять его JSON'ом
            // А не плодить сразу три запроса на сервак

        }

        private async Task<Pc> InitPC()
        {

            if (File.Exists("Secret.Data"))
                using (FileStream fs = new FileStream("Secret.Data", FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(fs))
                {
                    pc.id = int.Parse(sr.ReadLine());
                    pc.GUID = sr.ReadLine();
                }
            
            Pc temp = HttpMessage.MethodPost("api/Pcs", pc).Result;

            using (FileStream fs = new FileStream("Secret.Data", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(temp.id);
                sw.WriteLine(temp.GUID);
            }

            return temp;

        }

        private void SendGeneralInfo()
        {
            PcGeneralInfo pgi = GetGeneralInfo();
            HttpMessage.MethodPut("api/PcGeneralInfoes/" + pc.id, pgi);
        }

        private void SendDrive()
        {
            List<PcDrive> lpd = GetDrive();
            HttpMessage.MethodPut("api/PcDrives/" + pc.id, lpd);
        }

        private void SendLoad()
        {
            PcLoadInfo pci = GetLoadInfo();
            HttpMessage.MethodPut("api/PcLoadInfoes/" + pc.id, pci);
        }





        public void Listen()
        {

            SendLoad();
                // Тут нунжо сделать постоянную заливку в БД
                // Заливаем загрузку компа  (PcLoadInfo ) и, если необходимо, логи (Logs)


        }


        PcGeneralInfo GetGeneralInfo()
        {
            PcGeneralInfo ret = new PcGeneralInfo();

            try
            {
                foreach (ManagementObject queryObj in searcher1.Get())
                {
                    ret.Cpu = queryObj["Name"].ToString();
                    ret.Cores = int.Parse(queryObj["NumberOfCores"].ToString());
                    ret.LogicalProcessors = int.Parse(queryObj["NumberOfLogicalProcessors"].ToString());
                    ret.Socket = queryObj["SocketDesignation"].ToString();
                    ret.SystemName = queryObj["SystemName"].ToString();
                }
            }
            catch (Exception e)
            {
                //logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
                //Logs();
            }

            try
            {
                foreach (ManagementObject queryObj in searcher2.Get())
                {
                    ret.OsArchitecture = queryObj["OSArchitecture"].ToString();
                    ret.OsVersion = queryObj["Caption"].ToString();
                    ret.Ram = ulong.Parse(queryObj["TotalVisibleMemorySize"].ToString());

                }
            }
            catch (Exception e)
            {
                //logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
                //Logs();
            }

            return ret;
        }

        List<PcDrive> GetDrive()
        {
            List<PcDrive> ret = new List<PcDrive>();

            try
            {

                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    PcDrive disk = new PcDrive();

                    disk.Drive = d.Name;
                    disk.DriveType = d.DriveType.ToString();
                    if (d.IsReady == true)
                    {
                        disk.FileSystem = d.DriveFormat;
                        disk.AvailabeSpace = d.TotalFreeSpace;
                        disk.TotalSize = d.TotalSize;
                    }
                    ret.Add(disk);
                }

            }
            catch (Exception e)
            {

                //logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
                //Logs();
            }

            return ret;

        }

        PcLoadInfo GetLoadInfo()
        {
            PcLoadInfo ret = new PcLoadInfo();

            //CpuByCores
            try
            {
                foreach (ManagementObject queryObj in searcher4.Get())
                {
                    ret.CpuLoadByCore.Add(int.Parse(queryObj["PercentProcessorTime"].ToString()));
                }
            }
            catch (ManagementException e)
            {
                //logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);
                //Logs();

            }

            // CPU%
            try
            {
                foreach (ManagementObject item in searcher6.Get())
                {
                    int percentCpu = int.Parse(item["LoadPercentage"].ToString());
                    ret.CpuLoad = percentCpu;
                }

            }
            catch (Exception e)
            {
                //logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);
                //Logs();

            }

            // %RAM%
            try
            {
                foreach (ManagementObject item in searcher5.Get())
                {
                    long free = long.Parse(item["FreePhysicalMemory"].ToString());
                    long total = long.Parse(item["TotalVisibleMemorySize"].ToString());
                    decimal temp = ((decimal)total - (decimal)free) / (decimal)total;
                    ret.RamLoad = (int)(Math.Round(temp, 2) * 100);
                }

            }
            catch (ManagementException e)
            {
                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);
                //Logs();

            }

            return ret;
        }




        PcInfo GetInfo()
        {
            PcInfo pcInfo = new PcInfo();

            // PcInfo
            try
            {
                foreach (ManagementObject queryObj in searcher1.Get())
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

                logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
            }
            // PcInfo
            try
            {


                foreach (ManagementObject queryObj in searcher2.Get())
                {
                    pcInfo.OSArchitecture = queryObj["OSArchitecture"].ToString();
                    pcInfo.OSVersion = queryObj["Caption"].ToString();
                    pcInfo.Ram = ulong.Parse(queryObj["TotalVisibleMemorySize"].ToString());

                }
            }
            catch (Exception e)
            {

                logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
            }
            // DiskInfo
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

                logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
            }

            logs.WriteEntry($"{DateTime.Now}\n" + pcInfo.GetMessage(), EventLogEntryType.Information, logsID++);

            return pcInfo;
        }
        ProcessInfo GetProcessInfo()
        {
            ProcessInfo pInfo = new ProcessInfo();

            pInfo.ProcessList = new List<Proc>();
            pInfo.CpuLoadByCore = new List<int>();

            // ProcessInfo
            try
            {


                Proc newProc = new Proc();

                foreach (ManagementObject queryObj in searcher3.Get())
                {

                    newProc.ID = int.Parse(queryObj["IDProcess"].ToString());
                    newProc.Name = queryObj["Name"].ToString();
                    newProc.Cpu = int.Parse(queryObj["PercentProcessorTime"].ToString());
                    newProc.Ram = ulong.Parse(queryObj["WorkingSetPrivate"].ToString()); //Or WorkingSet

                    if (newProc.Name == "Idle" || newProc.Name == "_Total")
                        continue;

                    pInfo.ProcessList.Add(newProc);
                }


            }
            catch (ManagementException e)
            {
                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);
            }

            // COREs%
            try
            {


                foreach (ManagementObject queryObj in searcher4.Get())
                {
                    pInfo.CpuLoadByCore.Add(int.Parse(queryObj["PercentProcessorTime"].ToString()));
                }
            }
            catch (ManagementException e)
            {
                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);

            }

            // %RAM%
            try
            {


                foreach (ManagementObject item in searcher5.Get())
                {
                    long free = long.Parse(item["FreePhysicalMemory"].ToString());
                    long total = long.Parse(item["TotalVisibleMemorySize"].ToString());
                    decimal temp = ((decimal)total - (decimal)free) / (decimal)total;
                    pInfo.TotalRamLoad = (int)(Math.Round(temp, 2) * 100);
                }


            }
            catch (ManagementException e)
            {

                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);

            }

            // CPU%
            try
            {



                foreach (ManagementObject item in searcher6.Get())
                {
                    int percentCpu = int.Parse(item["LoadPercentage"].ToString());
                    pInfo.CpuTotal = percentCpu;
                }

            }
            catch (Exception e)
            {

                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);

            }

            logs.WriteEntry($"{DateTime.Now}\n" + pInfo.GetMessage(), EventLogEntryType.Information, logsID++);

            return pInfo;
        }


        byte[] GetJpeg()
        {
            byte[] ret;
            byte[] length;
            byte[] mes;
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp as Image);
            g.CopyFromScreen(0, 0, 0, 0, bmp.Size);

            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                mes = ms.ToArray();
            }

            length = BitConverter.GetBytes(mes.Length);
            ret = new byte[4 + mes.Length];
            length.CopyTo(ret, 0);
            mes.CopyTo(ret, 4);

            int test = 0;
            foreach (var item in mes)
            {
                test += item;
            }

            return ret;
        }
    }





}
