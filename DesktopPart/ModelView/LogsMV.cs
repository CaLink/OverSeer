using DesktopPart.Model;
using DesktopPart.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.ModelView
{
    public class LogsMV : NotifyModel
    {

        public List<PC> Pcs { get; set; }

        private PC selectedPC;
        public PC SelectedPC { get { return selectedPC; } set { selectedPC = value; RaiseEvent(nameof(SelectedPC)); } }

        string contain;
        public string Contain { get => contain; set { contain = value; RaiseEvent(nameof(Contain)); } }


        public List<LogsM> FullLogsList { get; set; }
        ObservableCollection<LogsM> selectedLogs;

        public ObservableCollection<LogsM> SelectedLogs { get=>selectedLogs; set { selectedLogs = value;RaiseEvent(nameof(SelectedLogs)); } }


        public CustomCUMmand<string> Find { get; set; }
        public CustomCUMmand<string> Drop{ get; set; }
        public CustomCUMmand<string> Close { get; set; }
        public CustomCUMmand<string> Refresh { get; set; }


        public LogsMV()
        {
            LogsInit();

            Find = new CustomCUMmand<string>
                (s =>
                {
                    SelectLogs();

                },
                () =>
                {
                    if (SelectedPC != null || !string.IsNullOrWhiteSpace(Contain))
                        return true;
                    else
                        return false;
                });

            Drop = new CustomCUMmand<string>
                (s =>
                {
                    SelectedPC = null;
                    Contain = "";
                    SelectLogs();

                },
                () =>
                {
                    if (SelectedPC != null || !string.IsNullOrWhiteSpace(Contain))
                        return true;
                    else
                        return false;
                });

            Refresh = new CustomCUMmand<string>
                (s =>
                {
                    GetLogs();
                });

            Close = new CustomCUMmand<string>
                (s =>
                {
                    Manager.Close(typeof(LogsV));
                });
        }

        private void LogsInit()
        {
            Pcs = new List<PC>();

            List<PcGroupe> tempGroup = Data.PcGroupe.ToList();
            tempGroup.ForEach(x => 
            {
                var temp = x.PcMs.ToList();
                Pcs.AddRange(temp);
            });

            GetLogs();

        }

        async void GetLogs()
        {
            List<TempLogs> tempL = await HttpMessage.MethodGet<TempLogs>("api/Logs");
            FullLogsList = new List<LogsM>();
            tempL.ForEach(x => FullLogsList.Add((LogsM)x));
            
            

            

            SelectLogs();
        }

        void SelectLogs()
        {
            if (SelectedPC != null & !string.IsNullOrWhiteSpace(Contain))
                SelectedLogs = new ObservableCollection<LogsM>(FullLogsList.Where(x => x.ID == SelectedPC.id & x.Message.Contains(Contain)).ToList());
            else if (SelectedPC != null)
                SelectedLogs = new ObservableCollection<LogsM>(FullLogsList.Where(x => x.ID == SelectedPC.id).ToList());
            else if (!string.IsNullOrWhiteSpace(Contain))
                SelectedLogs = new ObservableCollection<LogsM>(FullLogsList.Where(x => x.Message.Contains(Contain)).ToList());
            else
                SelectedLogs = new ObservableCollection<LogsM>(FullLogsList);

        }


    }
}
