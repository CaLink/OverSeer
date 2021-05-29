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
        const string sUPERsECRETnAME = "BiggerLongerUncut";

        public ObservableCollection<PcGroupe> PcGroupes { get; set; }   //TODO Когда-нибудь я научусь не делать 10^6 одинаковых переменных
        public ObservableCollection<PcGroupe> MainGroupe { get; set; }


        PcGroupe selectedGroupe;
        public PcGroupe SelectedGroupe { get { return selectedGroupe; } set { selectedGroupe = value; RaiseEvent(nameof(SelectedGroupe)); } }

        PcGroupe unGroupe;
        public PcGroupe UnGroupe { get { return unGroupe; } set { unGroupe = value; RaiseEvent(nameof(UnGroupe)); } }

        public PC ChosenAllPC { get; set; }
        public PC ChosenGroupePC { get; set; }



        public CustomCUMmand<string> AddPC { get; set; }
        public CustomCUMmand<string> RemovePC { get; set; }
        public CustomCUMmand<string> Save { get; set; }
        public CustomCUMmand<string> Remove { get; set; }
        public CustomCUMmand<string> Add { get; set; }
        public CustomCUMmand<string> AddGroupe { get; set; }
        public CustomCUMmand<string> RemoveGroupe { get; set; }

        public EditMV()
        {
            /*
            Init();

            AddPC = new CustomCUMmand<string>(
                (s) =>
                {
                    Manager.AddWindowsOpen(new AddPC());
                    if (Data.Pc != null)
                        UnGroupe.PCs.Add(Data.Pc);
                    Data.Pc = null;
                });



            Save = new CustomCUMmand<string>(
                (s) =>
                {
                    SaveFunc();



                    Manager.Close(typeof(EditV));
                });

            RemovePC = new CustomCUMmand<string>(
                (s) =>
                {
                    UnGroupe.PCs.Remove(ChosenAllPC);
                },
                () =>
                {
                    if (ChosenAllPC == null)
                        return false;
                    else
                        return true;
                });

            Remove = new CustomCUMmand<string>(
                (s) =>
                {
                    PC temp = ChosenGroupePC;
                    UnGroupe.PCs.Add(temp);
                    SelectedGroupe.PCs.Remove(temp);

                },
                () =>
                {
                    if (ChosenGroupePC == null)
                        return false;
                    else
                        return true;
                });

            Add = new CustomCUMmand<string>(
                (s) =>
                {
                    PC temp = ChosenAllPC;
                    SelectedGroupe.PCs.Add(temp);
                    UnGroupe.PCs.Remove(temp);
                },
                () =>
                {
                    if (ChosenAllPC == null)
                        return false;
                    else
                        return true;
                });

            AddGroupe = new CustomCUMmand<string>(
                (s) =>
                {
                    SelectedGroupe = new PcGroupe { Name = "NewGroupe"};
                    MainGroupe.Add(SelectedGroupe);
                    
                });


            RemoveGroupe = new CustomCUMmand<string>(
                (s) =>
                {
                    foreach (var item in SelectedGroupe.PCs)
                    {
                        UnGroupe.PCs.Add(item);
                    }

                    MainGroupe.Remove(SelectedGroupe);

                    SelectedGroupe = null;
                },
                () =>
                {
                    if (SelectedGroupe == null)
                        return false;
                    else
                        return true;
                });
            */
        }

        void Init()
        {
            PcGroupes = Data.PcGroupe;
            List<PcGroupe> tempUnCat = PcGroupes.Where(s => s.Name == sUPERsECRETnAME).ToList(); //TODO Придумать Сложно И клАССное имя (ну или зделать проверку на дублирование)
            List<PcGroupe> tempCat = PcGroupes.Where(s => s.Name != sUPERsECRETnAME).ToList(); //TODO Придумать Сложно И клАССное имя (ну или зделать проверку на дублирование)

            if (tempUnCat.Count() != 0)
            {
                UnGroupe = tempUnCat[0];
                MainGroupe = new ObservableCollection<PcGroupe>(tempCat);

            }
            else
            {
                MainGroupe = new ObservableCollection<PcGroupe>(tempCat);
                UnGroupe = new PcGroupe() { Name = sUPERsECRETnAME };
            }


        }

        void SaveFunc()
        {
            List<PcGroupe> temp = new List<PcGroupe>();

            temp.AddRange(MainGroupe);
            temp.Add(UnGroupe);

            using (FileStream fs = new FileStream("Pc.Groupe", FileMode.Create, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                string json = JsonSerializer.Serialize<List<PcGroupe>>(temp, new JsonSerializerOptions() { WriteIndented = true });
                sw.WriteLine(json);
            }

            Data.PcGroupe = new ObservableCollection<PcGroupe>(temp);
        }
    }
}
