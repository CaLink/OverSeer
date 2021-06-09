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
    public class PcEditorController : ApiController
    {
        private BasedEntities db = new BasedEntities();

        // GET: api/PcEditor
        public IQueryable<Pc> GetPcs()
        {
            return db.Pcs;
        }

        // GET: api/PcEditor/5
        [ResponseType(typeof(Pc))]
        public async Task<IHttpActionResult> GetPc(int id)
        {
            Pc pc = await db.Pcs.FindAsync(id);
            if (pc == null)
            {
                return NotFound();
            }

            return Ok(pc);
        }

        // PUT: api/PcEditor/5
        [ResponseType(typeof(PcM))]
        public async Task<IHttpActionResult> PutPc(int id, PcM pc)
        {
            Pc updatedPC = db.Pcs.Find(pc.id);
            PcGroup updatedGroup = db.PcGroups.Find(id);
            if (updatedPC == null || updatedGroup == null)
                return Ok(new PcM { id = -1 });

            updatedPC.PcGroupID = id;

            try
            {
                await db.SaveChangesAsync();
                return Ok((PcM)updatedPC);
            }
            catch (Exception)
            {
                return Ok(new PcM { id = -1 });
            }
        }

        // POST: api/PcEditor
        [ResponseType(typeof(PcM))]
        public async Task<IHttpActionResult> PostPc(PcM pc)
        {
            Pc updatedPC = db.Pcs.Find(pc.id);
            if (updatedPC == null)
                return Ok(new PcM { id = -1 });

            updatedPC.Name = pc.Name;
            updatedPC.IP = pc.IP;
            updatedPC.Port = pc.Port;

            try
            {
                await db.SaveChangesAsync();
                return Ok((PcM)updatedPC);
            }
            catch (Exception)
            {
                return Ok(new PcM { id = -1 });
            }
        }

        // DELETE: api/PcEditor/5
        [ResponseType(typeof(PcM))]
        public async Task<IHttpActionResult> DeletePc(int id)
        {
            Pc updatedPC = db.Pcs.Find(id);
            if (updatedPC == null)
                return Ok(new PcM { id = -1 });

            updatedPC.PcGroupID = 1;

            try
            {
                await db.SaveChangesAsync();
                return Ok((PcM)updatedPC);
            }
            catch (Exception)
            {
                return Ok(new PcM { id = -1 });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PcExists(int id)
        {
            return db.Pcs.Count(e => e.id == id) > 0;
        }
    }
}