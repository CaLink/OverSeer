using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ServicePart
{
    public partial class OverSeerService : ServiceBase
    {
        Timer timer;
        List<Process> process;
        int  eventID = 1;

        public OverSeerService()
        {
            InitializeComponent();

            //Работа с эвентами
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource("MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("ITS ALIVE"); // Стартуем


            timer =  new Timer(); //Настройка таймера
            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(this.TimeToStart);
            timer.Start();

        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Murder"); // !Стартуем
        }

        private void TimeToStart(object sender, ElapsedEventArgs e)
        {
            string mail=$"{DateTime.Now}\n";
            process = Process.GetProcesses().ToList();

            process.ForEach(x => mail += $"{x.Id}, {x.ProcessName}");

            eventLog1.WriteEntry(mail,EventLogEntryType.Information,eventID++);
        }
    }
}
