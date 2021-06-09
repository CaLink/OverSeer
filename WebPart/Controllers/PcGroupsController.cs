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
        [ResponseType(typeof(PcGroup))]
        public async Task<IHttpActionResult> PutPcGroup(int id, PcGroupM pcGroup)
        {

            /*
             *  Залваем существующую группу
             *  Вычисляем группу по IP
             *  Изменяем её параметры
             *  Возвращаем инфу, получилось ли изменить
             */

            

            PcGroup updatePc = db.PcGroups.Find(id);

            if (string.IsNullOrWhiteSpace(pcGroup.Name) || updatePc == null  )
            {
                return Ok(new PcGroupM());
            }

            updatePc.Name = pcGroup.Name;

            try
            {
                await db.SaveChangesAsync();
                return Ok((PcGroupM)updatePc);
            }
            catch (Exception e)
            {
                return Ok(new PcGroupM());

            }

        }

        // POST: api/PcGroups
        [ResponseType(typeof(PcGroupM))]
        public async Task<IHttpActionResult> PostPcGroup(PcGroupM pcGroup)
        {
            /*
             *  Получаем "новую группу" (хз про его имя)
             *  Заливаем её в бд
             *  Возвращаем новую гуппу с новым id
             *  Если не фартануло, вернем id-1
             *  
             */

            if (string.IsNullOrWhiteSpace(pcGroup.Name))
            {
                return Ok(new PcGroupM());
            }

            PcGroup addedPcGroup = new PcGroup() { Name = pcGroup.Name };
            db.PcGroups.Add(addedPcGroup);
            try
            {
                await db.SaveChangesAsync();
                return Ok((PcGroupM)addedPcGroup);
            }
            catch (Exception)
            {
                return Ok(new PcGroupM());
            }

        }

        // DELETE: api/PcGroups/5
        [ResponseType(typeof(PcGroupM))]
        public async Task<IHttpActionResult> DeletePcGroup(int id)
        {

            /*
             * 
             * Получаем id группы
             * Убераем из неё все компы
             * Удаляем сам комп
             * Возващаем резульат выполнения
             * 
             */


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