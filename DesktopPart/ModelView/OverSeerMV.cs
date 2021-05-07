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
        public CustomCUMmand<string> UpdateJPEG{ get; set; }

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



                });

            UpdateJPEG = new CustomCUMmand<string>(
                (s) =>
                {
                    JPEG = DoJpegMessage();
                },
                () => 
                {
                    if (selectedPC == null)
                        return false;
                    else
                        return true;
                });

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

        }

        private string DoMessage(string message)
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
           
           //try
            {
                byte[] data = Encoding.UTF8.GetBytes("GetJpeg");

                byte[] leng = new byte[4];
                int dataLeng = 0;


                TcpClient client = new TcpClient();
                client.Connect(SelectedPC.IP, 1488);

                NetworkStream ns = client.GetStream();
                ns.Write(data, 0, data.Length);

                int bytesRead = ns.Read(leng, 0, 4);
                dataLeng = BitConverter.ToInt32(leng,0);

                byte[] byteJpeg = new byte[dataLeng];

                bytesRead = ns.Read(byteJpeg, 0, byteJpeg.Length);
                

                

                ns.Close();
                client.Close();

                using (MemoryStream ms = new MemoryStream(byteJpeg))
                {
                    jpeg = new Bitmap(ms);
                    jpeg.Save(@"D:\Desktop\Ass.png", ImageFormat.Png);
                    // Падает, сука
                    // A generic error occurred in GDI+
                }


                BitmapImage returnal = new BitmapImage();

                returnal = Translate(byteJpeg);

                return returnal;



            }
           // catch (Exception e )
            {

               // MessageBox.Show("Lost connection\n" + e.Message);
                //return null;
                
            }

           

        }


        private BitmapImage Translate(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
               
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();

                return bmp;
            }
        }


    }
}
