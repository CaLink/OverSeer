using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DesktopPart.Model
{
    public class PC
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public List<PcDrive> DriveList { get; set; }
        public PcGeneralInfo GeneralInfo { get; set; }
        
    }

    public class PcGroupe
    {
        public int id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<PC> PcMs { get; set; }

        public PcGroupe()
        {
            PcMs = new ObservableCollection<PC>();
        }
    }
}
