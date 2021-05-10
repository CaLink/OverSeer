using DesktopPart.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DesktopPart.ModelView
{
    class PickShowerMV : NotifyModel
    {
        private BitmapImage bmp;

        public BitmapImage BMP { get { return bmp; } set { bmp = value; RaiseEvent(nameof(BMP)); } }
        public CustomCUMmand<string> Refresh { get; set; }
        public CustomCUMmand<string> Save { get; set; }

        public PickShowerMV()
        {
            BMP = Data.Bmp;

            Refresh = new CustomCUMmand<string>(
                (s) =>
                {
                    BMP = DoJpegMessage();
                    Data.Bmp = BMP;
                });

            Save = new CustomCUMmand<string>(
                (s) =>
                {
                    Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                    sfd.Filter = "Png files(*.png)|*.png";
                    if (sfd.ShowDialog() != true)
                        return;

                    using (FileStream fs = new FileStream(sfd.FileName,FileMode.Create, FileAccess.Write))
                    {
                        BitmapEncoder bme = new BmpBitmapEncoder();
                        bme.Frames.Add(BitmapFrame.Create(BMP));
                        bme.Save(fs);
                    }

                });
        }


        //TODO Дублирование говноКода
        private BitmapImage DoJpegMessage()
        {
            Bitmap jpeg = null;

            try
            {
                byte[] data = Encoding.UTF8.GetBytes("GetJpeg");

                byte[] leng = new byte[4];
                int dataLeng = 0;


                TcpClient client = new TcpClient();
                client.Connect(Data.Pc.IP, 1488);

                NetworkStream ns = client.GetStream();
                ns.Write(data, 0, data.Length);

                int bytesRead = ns.Read(leng, 0, 4); // TODO Убрать
                dataLeng = BitConverter.ToInt32(leng, 0); // TODO Убрать

                jpeg = new Bitmap(ns);


                ns.Close();
                client.Close();


                BitmapImage returnal = new BitmapImage();

                returnal = Translate(jpeg);
                Data.Bmp = returnal;

                return returnal;



            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show("Lost connection\n" + e.Message);
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
