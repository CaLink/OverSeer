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
    public class PcLoadInfoesController : ApiController
    {
        private BasedEntities db = new BasedEntities();

        // GET: api/PcLoadInfoes
        public IQueryable<PcLoadInfo> GetPcLoadInfoes()
        {
            /*
            return db.PcLoadInfoes;
            */
            return null;

        }

        // GET: api/PcLoadInfoes/5
        [ResponseType(typeof(PcLoadInfo))]
        public async Task<IHttpActionResult> GetPcLoadInfo(int id)
        {
            /*
            PcLoadInfo pcLoadInfo = await db.PcLoadInfoes.FindAsync(id);
            if (pcLoadInfo == null)
            {
                return NotFound();
            }

            return Ok(pcLoadInfo);
            */
            return BadRequest();
        }

        // PUT: api/PcLoadInfoes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPcLoadInfo(int id, PcLoadInfoM pcLoadInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != pcLoadInfo.PcID)
            //{
            //    return BadRequest();
            //}

            List<PcLoadInfo> pci = db.PcLoadInfoes.Where(x => x.PcID == id).ToList();

            if (pci.Count > 0)
            {
                pci[0].CpuLoad = pcLoadInfo.CpuLoad;

                string clbc = "";
                for (int i = 0; i < pcLoadInfo.CpuLoadByCore.Count; i++)
                {
                    clbc += pcLoadInfo.CpuLoadByCore[i];
                    if (i + 1 < pcLoadInfo.CpuLoadByCore.Count)
                        clbc += "/";
                }
                
                pci[0].CpuLoadByCore = clbc;
                pci[0].RamLoad = pcLoadInfo.RamLoad;

                db.Entry(pci[0]).State = EntityState.Modified;
            }
            else
            {
                PcLoadInfo pci2 = new PcLoadInfo();
                pci2.CpuLoad = pcLoadInfo.CpuLoad;

                string clbc = "";
                for (int i = 0; i < pcLoadInfo.CpuLoadByCore.Count; i++)
                {
                    clbc += pcLoadInfo.CpuLoadByCore[i];
                    if (i + 1 < pcLoadInfo.CpuLoadByCore.Count)
                        clbc += "/";
                }

                pci2.CpuLoadByCore = clbc;
                pci2.RamLoad = pcLoadInfo.RamLoad;
                pci2.PcID = id;
                db.PcLoadInfoes.Add(pci2);

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

        }

        // POST: api/PcLoadInfoes
        [ResponseType(typeof(PcLoadInfo))]
        public async Task<IHttpActionResult> PostPcLoadInfo(PcLoadInfo pcLoadInfo)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PcLoadInfoes.Add(pcLoadInfo);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PcLoadInfoExists(pcLoadInfo.PcID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pcLoadInfo.PcID }, pcLoadInfo);
            */
            return BadRequest();
        }

        // DELETE: api/PcLoadInfoes/5
        [ResponseType(typeof(PcLoadInfo))]
        public async Task<IHttpActionResult> DeletePcLoadInfo(int id)
        {
            /*
            PcLoadInfo pcLoadInfo = await db.PcLoadInfoes.FindAsync(id);
            if (pcLoadInfo == null)
            {
                return NotFound();
            }

            db.PcLoadInfoes.Remove(pcLoadInfo);
            await db.SaveChangesAsync();

            return Ok(pcLoadInfo);
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

        private bool PcLoadInfoExists(int id)
        {
            return db.PcLoadInfoes.Count(e => e.PcID == id) > 0;
        }
    }
}