using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ServicePart
{
    public partial class OverSeerService : ServiceBase
    {
        int eventID = 1;

        PcInfo pcInfo;
        ProcessInfo pInfo;
        Bitmap bmp;
        //BitmapImage jpeg;

        IPAddress localhost;
        int port;
        TcpListener Hey;



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
            eventLog1.WriteEntry($"{DateTime.Now}\nITS ALIVE", EventLogEntryType.Information, eventID++); // Стартуем
            
            //Todo Отдельный поток

            localhost = IPAddress.Parse("127.0.0.1");
            port = 1488;
            Hey = new TcpListener(localhost, port);

            Hey.Start();

            while (true)
            {
                eventLog1.WriteEntry("Начинаем слушать", EventLogEntryType.Information, eventID++);
                TcpClient client = Hey.AcceptTcpClient();
                NetworkStream ns = client.GetStream();
                eventLog1.WriteEntry("Услышали", EventLogEntryType.Information, eventID++);

                byte[] data = new byte[256];
                StringBuilder sb = new StringBuilder();
                int bytes = 0;

                do
                {
                    bytes = ns.Read(data, 0, data.Length);
                    sb.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (ns.DataAvailable);

                string message = sb.ToString();

                eventLog1.WriteEntry(message, EventLogEntryType.Information, eventID++);

                string send = "";
                JsonSerializerOptions jso = new JsonSerializerOptions() { WriteIndented = true };


                switch (message)
                {
                    case "GetFullInfo":
                        GetInfo();
                        GetProccessInfo();
                        eventLog1.WriteEntry("Жив", EventLogEntryType.Information, eventID++);
                        
                        //Todo
                        try
                        {
                            GetScreen();

                        }
                        catch (Exception e)
                        {

                            eventLog1.WriteEntry(e.ToString(), EventLogEntryType.Information, eventID++);
                        }

                        eventLog1.WriteEntry("оЖил", EventLogEntryType.Information, eventID++);
                        ChosenOne tempChosen = new ChosenOne() { PcInfo = pcInfo, ProcessInfo = pInfo, Bmp = bmp};
                        eventLog1.WriteEntry("Жив", EventLogEntryType.Information, eventID++);
                        send = JsonSerializer.Serialize<ChosenOne>(tempChosen, jso);
                        eventLog1.WriteEntry("СМЭРД", EventLogEntryType.Information, eventID++);
                        break;
                    case "GetProcess":
                        GetProccessInfo();
                        send = JsonSerializer.Serialize<ProcessInfo>(pInfo, jso);
                        break;
                    case "GetJpeg":
                        GetScreen();
                        send = JsonSerializer.Serialize<Bitmap>(bmp, jso);
                        break;

                    default:
                        return;

                }

                eventLog1.WriteEntry(send, EventLogEntryType.Information, eventID++);

                data = Encoding.UTF8.GetBytes(send);
                ns.Write(data, 0, data.Length);
                eventLog1.WriteEntry("Полетели", EventLogEntryType.Information, eventID++);


                /*if (message == "GetFullInfo")
                {
                    GetInfo();
                    GetProccessInfo();

                    ChosenOne tempChosen = new ChosenOne() { PcInfo = pcInfo, ProcessInfo = pInfo };

                    string send = JsonSerializer.Serialize<ChosenOne>(tempChosen,new JsonSerializerOptions() { WriteIndented = true});

                    eventLog1.WriteEntry(send, EventLogEntryType.Information, eventID++);

                    data = Encoding.UTF8.GetBytes(send);
                    ns.Write(data, 0, data.Length);
                    eventLog1.WriteEntry("Полетели", EventLogEntryType.Information, eventID++);

                }*/

                ns.Close();
                client.Close();

                eventLog1.WriteEntry("Конец связи", EventLogEntryType.Information, eventID++);


            }








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
            eventLog1.WriteEntry($"{DateTime.Now}\nMurder", EventLogEntryType.Information, eventID++); // !Стартуем
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

            // ProcessInfo
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
                eventLog1.WriteEntry("An error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, eventID++);
            }

            // COREs%
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

            // %RAM%
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");

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

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");


                foreach (ManagementObject item in searcher.Get())
                {
                    int percentCpu = int.Parse(item["LoadPercentage"].ToString());
                    pInfo.CpuTotal = percentCpu;
                }

            }
            catch (ManagementException e)
            {

                eventLog1.WriteEntry("An error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, eventID++);

            }

            eventLog1.WriteEntry(pInfo.GetMessage(), EventLogEntryType.Information, eventID++);

        }

        void GetScreen()
        {
            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp as Image);
            g.CopyFromScreen(0, 0, 0, 0, bmp.Size); //тут падает?

            //jpeg = Translate(bmp);

        }

        //BitmapImage Translate(Bitmap bitmap)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bitmap.Save(ms, ImageFormat.Bmp);
        //        ms.Position = 0;
        //        BitmapImage bmp = new BitmapImage();
        //        bmp.BeginInit();
        //        bmp.StreamSource = ms;
        //        bmp.CacheOption = BitmapCacheOption.OnLoad;
        //        bmp.EndInit();

        //        return bmp;
        //    }
        //}

    }


    public class ChosenOne
    {
        public PcInfo PcInfo { get; set; }
        public ProcessInfo ProcessInfo { get; set; }
        public Bitmap Bmp { get; set; }
    }
}
