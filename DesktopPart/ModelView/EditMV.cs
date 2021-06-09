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

        private string virgin;

        public List<PcGroupe> PcGroupes { get; set; }   //TODO Когда-нибудь я научусь не делать 10^6 одинаковых переменных
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
        public CustomCUMmand<string> UpdateName { get; set; }
        public CustomCUMmand<string> RemoveGroupe { get; set; }

        public EditMV()
        {

            Init();

            AddPC = new CustomCUMmand<string>(
                (s) =>
                {
                    switch (s)
                    {
                        case "Chouse": Data.Pc = ChosenGroupePC; break;
                        case "All": Data.Pc = ChosenAllPC; break;
                    }

                    Manager.AddWindowsOpen(new AddPC());

                    Data.Pc = new PC();


                });



            Save = new CustomCUMmand<string>(
                (s) =>
                {
                    List<PcGroupe> temp = new List<PcGroupe>();
                    temp.Add(UnGroupe);
                    temp.AddRange(MainGroupe);

                    List<PcGroupe> req = HttpMessage.MethodPost("api/PcGroups", temp).Result;

                    if (req.Count == 1)
                    {
                        Data.PcGroupe = new ObservableCollection<PcGroupe>(temp);
                        Manager.Close(typeof(EditV));
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Connection Error");
                        List<PcGroupe> virginList = (List<PcGroupe>)JsonSerializer.Deserialize(virgin, typeof(List<PcGroupe>));
                        Data.PcGroupe = new ObservableCollection<PcGroupe>(virginList);
                        Manager.Close(typeof(EditV));


                    }


                });

            Remove = new CustomCUMmand<string>(
                (s) =>
                {
                    PC ret = HttpMessage.MethodDell<PC>("api/PcEditor/", ChosenGroupePC.id).Result;

                    if (ret.id == -1)
                    {
                        System.Windows.MessageBox.Show("Wrong Input");
                        return;
                    }

                    PC temp = ChosenGroupePC;
                    UnGroupe.PcMs.Add(temp);
                    SelectedGroupe.PcMs.Remove(temp);
                    ChosenGroupePC = null;


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
                    PC ret = HttpMessage.MethodPut("api/PcEditor/"+ SelectedGroupe.id, ChosenAllPC ).Result;

                    if (ret.id == -1)
                    {
                        System.Windows.MessageBox.Show("Wrong Input");
                        return;
                    }


                    PC temp = ChosenAllPC;
                    SelectedGroupe.PcMs.Add(temp);
                    UnGroupe.PcMs.Remove(temp);
                    ChosenAllPC = null;

                },
                () =>
                {
                    if (ChosenAllPC == null || SelectedGroupe == null)
                        return false;
                    else
                        return true;
                });

            AddGroupe = new CustomCUMmand<string>(
                (s) =>
                {

                    PcGroupe newGroup = new PcGroupe() { Name = "NewGroup" };
                    newGroup = HttpMessage.MethodPost("api/PcGroups", newGroup).Result;

                    if (newGroup.id == 0 || string.IsNullOrWhiteSpace(newGroup.Name))
                    {
                        System.Windows.MessageBox.Show("Wrong Input");
                        return;
                    }

                    MainGroupe.Add(newGroup);


                });


            UpdateName = new CustomCUMmand<string>(
                (s) =>
                {
                    PcGroupe result = HttpMessage.MethodPut("api/PcGroups/" + SelectedGroupe.id, SelectedGroupe).Result;
                    if(result.id != selectedGroupe.id)
                    {
                        System.Windows.MessageBox.Show("Wrong Input");
                    }


                }, () =>
                {
                    if (SelectedGroupe != null)
                        if (!string.IsNullOrWhiteSpace(SelectedGroupe.Name))
                        {
                            return true;
                        }
                        else
                            return false;
                    else
                        return false;
                });




            RemoveGroupe = new CustomCUMmand<string>(
                (s) =>
                {

                    //TODO Ебанет?
                    PcGroupe respounce = HttpMessage.MethodDell<PcGroupe>("api/PcGroups", SelectedGroupe.id).Result;

                    if (respounce.id == -1)
                    {
                        System.Windows.MessageBox.Show("WrongInput");
                        return;
                    }




                    PcGroupe temp = SelectedGroupe;
                    temp.PcMs.ToList().ForEach(x => unGroupe.PcMs.Add(x));
                    MainGroupe.Remove(temp);
                    Data.PcGroupe.Remove(temp);
                    SelectedGroupe = null;

                },
                () =>
                {
                    if (SelectedGroupe == null)
                        return false;
                    else
                        return true;
                });

        }

        void Init()
        {
            PcGroupes = new List<PcGroupe>(Data.PcGroupe);

            MainGroupe = new ObservableCollection<PcGroupe>(PcGroupes.Where(x => x.id != 1).ToList());
            UnGroupe = PcGroupes.First(); //TODO Ебанет?

            virgin = JsonSerializer.Serialize(PcGroupes);

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
