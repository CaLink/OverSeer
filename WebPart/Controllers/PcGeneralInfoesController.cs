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
    public class PcGeneralInfoesController : ApiController
    {
        private BasedEntities db = new BasedEntities();

        // GET: api/PcGeneralInfoes
        public IQueryable<PcGeneralInfo> GetPcGeneralInfoes()
        {
            //   return db.PcGeneralInfoes;
            return null;
        }

        // GET: api/PcGeneralInfoes/5
        [ResponseType(typeof(PcGeneralInfo))]
        public async Task<IHttpActionResult> GetPcGeneralInfo(int id)
        {
            /*
            PcGeneralInfo pcGeneralInfo = await db.PcGeneralInfoes.FindAsync(id);
            if (pcGeneralInfo == null)
            {
                return NotFound();
            }

            return Ok(pcGeneralInfo);
            */
            return BadRequest();
        }

        // PUT: api/PcGeneralInfoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPcGeneralInfo(int id, PcGeneralInfoMA pcGeneralInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*
            if (id != pcGeneralInfo.PcID)
            {
                return BadRequest();
            }
            */

            if (!(db.PcGeneralInfoes.Where(x=>x.PcID == id).Count() > 0))
            {
                PcGeneralInfo newInfo = new PcGeneralInfo();
                newInfo.PcID = id;
                newInfo.Cpu = pcGeneralInfo.Cpu;
                newInfo.Cores = (short)pcGeneralInfo.Cores;
                newInfo.LogicalProcessors = (short)pcGeneralInfo.LogicalProcessors;
                newInfo.Socket = pcGeneralInfo.Socket;
                newInfo.Ram = (short)pcGeneralInfo.Ram;
                newInfo.SystemName = pcGeneralInfo.SystemName;
                newInfo.OsArchitecture = pcGeneralInfo.OsArchitecture;
                newInfo.OsVersion = pcGeneralInfo.OsVersion;

                db.PcGeneralInfoes.Add(newInfo);
                
            }
            else
            {
                PcGeneralInfo updateInfo = db.PcGeneralInfoes.Find(id);

                updateInfo.Cpu = pcGeneralInfo.Cpu;
                updateInfo.Cores = (short)pcGeneralInfo.Cores;
                updateInfo.LogicalProcessors = (short)pcGeneralInfo.LogicalProcessors;
                updateInfo.Socket = pcGeneralInfo.Socket;
                updateInfo.Ram = (short)pcGeneralInfo.Ram;
                updateInfo.SystemName = pcGeneralInfo.SystemName;
                updateInfo.OsArchitecture = pcGeneralInfo.OsArchitecture;
                updateInfo.OsVersion = pcGeneralInfo.OsVersion;

                db.Entry(updateInfo).State = EntityState.Modified;
                
            }

            try
            {
                await db.SaveChangesAsync();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {

                return StatusCode(HttpStatusCode.InternalServerError);
            }
            

            /*try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PcGeneralInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            */


        }

        // POST: api/PcGeneralInfoes
        [ResponseType(typeof(PcGeneralInfo))]
        public async Task<IHttpActionResult> PostPcGeneralInfo(PcGeneralInfo pcGeneralInfo)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PcGeneralInfoes.Add(pcGeneralInfo);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PcGeneralInfoExists(pcGeneralInfo.PcID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pcGeneralInfo.PcID }, pcGeneralInfo);
            */
            return BadRequest();
        }

        // DELETE: api/PcGeneralInfoes/5
        [ResponseType(typeof(PcGeneralInfo))]
        public async Task<IHttpActionResult> DeletePcGeneralInfo(int id)
        {
            /*
            PcGeneralInfo pcGeneralInfo = await db.PcGeneralInfoes.FindAsync(id);
            if (pcGeneralInfo == null)
            {
                return NotFound();
            }

            db.PcGeneralInfoes.Remove(pcGeneralInfo);
            await db.SaveChangesAsync();

            return Ok(pcGeneralInfo);
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

        private bool PcGeneralInfoExists(int id)
        {
            return db.PcGeneralInfoes.Count(e => e.PcID == id) > 0;
        }
    }
}