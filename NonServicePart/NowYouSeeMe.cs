using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NonServicePart
{
    class NowYouSeeMe
    {
        NotifyIcon tray;

        public NowYouSeeMe()
        {
            tray = new NotifyIcon();
            tray.Icon = Properties.Resources.O4ko;
            tray.Visible = true;

            ContextMenu menu = new ContextMenu();

            menu.MenuItems.Add(
                new MenuItem("Exit", (o, e) => Environment.Exit(0)));
            tray.ContextMenu = menu;
        }
    }
}
