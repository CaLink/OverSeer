using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    public class LogsM
    {
        public DateTime Date { get; set; }
        public int ID { get; set; }
        public string Message { get; set; }

        public static implicit operator LogsM(Log from)
        {

            return new LogsM
            {

                Date = from.Date,
                ID = from.PcID,
                Message = from.Message
            };
        }
    }
}