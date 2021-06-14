using DesktopPart.Model;
using DesktopPart.View;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopPart.ModelView
{
    public class OverSeerMV : NotifyModel
    {
        [SuppressUnmanagedCodeSecurity]
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
        static string lastColumn;
        static bool columnChecher;


        ObservableCollection<PcGroupe> pcGroupes;
        public ObservableCollection<PcGroupe> PcGroupes { get { return pcGroupes; } set { pcGroupes = value; RaiseEvent(nameof(pcGroupes)); } } // Очень странно это работает

        PC selectedPC;
        public PC SelectedPC { get { return selectedPC; } set { selectedPC = value as PC; Data.Pc = value as PC; PrepareChart(); RaiseEvent(nameof(SelectedPC)); } }


        private PcLoadInfo pcLoad;
        public PcLoadInfo PcLoad { get { return pcLoad; } set { pcLoad = value; RaiseEvent(nameof(PcLoad)); } }

        private ObservableCollection<Proc> processList;
        public ObservableCollection<Proc> ProcessList { get { return processList; } set { processList = value; RaiseEvent(nameof(ProcessList)); } }


        private BitmapImage jpeg;
        public BitmapImage JPEG { get { return jpeg; } set { jpeg = value; RaiseEvent(nameof(JPEG)); } }

        private string cpuBoss;
        public string CpuBoss { get { return cpuBoss; } set { cpuBoss = value; RaiseEvent(nameof(CpuBoss)); } }

        private string ramBoss;
        public string RamBoss { get { return ramBoss; } set { ramBoss = value; RaiseEvent(nameof(RamBoss)); } }

        public SeriesCollection CpuChart { get; set; }
        public SeriesCollection CpuChartByCore { get; set; }
        public SeriesCollection RamChart { get; set; }

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();


        public CustomCUMmand<string> MenuButton { get; set; }
        public CustomCUMmand<string> GetInfo { get; set; }
        public CustomCUMmand<string> UpdateJPEG { get; set; }
        public CustomCUMmand<string> ShowPick { get; set; }
        public CustomCUMmand<string> UpdateProcessList { get; set; }

        public OverSeerMV()
        {
            timer.Tick += Timer_Tick;
            timer.Interval = 3000;

            CpuChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Cpu%",
                    Values = new ChartValues<double> { 0 }
                }
            };

            RamChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Ram%",
                    Values = new ChartValues<double> { 0 }
                }
            };


            /*
            CpuChartByCore = new SeriesCollection();

            for (int i = 0; i < 5; i++)
            {
                CpuChartByCore.Add(new LineSeries { Title = $"Core{i + 1}", Values = new ChartValues<double>() });
            }
            */

            dbInit();

            MenuButton = new CustomCUMmand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case "Edit":
                            if (Data.PcGroupe.Count == 0)
                            {
                                System.Windows.MessageBox.Show("Please, Update DB");
                                return;
                            }
                            Manager.AddWindowsOpen(new EditV()); PcGroupes = Data.PcGroupe; break;
                        case "UpdateDB": dbInit(); break;
                        case "Log":
                            if (Data.PcGroupe.Count == 0)
                            {
                                System.Windows.MessageBox.Show("Please, Update DB");
                                return;
                            }
                            Manager.AddWindowsOpen(new LogsV()); break;
                        case "Settings": break;
                        case "About": break;

                    }
                });

            GetInfo = new CustomCUMmand<string>(
                (s) =>
                {
                    //МертвыйМетод (на всякий случай)
                });

            UpdateProcessList = new CustomCUMmand<string>(
                async (s) =>
                {
                    string tempJson = await HttpMessage.MethodGetBut<string>("api/ListProc/" + SelectedPC.id);
                    List<Proc> tempProc = JsonSerializer.Deserialize<List<Proc>>(tempJson);
                    ProcessList = new ObservableCollection<Proc>(tempProc);

                },
                () =>
                {
                    if (SelectedPC == null)
                        return false;
                    else
                        return true;
                });

            UpdateJPEG = new CustomCUMmand<string>(
                async (s) =>
                {
                    try
                    {
                        string tempJson = await HttpMessage.MethodGetBut<string>("api/ByteJpeg/" + SelectedPC.id);
                        ByteJpeg byteJpeg = JsonSerializer.Deserialize<ByteJpeg>(tempJson);
                        JPEG = Translate(byteJpeg.Jpeg);
                        Data.Bmp = JPEG;
                    }
                    catch
                    {
                        return;
                    }

                },
                () =>
                {
                    if (SelectedPC == null)
                        return false;
                    else
                        return true;
                });

            ShowPick = new CustomCUMmand<string>(
                (s) =>
                {
                    Data.Pc = SelectedPC;
                    new PickShowerV().ShowDialog();
                    JPEG = Data.Bmp;
                },
                () =>
                {
                    if (SelectedPC == null)
                        return false;
                    else
                        return true;
                });

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (SelectedPC == null)
                return;

            UpdatePc();
            if (PcLoad == null)
                return;
            if (CpuChartByCore.Count > PcLoad.CpuLoadByCore.Count)
                return;
            CpuChart[0].Values[0] = (double)PcLoad.CpuLoad;
            RamChart[0].Values[0] = (double)PcLoad.RamLoad;
            for (int i = 0; i < CpuChartByCore.Count; i++)
            {
                if (CpuChartByCore[i].Values.Count > 10)
                    CpuChartByCore[i].Values.RemoveAt(0);
                CpuChartByCore[i].Values.Add((double)PcLoad.CpuLoadByCore[i]);
            }


        }

        private async void dbInit()
        {

            List<PcGroupe> temp = await HttpMessage.MethodGet<PcGroupe>("api/Pcs");
            if (temp == null)
                return;

            Data.PcGroupe = new ObservableCollection<PcGroupe>(temp);
            PcGroupes = Data.PcGroupe;


            timer.Start();

        }

        private async void UpdatePc() //TODO Добавить как-то в вечный цикл
        {
            PcLoad = await HttpMessage.MethodGetBut<PcLoadInfo>("api/Pcs/" + SelectedPC.id);
        }

        private void PrepareChart()
        {

            CpuChartByCore = Manager.GetOverSeerV().RemakeChart(selectedPC.GeneralInfo.LogicalProcessors);


        }



        private BitmapImage Translate(byte[] jpeg)
        {
            if (jpeg == null)
                return null;

            using (MemoryStream ms = new MemoryStream(jpeg))
            {

                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();



                return bmp;
            }
        }



        public void RaiseSort(string header, IEnumerable itemSource)
        {
            if (lastColumn == header)
                if (columnChecher) columnChecher = false;
                else columnChecher = true;
            else columnChecher = false;
            lastColumn = header;

            var list = (ObservableCollection<Proc>)itemSource;
            var temp = list.ToList();
            temp.Sort((x, y) =>
            {
                if (header == "ID" || header == "Name")
                {
                    var prop = typeof(Proc).GetProperty(header);
                    if (columnChecher) return StrCmpLogicalW(prop.GetValue(x).ToString(), prop.GetValue(y).ToString());
                    else return StrCmpLogicalW(prop.GetValue(y).ToString(), prop.GetValue(x).ToString());
                }
                else
                    switch (header)
                    {
                        case "CPU %":
                            if (columnChecher) return StrCmpLogicalW(x.Cpu.ToString(), y.Cpu.ToString());
                            else return StrCmpLogicalW(y.Cpu.ToString(), x.Cpu.ToString());
                        default:
                            if (columnChecher) return StrCmpLogicalW(x.Ram.ToString(), y.Ram.ToString());
                            else return StrCmpLogicalW(y.Ram.ToString(), x.Ram.ToString());
                    }
            });

            ProcessList = new ObservableCollection<Proc>(temp);

        }
    }
}
