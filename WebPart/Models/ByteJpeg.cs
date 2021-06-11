using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.Models
{
    [Serializable]
    public class ByteJpeg
    {
        public byte[] Jpeg { get; set; }
    }
}