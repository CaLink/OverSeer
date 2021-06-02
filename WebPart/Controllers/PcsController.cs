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
        private Random rnd = new Random();

        //TODO А почему он не 
        // А тут список груп, компов и их статической инфы
        // GET: api/Pcs
        public List<PcGroupM> GetPcs()
        {
            List<PcGroup> tempPcGroupList = db.PcGroups.ToListAsync().Result;
            List<Pc> tempPcList = db.Pcs.ToListAsync().Result;
            List<PcDrive> driveList = db.PcDrives.ToListAsync().Result;

            List<PcGroupM> groupList = new List<PcGroupM>();
            tempPcGroupList.ForEach(x => groupList.Add((PcGroupM)x));

            List<PcM> pcList = new List<PcM>();
            tempPcList.ForEach(x => pcList.Add((PcM)x));

            groupList.ForEach(x =>
            {
                x.PcMs = new List<PcM>();

                tempPcList.Where(z => z.PcGroupID == x.id).ToList().ForEach(z =>
                {
                    PcM PMs = (PcM)z;
                    PMs.DriveList = new List<PcDriveM>();
                    driveList.Where(c => c.PcID == z.id).ToList().ForEach(c => PMs.DriveList.Add((PcDriveM)c));
                    PMs.GeneralInfo = db.PcGeneralInfoes.Find(z.id);

                    x.PcMs.Add(PMs);

                });

            });



            /*
            pcList.ForEach(x =>  
            {
                x.GeneralInfo = (PcGeneralInfoM)db.PcGeneralInfoes.Find(x.id);

                List<PcDriveM> trueDriveList = new List<PcDriveM>();
                trueDriveList = driveList.Where(z => z.PcID == x.id).Select(z=>(PcDriveM)z).ToList();

                x.DriveList = trueDriveList;
            });

            groupList.ForEach(x => 
            {
                List<PcM> temp = pcList.Where(z => z.GroupID == x.id).ToList();
                x.PcMs.AddRange(temp);
            });
            */

            return groupList;
        }


        //Запрос всей обновляемой инфы по компу?
        // GET: api/Pcs/5
        [ResponseType(typeof(PcLoadInfoM))]
        public async Task<IHttpActionResult> GetPc(int id)
        {

            PcLoadInfo pli = await db.PcLoadInfoes.FindAsync(id);
            if (pli == null)
            {
                return NotFound(); //IDK
            }

            PcLoadInfoM ret = (PcLoadInfoM)pli;


            return Ok(ret);
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
        [ResponseType(typeof(PcMA))]
        public async Task<IHttpActionResult> PostPc(PcMA pc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //TODO Это бы как-то проверить
            if (pc != null)
            {
                var temp = db.Pcs.Where(x => x.GUID == pc.GUID).ToList();
                if (temp.Count == 1)
                    return Ok(new PcMA
                    {
                        id = temp[0].id,
                        GUID = temp[0].GUID
                    }) ;

            }

            Pc sas = new Pc();

            sas.IP = "Woops";
            sas.Name = "Woops";
            sas.Port = -1;
            sas.PcGroupID = 1;
            //sas.GUID = Guid.NewGuid().ToString();

            do
            {
                sas.GUID = Guid.NewGuid().ToString();
            }
            while (db.Pcs.Where(x => x.GUID == sas.GUID).ToList().Count > 0);

           
            db.Pcs.Add(sas);
            try
            {
                await db.SaveChangesAsync();
                PcMA pici = new PcMA();
                pici.id = db.Pcs.Where(x=>x.GUID == sas.GUID).ToList()[0].id;
                pici.GUID = sas.GUID;
                return Ok(pici);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return Ok(new PcMA());
            }
            /*
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
           */
            //return CreatedAtRoute("DefaultApi", new { id = pc.id }, pc);
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