using DesktopPart.Model;
using DesktopPart.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesktopPart.ModelView
{
    public class OverSeerMV : NotifyModel
    {
        public ObservableCollection<PC> PCs { get; set; }

        PC selectedPC;
        public PC SelectedPC { get { return selectedPC; } set { selectedPC = value; } }
        public ChosenOne SelectedPCInfo { get; set; }



        public CustomCUMmand<string> OpenSMT { get; set; }
        public CustomCUMmand<string> GetInfo { get; set; }

        public OverSeerMV()
        {
            Init();

            OpenSMT = new CustomCUMmand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case "Edit": new EditV().ShowDialog(); break;
                        case "Log": break;
                        case "Settings": break;
                        case "About": break;
                    }
                });

            GetInfo = new CustomCUMmand<string>(
                (s) =>
                {
                    
                });
        }

        private void Init()
        {
            using (FileStream fs = new FileStream("PC.List", FileMode.OpenOrCreate, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                Data.PCs = new ObservableCollection<PC>();
                PCs = Data.PCs;
                string json = sr.ReadToEnd();
                if (string.IsNullOrEmpty(json))
                    return;

                Data.PCs = JsonSerializer.Deserialize<ObservableCollection<PC>>(json); 
                PCs = Data.PCs;
            }






            //PCs = new ObservableCollection<PC_Groupe>()
            //{
            //    new PC_Groupe()
            //    {
            //        Name = "SaS",
            //        PCs = new ObservableCollection<PC>()
            //        {
            //            new PC{Name="qwe",IP="1488"},
            //            new PC{Name="qwa",IP="1488"},
            //        }

            //    },
            //    new PC_Groupe()
            //    {
            //        Name = "SoS",
            //        PCs = new ObservableCollection<PC>()
            //        {
            //            new PC{Name="qwe",IP="1488"},
            //            new PC{Name="qwa",IP="1488"},
            //        }

            //    }
            //}; //Чтение из файла сделать
        }

        
    }
}
