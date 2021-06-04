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
    public class PcGroupsController : ApiController
    {
        private BasedEntities db = new BasedEntities();

        // GET: api/PcGroups
        public IQueryable<PcGroup> GetPcGroups()
        {

            //return db.PcGroups;

            return null;
        }

        // GET: api/PcGroups/5
        [ResponseType(typeof(PcGroup))]
        public async Task<IHttpActionResult> GetPcGroup(int id)
        {
            /*
            PcGroup pcGroup = await db.PcGroups.FindAsync(id);
            if (pcGroup == null)
            {
                return NotFound();
            }

            return Ok(pcGroup);
            */
            return BadRequest();
        }

        // PUT: api/PcGroups/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPcGroup(int id, PcGroup pcGroup)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pcGroup.id)
            {
                return BadRequest();
            }

            db.Entry(pcGroup).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PcGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
            */
            return BadRequest();
        }

        // POST: api/PcGroups
        [ResponseType(typeof(List<PcGroupM>))]
        public async Task<IHttpActionResult> PostPcGroup(List<PcGroupM> pcGroup)
        {
            /*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PcGroups.Add(pcGroup);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pcGroup.id }, pcGroup);
            */
            /*
            PcGroup toAdd = new PcGroup() { Name = pcGroup.Name};

            db.PcGroups.Add(toAdd);

            try
            {
                await db.SaveChangesAsync();
                return Ok((PcGroupM)toAdd);
            }
            catch (Exception e)
            {
                return Ok(new PcGroupM { id =-1});
            }*/



            try
            {
                await db.SaveChangesAsync();
                return Ok(new List<PcGroup>() { new PcGroup() });
            }
            catch (Exception)
            {
                return Ok(new List<PcGroup>());
            }




        }

        // DELETE: api/PcGroups/5
        [ResponseType(typeof(PcGroupM))]
        public async Task<IHttpActionResult> DeletePcGroup(int id)
        {

            List<Pc> pcs = db.Pcs.Where(x => x.PcGroupID == id).ToList();
            pcs.ForEach(x => { x.PcGroupID = 1; db.Entry(x).State = EntityState.Modified; });

            PcGroup group = await db.PcGroups.FindAsync(id);
            db.PcGroups.Remove(group);

            try
            {
                await db.SaveChangesAsync();
                return Ok(new PcGroupM() { id = 1 }); ;
            }
            catch (Exception)
            {
                return Ok(new PcGroupM() { id = -1 }); ;

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

        private bool PcGroupExists(int id)
        {
            return db.PcGroups.Count(e => e.id == id) > 0;
        }
    }
}