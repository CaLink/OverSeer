using DesktopPart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.ModelView
{
    public class AddPCMV
    {
        public PC NewPC{ get; set; }

        public string Name { get; set; }
        public string Ip { get; set; }

        public CustomCUMmand<string> Save { get; set; }
        public AddPCMV()
        {

            Save = new CustomCUMmand<string>(
                (s) => 
                {
                    NewPC = new PC() { IP = Ip, Name = Name};
                    Data.PCs.Add(NewPC);
                    
                },()=>
                {
                    if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Ip))
                        return false;
                    else
                        return true;
                });
        }
    }
}
