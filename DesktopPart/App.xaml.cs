using DesktopPart.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DesktopPart
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Manager.AddWindowsOpen(new OverSeerV());
        }
    }

    static class Manager
    {
        static List<Window> openedWindows = new List<Window>();

        public static void AddWindowsOpen(Window win)
        {
            win.Closing += Win_Closing;
            openedWindows.Add(win);
            win.ShowDialog();
        }

        private static void Win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((Window)sender).Closing -= Win_Closing;
            openedWindows.Remove((Window)sender);
            TestToExit();
        }

        internal static void Close(Type type)
        {
            var list = openedWindows.FindAll(s => s.GetType() == type);
            list.ForEach(s => s.Close());
        }

        private static void TestToExit()
        {
            if (openedWindows.Count == 0)
                App.Current.Shutdown();
        }

        public static OverSeerV GetOverSeerV()
        {
            return (OverSeerV)openedWindows[0];
        }

    }
}
