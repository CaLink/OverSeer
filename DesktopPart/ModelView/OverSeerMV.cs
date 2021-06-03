using DesktopPart.Model;
using DesktopPart.View;
using LiveCharts;
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
        public ObservableCollection<PcGroupe> PcGroupes { get { return pcGroupes; } set {pcGroupes =value;RaiseEvent(nameof(pcGroupes)); } } // Очень странно это работает

        PC selectedPC;
        public PC SelectedPC { get { return selectedPC; } set { selectedPC = value as PC; Data.Pc = value as PC; RaiseEvent(nameof(SelectedPC)); } }
        
        private PcLoadInfo pcLoad;
        public PcLoadInfo PcLoad { get { return pcLoad; } set { pcLoad = value; RaiseEvent(nameof(PcLoad)); } }



        private BitmapImage jpeg;
        public BitmapImage JPEG { get { return jpeg; } set { jpeg = value; RaiseEvent(nameof(JPEG)); } }

        private string cpuBoss;
        public string CpuBoss { get { return cpuBoss; } set { cpuBoss = value; RaiseEvent(nameof(CpuBoss)); } }

        private string ramBoss;
        public string RamBoss { get { return ramBoss; } set { ramBoss = value; RaiseEvent(nameof(RamBoss)); } }

        private SeriesCollection cpuChart;

        public SeriesCollection CpuChart { get { return cpuChart; } set { cpuChart = value; } }

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();


        public CustomCUMmand<string> MenuButton { get; set; }
        public CustomCUMmand<string> GetInfo { get; set; }
        public CustomCUMmand<string> UpdateJPEG { get; set; }
        public CustomCUMmand<string> ShowPick { get; set; }

        public OverSeerMV()
        {
            timer.Tick += Timer_Tick;
            timer.Interval = 3000;
            dbInit();

            MenuButton = new CustomCUMmand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case "Edit": Manager.AddWindowsOpen(new EditV()); PcGroupes = Data.PcGroupe; break;
                        case "UpdateDB":dbInit();break;
                        case "Log": Manager.AddWindowsOpen(new LogsV()); break;
                        case "Settings": break;
                        case "About": break;
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
            if (SelectedPC != null)
                UpdatePc();
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
