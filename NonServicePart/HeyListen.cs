﻿using NonServicePart.Model;
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


        IPAddress ipAddress;
        int port = 1488;
        TcpListener hey;

        JsonSerializerOptions jso = new JsonSerializerOptions() { WriteIndented = true };


        public HeyListen(string ip = "127.0.0.1")
        {
            if (!EventLog.SourceExists("OverSeerServ"))
            {
                EventLog.CreateEventSource("OverSeerServ", "OverSeerServLog");
            }
            logs.Source = "OverSeerServ";
            logs.Log = "OverSeerServLog";

            logs.WriteEntry($"{DateTime.Now}\nITS ALIVE", EventLogEntryType.Information, logsID++);

            ipAddress = IPAddress.Parse(ip);
            hey = new TcpListener(ipAddress, port);

            Thread listen = new Thread(new ThreadStart(Listen));
            listen.Start();
        }

        public void Listen()
        {
            string mes;
            byte[] send;

            hey.Start();
            logs.WriteEntry($"{DateTime.Now}\nHey, Listen", EventLogEntryType.Information, logsID++);

            //TODO Механизм выхода
            while (true)
            {
                logs.WriteEntry($"{DateTime.Now}\nHey, Listen (Test)", EventLogEntryType.Information, logsID++);

                TcpClient client = hey.AcceptTcpClient();
                NetworkStream ns = client.GetStream();

                logs.WriteEntry($"{DateTime.Now}\nSir, Yes, Sir", EventLogEntryType.Information, logsID++);

                mes = GetMessage(ns);

                send = PrepareMessage(mes);

                logs.WriteEntry($"{DateTime.Now}\n" + send, EventLogEntryType.Information, logsID++);

                ns.Write(send, 0, send.Length);

                logs.WriteEntry($"{DateTime.Now}\nVAINT" , EventLogEntryType.Information, logsID++);

                ns.Close();
                client.Close();

                logs.WriteEntry($"{DateTime.Now}\nSessionEnd", EventLogEntryType.Information, logsID++);
            
            }
        }


        string GetMessage(NetworkStream ns)
        {
            string message = "";

            byte[] data = new byte[256];
            StringBuilder sb = new StringBuilder();
            int bytes = 0;

            do
            {
                bytes = ns.Read(data, 0, data.Length);
                sb.Append(Encoding.UTF8.GetString(data, 0, bytes));
            }
            while (ns.DataAvailable);

            message = sb.ToString();

            logs.WriteEntry($"{DateTime.Now}\n{message}", EventLogEntryType.Information, logsID++);

            return message;
        }

        byte[] PrepareMessage(string type)
        {
            ChosenOne tempChosen = new ChosenOne();
            ProcessInfo tempPI = new ProcessInfo();
            //TODO Bitmap
            //Bitmap bmp;
            string mes = "";
            byte[] preparedMessage = new byte[0];

            switch (type)
            {
                case "GetFullInfo":
                    tempChosen.PcInfo = GetInfo();
                    tempChosen.ProcessInfo = GetProcessInfo();

                    mes = JsonSerializer.Serialize<ChosenOne>(tempChosen, jso);
                    preparedMessage = Encoding.UTF8.GetBytes(mes);

                    logs.WriteEntry($"{DateTime.Now}\n" + tempChosen.PcInfo.GetMessage() + "\n\n" + tempChosen.ProcessInfo.GetMessage(), EventLogEntryType.Information, logsID++);

                    break;

                case "GetProcess":
                    tempPI = GetProcessInfo();

                    mes = JsonSerializer.Serialize<ProcessInfo>(tempPI, jso);
                    preparedMessage = Encoding.UTF8.GetBytes(mes);

                    logs.WriteEntry($"{DateTime.Now}\n" + tempPI.GetMessage());

                    break;

                case "GetJpeg":
                    preparedMessage = GetJpeg();
                    logs.WriteEntry($"{DateTime.Now}\n" + preparedMessage.Length,EventLogEntryType.Information,logsID++);

                    break;

                //TODO Возможно, оно умрет здесь
                default:
                    break;
            }

            return preparedMessage;
        }

        PcInfo GetInfo()
        {
            PcInfo pcInfo = new PcInfo();
            
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

                logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
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

                logs.WriteEntry($"{DateTime.Now}\n" + e.ToString(), EventLogEntryType.Error, logsID++);
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
                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);
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
                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);

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

                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);

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

                logs.WriteEntry($"{DateTime.Now}\nAn error occurred while querying for WMI data: " + e.Message, EventLogEntryType.Error, logsID++);

            }

            logs.WriteEntry($"{DateTime.Now}\n" + pInfo.GetMessage(), EventLogEntryType.Information, logsID++);

            return pInfo;
        }


        //TODO Ну сделать нада
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


            return ret;
        }
    }

    



}