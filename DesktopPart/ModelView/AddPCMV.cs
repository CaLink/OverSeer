using DesktopPart.Model;
using DesktopPart.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.ModelView
{
    public class AddPCMV
    {
        public PC Pc{ get; set; }

        public CustomCUMmand<string> Save { get; set; }
        public AddPCMV()
        {

            Pc = Data.Pc;

            Save = new CustomCUMmand<string>(
                (s) => 
                {
                    PC res = HttpMessage.MethodPost("api/PcEditor/", Pc).Result;
                    
                    if (res.id == -1)
                    {
                        System.Windows.MessageBox.Show("Wrong Input");
                        return;
                    }

                    Manager.Close(typeof(AddPC));
                },()=>
                {
                    if (string.IsNullOrEmpty(Pc.Name) || string.IsNullOrEmpty(Pc.IP) || string.IsNullOrEmpty(Pc.Port.ToString()))
                        return false;
                    else
                        return true;
                });
        }
    }
}
