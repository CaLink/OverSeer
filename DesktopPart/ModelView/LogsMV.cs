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
        public PC SelectedPC { get { return selectedPC; } set { selectedPC = value; } }

        string contain;
        public string Contain { get => contain; set { contain = value; } }


        public List<LogsM> FullLogsList { get; set; }
        public ObservableCollection<LogsM> SelectedLogs { get; set; }


        public CustomCUMmand<string> Find { get; set; }
        public CustomCUMmand<string> Close { get; set; }
        public CustomCUMmand<string> Refresh { get; set; }


        public LogsMV()
        {
            LogsInit();

            Find = new CustomCUMmand<string>
                (s =>
                {
                    //Если что-то выбрано, то сделай поиск

                },
                () =>
                {
                    if (SelectedPC != null || !string.IsNullOrWhiteSpace(Contain))
                        return false;
                    else
                        return true;
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
            throw new NotImplementedException();
        }

        void GetLogs()
        {
        
        }


    }
}
