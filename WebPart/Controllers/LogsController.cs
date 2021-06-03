using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebPart;
using WebPart.Models;

namespace WebPart.Controllers
{
    public class LogsController : ApiController
    {
        private BasedEntities db = new BasedEntities();

        // GET: api/Logs
        public List<TempLogs> GetLogs()
        {
            List<TempLogs> ret = new List<TempLogs>();
            db.Logs.ToList().ForEach(x => ret.Add((TempLogs)x));

            return ret;
        }

        // GET: api/Logs/5
        [ResponseType(typeof(Log))]
        public async Task<IHttpActionResult> GetLog(int id)
        {
            /*
            Log log = await db.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
            */
            return BadRequest();
        }

        // PUT: api/Logs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLog(int id, string mes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Log log = new Log()
            {
                PcID = id,
                Date = DateTime.Now,
                Message = mes

            };
            db.Logs.Add(log);

            try
            {
                await db.SaveChangesAsync();
                return StatusCode(HttpStatusCode.NoContent);

            }
            catch (Exception)
            {

                return StatusCode(HttpStatusCode.InternalServerError);

            }



            /*if (id != log.id)
            {
                return BadRequest();
            }

            db.Entry(log).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);*/
        }

        // POST: api/Logs
        [ResponseType(typeof(Log))]
        public async Task<IHttpActionResult> PostLog(Log log)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Logs.Add(log);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = log.id }, log);
            */
            return BadRequest();

        }

        // DELETE: api/Logs/5
        [ResponseType(typeof(Log))]
        public async Task<IHttpActionResult> DeleteLog(int id)
        {
            /*
            Log log = await db.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            db.Logs.Remove(log);
            await db.SaveChangesAsync();

            return Ok(log);
            */
            return BadRequest();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogExists(int id)
        {
            return db.Logs.Count(e => e.id == id) > 0;
        }
    }
}