using DesktopPart.Model;
using DesktopPart.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace DesktopPart.ModelView
{
    public class EditMV : NotifyModel
    {
        public ObservableCollection<PC> PCs { get; set; }
        public ObservableCollection<PC> GroupePC { get; set; }
        public PC ChosenAllPC { get; set; }


        public CustomCUMmand<string> AddPC { get; set; }
        public CustomCUMmand<string> RemovePC { get; set; }
        public CustomCUMmand<string> Save { get; set; }

        public EditMV()
        {
            Init();

            AddPC = new CustomCUMmand<string>(
                (s) =>
                {
                    new AddPC().ShowDialog();
                });

            RemovePC = new CustomCUMmand<string>(
                (s) =>
                {
                    Data.PCs.Remove(ChosenAllPC);
                },
                () =>
                {
                    if (ChosenAllPC == null)
                        return false;
                    else
                        return true;
                });

            Save = new CustomCUMmand<string>(
                (s) =>
                {
                    using(FileStream fs = new FileStream("PC.List",FileMode.Create,FileAccess.Write))
                    using(StreamWriter sw = new StreamWriter(fs))
                    {
                       string json = JsonSerializer.Serialize<ObservableCollection<PC>>(PCs);
                       sw.WriteLine(json);
                    }
                });
        }

        void Init()
        {
            PCs = Data.PCs;
        }
    }
}
