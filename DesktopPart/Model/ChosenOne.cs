using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DesktopPart.Model
{
    public class ChosenOne
    {
        /*
        public int CpuLoad { get; set; }
        public int RamLoad { get; set; }
        public BitmapImage BMP { get; set; }
        public ObservableCollection<Process> Processes { get; set; }

        public string CpuName { get; set; }
        public string RamTotalValue { get; set; }

        public List<int> CpuValues { get; set; }
        public List<int> RamValues { get; set; }

        public ChosenOne()
        {
            CpuLoad = 0;
            RamLoad = 0;
            BMP = new BitmapImage();
            Processes = new ObservableCollection<Process>();

            CpuName = "";
            RamTotalValue = "";

            CpuValues = new List<int>();
            RamValues = new List<int>();
        }
        */

        public PcInfo PcInfo{ get; set; }
        public ProcessInfo ProcessInfo { get; set; }
        public Bitmap Bmp{ get; set; }

    }
}
