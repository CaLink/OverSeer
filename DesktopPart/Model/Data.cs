using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DesktopPart.Model
{
    public static class Data
    {
        public static ObservableCollection<PcGroupe> PcGroupe { get; set; } = new ObservableCollection<PcGroupe>();
        public static BitmapImage Bmp { get; set; } = new BitmapImage();
        public static PC Pc { get; set; } = new PC();
    }
}
