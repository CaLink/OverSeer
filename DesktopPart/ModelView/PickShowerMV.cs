using DesktopPart.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
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
                async (s) =>
                {
                    try
                    {
                        string tempJson = await HttpMessage.MethodGetBut<string>("api/ByteJpeg/" + Data.Pc.id);
                        ByteJpeg byteJpeg = JsonSerializer.Deserialize<ByteJpeg>(tempJson);
                        BMP = Translate(byteJpeg.Jpeg);
                        Data.Bmp = BMP;
                    }
                    catch (Exception)
                    {

                        return;
                    }
                });

            Save = new CustomCUMmand<string>(
                (s) =>
                {
                    Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                    sfd.Filter = "Png files(*.png)|*.png";
                    if (sfd.ShowDialog() != true)
                        return;

                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                    {
                        BitmapEncoder bme = new BmpBitmapEncoder();
                        bme.Frames.Add(BitmapFrame.Create(BMP));
                        bme.Save(fs);
                    }

                });
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

    }
}
