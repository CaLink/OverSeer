using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.DirectX.AudioVideoPlayback;

namespace DesktopPart.View
{
    /// <summary>
    /// Логика взаимодействия для AboutV.xaml
    /// </summary>
    public partial class AboutV : Window
    {
        Random rnd = new Random();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
     

        public AboutV()
        {
            InitializeComponent();


        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            uno.Foreground = new SolidColorBrush(Color.FromArgb((byte)rnd.Next(200, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255)));
            dos.Foreground = new SolidColorBrush(Color.FromArgb((byte)rnd.Next(200, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255)));
            trees.Foreground = new SolidColorBrush(Color.FromArgb((byte)rnd.Next(200, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255)));
            quadro.Foreground = new SolidColorBrush(Color.FromArgb((byte)rnd.Next(200, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255)));
        }

    }
}
