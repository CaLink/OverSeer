using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scatman
{
    class Program
    {
        static void Main(string[] args)
        {

            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp as Image);
            g.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            bmp.Save(Path.GetTempPath() + @"\Scatman.png",ImageFormat.Png);

        }
    }
}
