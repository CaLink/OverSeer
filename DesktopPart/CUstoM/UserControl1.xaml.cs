using LiveCharts;
using LiveCharts.Wpf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopPart.CUstoM
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {


        public SeriesCollection SeriesCollection
        {
            get { return (SeriesCollection)GetValue(SeriesCollectionProperty); }
            set { SetValue(SeriesCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SeriesCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeriesCollectionProperty =
            DependencyProperty.Register("SeriesCollection", typeof(SeriesCollection), typeof(UserControl1));

        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public UserControl1()
        {
            InitializeComponent();
        }
    }
}
