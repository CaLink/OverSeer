using DesktopPart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopPart.View
{
    /// <summary>
    /// Логика взаимодействия для PickShowerV.xaml
    /// </summary>
    public partial class PickShowerV : Window
    {
        System.Drawing.Size size;

        public PickShowerV()
        {
            InitializeComponent();

            size = Screen.PrimaryScreen.Bounds.Size;

            if (Data.Bmp.Height > size.Height)
                this.Height = size.Height;
            else
                this.Height = Data.Bmp.Height;

            if (Data.Bmp.Width > size.Width)
                this.Width = size.Width;
            else
                this.Width = Data.Bmp.Width;

        }
    }
}
