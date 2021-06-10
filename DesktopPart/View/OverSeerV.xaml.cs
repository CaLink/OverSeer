using DesktopPart.Model;
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
using System.Windows.Shapes;

namespace DesktopPart.View
{
    /// <summary>
    /// Логика взаимодействия для OverSeerV.xaml
    /// </summary>
    public partial class OverSeerV : Window
    {
        public OverSeerV()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Data.Pc == null)
                return;
            
        }

        public SeriesCollection RemakeChart(int cores)
        {
            SeriesCollection tempo = new SeriesCollection();


            for (int i = 0; i < Data.Pc.GeneralInfo.LogicalProcessors; i++)
            {
                LineSeries temp = new LineSeries();
                temp.Title = "Core " + (i + 1);
                temp.Values = new ChartValues<double>();
                tempo.Add(temp);

                //CpuChartByCore.Add(new LineSeries() { Title = $"Core{i + 1}", Values = new ChartValues<double>() });

            }


            sas.Series = tempo;

            return sas.Series;
        }
    }
}
