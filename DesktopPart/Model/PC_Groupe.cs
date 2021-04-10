using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.Model
{
    public class PC_Groupe
    {
        public string Name { get; set; }
        public ObservableCollection<PC> PCs { get; set; }

        public PC_Groupe()
        {
            PCs = new ObservableCollection<PC>();
        }

    }
}
