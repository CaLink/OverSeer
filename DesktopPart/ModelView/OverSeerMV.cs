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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopPart.ModelView
{
    public class OverSeerMV : NotifyModel
    {
        public ObservableCollection<PC> PCs { get; set; }
        public ObservableCollection<PcGroupe> PcGroupes { get; set; }

        PC selectedPC;
        public PC SelectedPC { get { return selectedPC; } set { selectedPC = value; Data.Pc = value;     } }
        public ChosenOne SelectedPCInfo { get; set; }

        private PcInfo pcInfo;
        public PcInfo PcInfo { get { return pcInfo; } set { pcInfo = value; RaiseEvent(nameof(PcInfo)); } }

        private ProcessInfo pInfo;
        public ProcessInfo PInfo { get { return pInfo; } set { pInfo = value; RaiseEvent(nameof(PInfo)); }}

        private BitmapImage jpeg;
        public BitmapImage JPEG { get { return jpeg; } set { jpeg = value; RaiseEvent(nameof(JPEG)); }}

        

        public CustomCUMmand<string> OpenSMT { get; set; }
        public CustomCUMmand<string> GetInfo { get; set; }
        public CustomCUMmand<string> UpdateJPEG{ get; set; }
        public CustomCUMmand<string> ShowPick { get; set; }

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
                    if (SelectedPC == null)
                        return;

                    string message = "GetFullInfo";
                    string getMess = "";
                    getMess = DoMessage(message);

                    if (getMess != "")
                    {
                        SelectedPCInfo = JsonSerializer.Deserialize<ChosenOne>(getMess, new JsonSerializerOptions() { WriteIndented = true });
                        PcInfo = SelectedPCInfo.PcInfo;
                        PInfo = SelectedPCInfo.ProcessInfo;
                    }

                    JPEG = DoJpegMessage();

                });

            UpdateJPEG = new CustomCUMmand<string>(
                (s) =>
                {
                    JPEG = DoJpegMessage();
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
                    new PickShowerV().Show();
                },
                ()=> 
                {
                    if (SelectedPC == null)
                        return false;
                    else
                        return true;
                });

        }

        

        private void Init()
        {
            using (FileStream fs = new FileStream("Pc.Groupe", FileMode.OpenOrCreate, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                Data.PcGroupe = new ObservableCollection<PcGroupe>();
                PcGroupes = Data.PcGroupe;
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                    return;

                Data.PcGroupe = JsonSerializer.Deserialize<ObservableCollection<PcGroupe>>(json);
                PcGroupes = Data.PcGroupe;
            }
        }
        
        private void TestInit()
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

        }

        public string DoMessage(string message)
        {
            string retMess = "";
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);

                TcpClient client = new TcpClient();
                client.Connect(SelectedPC.IP, 1488);

                NetworkStream ns = client.GetStream();
                ns.Write(data, 0, data.Length);

                data = new byte[256];
                int bytes = 0;

                StringBuilder sb = new StringBuilder();
                do
                {
                    bytes = ns.Read(data, 0, data.Length);
                    sb.Append(Encoding.UTF8.GetString(data, 0, bytes));

                }
                while (ns.DataAvailable);

                ns.Close();
                client.Close();
                
                retMess = sb.ToString();
            }
            catch (Exception e)
            {

                MessageBox.Show("Lost connection\n" + e.Message);
            }



            return retMess;
        }

        private BitmapImage DoJpegMessage()
        {
            Bitmap jpeg = null;
           
           try
            {
                byte[] data = Encoding.UTF8.GetBytes("GetJpeg");

                byte[] leng = new byte[4];
                int dataLeng = 0;


                TcpClient client = new TcpClient();
                client.Connect(SelectedPC.IP, 1488);

                NetworkStream ns = client.GetStream();
                ns.Write(data, 0, data.Length);

                int bytesRead = ns.Read(leng, 0, 4); // TODO Убрать
                dataLeng = BitConverter.ToInt32(leng,0); // TODO Убрать

                jpeg = new Bitmap(ns);


                ns.Close();
                client.Close();


                BitmapImage returnal = new BitmapImage();

                returnal = Translate(jpeg);
                Data.Bmp = returnal;

                return returnal;



            }
            catch (Exception e )
            {

                MessageBox.Show("Lost connection\n" + e.Message);
                return null;

            }



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
