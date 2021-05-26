using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class PcM
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public int GroupID { get; set; }
        public List<PcDriveM> DriveList { get; set; }
        public PcGeneralInfoM GeneralInfo{ get; set; }



        public static implicit operator PcM(Pc from)
        {

            return new PcM
            {
                id = from.id,
                Name = from.Name,
                IP = from.IP,
                Port = from.Port,
                GroupID = from.PcGroupID
                
            };

        }
    }


}