using DesktopPart.Model;
using DesktopPart.View;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
        ObservableCollection<PcGroupe> pcGroupes;
        public ObservableCollection<PcGroupe> PcGroupes { get { return pcGroupes; } set { pcGroupes = value; RaiseEvent(nameof(pcGroupes)); } } // Очень странно это работает

        PC selectedPC;
        public PC SelectedPC { get { return selectedPC; } set { selectedPC = value as PC; Data.Pc = value as PC; PrepareChart(); RaiseEvent(nameof(SelectedPC)); } }


        private PcLoadInfo pcLoad;
        public PcLoadInfo PcLoad { get { return pcLoad; } set { pcLoad = value; RaiseEvent(nameof(PcLoad)); } }



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
                        case "About":
                            if (selectedPC == null)
                                return;

                            CpuChartByCore = new SeriesCollection();


                            for (int i = 0; i < selectedPC.GeneralInfo.LogicalProcessors; i++)
                            {
                                LineSeries temp = new LineSeries();
                                temp.Title = "Core " + i + 1;
                                temp.Values = new ChartValues<double>();
                                CpuChartByCore.Add(temp);

                                //CpuChartByCore.Add(new LineSeries() { Title = $"Core{i + 1}", Values = new ChartValues<double>() });

                            }
                            break;
                    }
                });

            GetInfo = new CustomCUMmand<string>(
                (s) =>
                {
                    //МертвыйМетод
                });

            UpdateJPEG = new CustomCUMmand<string>(
                (s) =>
                {
                    //JPEG = DoJpegMessage();
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
                CpuChartByCore[i].Values.Add((double)PcLoad.CpuLoadByCore[i]);
            }


        }

        private async void dbInit()
        {

            List<PcGroupe> temp = await HttpMessage.MethodGet<PcGroupe>("api/Pcs");
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



        private BitmapImage Translate(Bitmap jpeg)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                jpeg.Save(ms, ImageFormat.Png);
                ms.Position = 0;

                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();



                return bmp;
            }
        }


    }
}
