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
    public class PcDrivesController : ApiController
    {
        private BasedEntities db = new BasedEntities();

        // GET: api/PcDrives
        public IQueryable<PcDrive> GetPcDrives()
        {
            //return db.PcDrives;
            return null;
        }

        // GET: api/PcDrives/5
        [ResponseType(typeof(PcDrive))]
        public async Task<IHttpActionResult> GetPcDrive(int id)
        {
            /*
            PcDrive pcDrive = await db.PcDrives.FindAsync(id);
            if (pcDrive == null)
            {
                return NotFound();
            }

            return Ok(pcDrive);
            */
            return BadRequest();
        }

        // PUT: api/PcDrives/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPcDrive(int id, List<PcDriveMA> pcDrive)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /*
            if (id != pcDrive.id)
            {
                return BadRequest();
            }
            */

            List<PcDrive> curetnPcDrive = db.PcDrives.Where(x => x.PcID == id).ToList();

            if (curetnPcDrive.Count == 0)
            {
                foreach (var item in pcDrive)
                {
                    PcDrive newDrive = new PcDrive();
                    newDrive.PcID = id;
                    newDrive.Drive = item.Drive;
                    newDrive.DriveType = item.DriveType;
                    newDrive.FileSystem = item.FileSystem;
                    newDrive.AvailabeSpace = (int)item.AvailabeSpace;
                    newDrive.TotalSize = (int)item.TotalSize;

                    db.PcDrives.Add(newDrive);
                }
            }
            else if (curetnPcDrive.Count == pcDrive.Count)
            {
                for (int i = 0; i < curetnPcDrive.Count; i++)
                {
                    //curetnPcDrive[i].PcID = id;
                    curetnPcDrive[i].Drive = pcDrive[i].Drive;
                    curetnPcDrive[i].DriveType = pcDrive[i].DriveType;
                    curetnPcDrive[i].FileSystem = pcDrive[i].FileSystem;
                    curetnPcDrive[i].AvailabeSpace = (int)pcDrive[i].AvailabeSpace;
                    curetnPcDrive[i].TotalSize = (int)pcDrive[i].TotalSize;
                    db.Entry(curetnPcDrive[i]).State = EntityState.Modified;
                }
            }
            else if (curetnPcDrive.Count < pcDrive.Count)
            {
                int i = 0;
                for (; i < curetnPcDrive.Count; i++)
                {
                    curetnPcDrive[i].Drive = pcDrive[i].Drive;
                    curetnPcDrive[i].DriveType = pcDrive[i].DriveType;
                    curetnPcDrive[i].FileSystem = pcDrive[i].FileSystem;
                    curetnPcDrive[i].AvailabeSpace = (int)pcDrive[i].AvailabeSpace;
                    curetnPcDrive[i].TotalSize = (int)pcDrive[i].TotalSize;
                    db.Entry(curetnPcDrive[i]).State = EntityState.Modified;
                }
                for (; i < pcDrive.Count; i++)
                {
                    PcDrive newDrive = new PcDrive();
                    newDrive.PcID = id;
                    newDrive.Drive = pcDrive[i].Drive;
                    newDrive.DriveType = pcDrive[i].DriveType;
                    newDrive.FileSystem = pcDrive[i].FileSystem;
                    newDrive.AvailabeSpace = (int)pcDrive[i].AvailabeSpace;
                    newDrive.TotalSize = (int)pcDrive[i].TotalSize;

                    db.PcDrives.Add(newDrive);
                }
            }
            else if (curetnPcDrive.Count > pcDrive.Count)
            {
                int i = 0;
                for (; i < pcDrive.Count; i++)
                {
                    curetnPcDrive[i].Drive = pcDrive[i].Drive;
                    curetnPcDrive[i].DriveType = pcDrive[i].DriveType;
                    curetnPcDrive[i].FileSystem = pcDrive[i].FileSystem;
                    curetnPcDrive[i].AvailabeSpace = (int)pcDrive[i].AvailabeSpace;
                    curetnPcDrive[i].TotalSize = (int)pcDrive[i].TotalSize;
                    db.Entry(curetnPcDrive[i]).State = EntityState.Modified;
                }
                for (; i < curetnPcDrive.Count; i++)
                {
                    db.PcDrives.Remove(curetnPcDrive[i]);
                }
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

        // POST: api/PcDrives
        [ResponseType(typeof(PcDrive))]
        public async Task<IHttpActionResult> PostPcDrive(PcDrive pcDrive)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PcDrives.Add(pcDrive);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pcDrive.id }, pcDrive);
            */
            return BadRequest();
        }

        // DELETE: api/PcDrives/5
        [ResponseType(typeof(PcDrive))]
        public async Task<IHttpActionResult> DeletePcDrive(int id)
        {
            /*
            PcDrive pcDrive = await db.PcDrives.FindAsync(id);
            if (pcDrive == null)
            {
                return NotFound();
            }

            db.PcDrives.Remove(pcDrive);
            await db.SaveChangesAsync();

            return Ok(pcDrive);
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

        private bool PcDriveExists(int id)
        {
            return db.PcDrives.Count(e => e.id == id) > 0;
        }
    }
}