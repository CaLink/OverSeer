using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class TempLogs
    {
        public long Date { get; set; }
        public int ID { get; set; }
        public string Message { get; set; }

        public static implicit operator TempLogs(Log from)
        {

            return new TempLogs
            {
                Date = from.Date.ToBinary(),
                ID = from.PcID,
                Message = from.Message
            };
        }
    }
}