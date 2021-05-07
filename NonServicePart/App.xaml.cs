using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NonServicePart
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        HeyListen navi;
        NowYouSeeMe snus;
        
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            navi = new HeyListen();
            snus = new NowYouSeeMe();
        }
    }
}
