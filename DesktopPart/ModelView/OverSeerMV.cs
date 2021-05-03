using DesktopPart.Model;
using DesktopPart.View;
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
using System.Windows.Media.Imaging;

namespace DesktopPart.ModelView
{
    public class OverSeerMV : NotifyModel
    {
        public ObservableCollection<PC> PCs { get; set; }

        PC selectedPC;
        public PC SelectedPC { get { return selectedPC; } set { selectedPC = value; } }
        public ChosenOne SelectedPCInfo { get; set; }

        private PcInfo pcInfo;
        public PcInfo PcInfo { get { return pcInfo; } set { pcInfo = value; RaiseEvent(nameof(PcInfo)); } }

        private ProcessInfo pInfo;
        public ProcessInfo PInfo { get { return pInfo; } set { pInfo = value; RaiseEvent(nameof(PInfo)); }}

        private BitmapImage jpeg;
        public BitmapImage JPEG { get { return jpeg; } set { jpeg = value; RaiseEvent(nameof(JPEG)); }}






        public CustomCUMmand<string> OpenSMT { get; set; }
        public CustomCUMmand<string> GetInfo { get; set; }

        public OverSeerMV()
        {
            Init();

            OpenSMT = new CustomCUMmand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case "Edit": new EditV().ShowDialog(); break;
                        case "Log": break;
                        case "Settings": break;
                        case "About": break;
                    }
                });

            GetInfo = new CustomCUMmand<string>(
                (s) =>
                {
                    if (selectedPC == null)
                        return;

                    string message = "GetFullInfo";

                    byte[] data = Encoding.UTF8.GetBytes(message);

                    try
                    {
                        TcpClient client = new TcpClient();
                        client.Connect(SelectedPC.IP, 1488);

                        NetworkStream ns = client.GetStream();
                        ns.Write(data, 0, data.Length);


                        data = new byte[256];
                        StringBuilder sb = new StringBuilder();
                        do
                        {
                            int bytes = ns.Read(data, 0, data.Length);
                            sb.Append(Encoding.UTF8.GetString(data, 0, bytes));

                        }
                        while (ns.DataAvailable);

                        ns.Close();
                        client.Close();

                        SelectedPCInfo = JsonSerializer.Deserialize<ChosenOne>(sb.ToString(), new JsonSerializerOptions() { WriteIndented = true });

                        PcInfo = SelectedPCInfo.PcInfo;
                        PInfo = SelectedPCInfo.ProcessInfo;
                        JPEG = Translate(SelectedPCInfo.Bmp);
                    }
                    catch (Exception e)
                    {

                        MessageBox.Show("Lost connection\n" + e.Message);
                    }
                    
                    
                });
        }

        private BitmapImage Translate(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();

                return bmp;
            }
        }

        private void Init()
        {
            using (FileStream fs = new FileStream("PC.List", FileMode.OpenOrCreate, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                Data.PCs = new ObservableCollection<PC>();
                PCs = Data.PCs;
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                    return;

                Data.PCs = JsonSerializer.Deserialize<ObservableCollection<PC>>(json);
                PCs = Data.PCs;
            }






            //PCs = new ObservableCollection<PC_Groupe>()
            //{
            //    new PC_Groupe()
            //    {
            //        Name = "SaS",
            //        PCs = new ObservableCollection<PC>()
            //        {
            //            new PC{Name="qwe",IP="1488"},
            //            new PC{Name="qwa",IP="1488"},
            //        }

            //    },
            //    new PC_Groupe()
            //    {
            //        Name = "SoS",
            //        PCs = new ObservableCollection<PC>()
            //        {
            //            new PC{Name="qwe",IP="1488"},
            //            new PC{Name="qwa",IP="1488"},
            //        }

            //    }
            //}; //Чтение из файла сделать
        }


    }
}
