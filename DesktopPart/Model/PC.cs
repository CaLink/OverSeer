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
        public string Name { get; set; }
        public string IP { get; set; }

        
    }

    public class PcGroupe
    {
        public string Name { get; set; }
        public ObservableCollection<PC> PCs { get; set; }

        public PcGroupe()
        {
            PCs = new ObservableCollection<PC>();
        }
    }
}
