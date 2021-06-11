using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Web.Http;
using WebPart.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebPart.Controllers
{


    public class ByteJpegController : ApiController
    {
        BasedEntities db = new BasedEntities();

        // GET: api/ByteJpeg
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ByteJpeg/5
        public string Get(int id)
        {
            Pc pc = db.Pcs.Find(id);
            if (pc == null)
                return null;
            else
                if (string.IsNullOrWhiteSpace(pc.IP))
                return null;

            string retMess = "";
            try
            {
                byte[] data = Encoding.UTF8.GetBytes("jpg");

                TcpClient client = new TcpClient();
                client.Connect(pc.IP, 1488);

                NetworkStream ns = client.GetStream();
                ns.Write(data, 0, data.Length);

                data = new byte[64];
                int bytes = 0;

                StringBuilder sb = new StringBuilder();
                do
                {
                    bytes = ns.Read(data, 0, data.Length);
                    sb.Append(Encoding.UTF8.GetString(data, 0, bytes));

                }
                while (ns.DataAvailable);

                ns.Close();
                client.Close();

                retMess = sb.ToString();
            }
            catch (Exception e)
            {
                return null;
            }

            /*
            ByteJpeg byteJpeg = (ByteJpeg)JsonSerializer.Deserialize(retMess,typeof(ByteJpeg));

            return byteJpeg;
            */
            return retMess;
        }

        // POST: api/ByteJpeg
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ByteJpeg/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ByteJpeg/5
        public void Delete(int id)
        {
        }
    }
}
