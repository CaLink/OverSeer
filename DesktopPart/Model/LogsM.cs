using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPart.Model
{
    public class LogsM
    {
        public DateTime Date { get; set; }
        public int ID { get; set; }
        public string Message { get; set; }

        public static implicit operator LogsM(TempLogs from)
        {
            return new LogsM
            {
                ID = from.ID,
                Message = from.Message,
                Date = DateTime.FromBinary(from.Date)
            };
        }
    }
}
