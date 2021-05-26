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
    public class PcsController : ApiController
    {
        private BasedEntities db = new BasedEntities();

        //TODO А почему он не 
        // А тут список груп, компов и их статической инфы
        // GET: api/Pcs
        public List<PcGroupM> GetPcs()
        {
            List<PcGroup> tempPcGroupList =  db.PcGroups.ToListAsync().Result;
            List<Pc> tempPcList = db.Pcs.ToListAsync().Result;

            List<PcGroupM> groupList = new List<PcGroupM>();
            tempPcGroupList.ForEach(x => groupList.Add((PcGroupM)x));

            List<PcM> pcList = new List<PcM>();
            tempPcList.ForEach(x => pcList.Add((PcM)x));

            pcList.ForEach(x =>  
            {
                x.GeneralInfo = (PcGeneralInfoM)db.PcGeneralInfoes.Find(x.id);
                List<PcDrive> temp = db.PcDrives.Where(z => z.PcID == x.id).ToList();  //TODO Async
                List<PcDriveM> trueTemp = new List<PcDriveM>();
                temp.ForEach(z => trueTemp.Add((PcDriveM)z));

                x.DriveList = trueTemp;
            });

            groupList.ForEach(x => 
            {
                List<PcM> temp = pcList.Where(z => z.GroupID == x.id).ToList();
                x.PcMs.AddRange(temp);
            });

            return groupList;
        }


        //Запрос всей обновляемой инфы по компу?
        // GET: api/Pcs/5
        [ResponseType(typeof(Pc))]
        public async Task<IHttpActionResult> GetPc(int id)
        {
            Pc pc = await db.Pcs.FindAsync(id);
            if (pc == null)
            {
                return NotFound();
            }

            PcLoadInfo pli = await db.PcLoadInfoes.FindAsync(id);
            if (pli == null)
            {
                //Создать элемент с мервтыми параметрами
            }

            List<PcDrive> pd = await db.PcDrives.Where(x => x.PcID == id).ToListAsync();
            if (pd == null)
            {
                //Обработать винты
            }

            PcGeneralInfo pgi = await db.PcGeneralInfoes.FindAsync(id);
            if(pgi == null)
            {
                //Ну тут тоже обработать
            }




            return Ok(pc);
        }

        // PUT: api/Pcs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPc(int id, Pc pc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pc.id)
            {
                return BadRequest();
            }

            db.Entry(pc).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PcExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Pcs
        [ResponseType(typeof(Pc))]
        public async Task<IHttpActionResult> PostPc(Pc pc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pcs.Add(pc);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PcExists(pc.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pc.id }, pc);
        }

        // DELETE: api/Pcs/5
        [ResponseType(typeof(Pc))]
        public async Task<IHttpActionResult> DeletePc(int id)
        {
            Pc pc = await db.Pcs.FindAsync(id);
            if (pc == null)
            {
                return NotFound();
            }

            db.Pcs.Remove(pc);
            await db.SaveChangesAsync();

            return Ok(pc);
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