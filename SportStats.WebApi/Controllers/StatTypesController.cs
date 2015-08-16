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
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using SportStats.WebApi.EntityFramework;

namespace SportStats.WebApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using SportStats.WebApi.EntityFramework;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<StatType>("StatTypes");
    builder.EntitySet<StatValue>("StatValues"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StatTypesController : ODataController
    {
        private SportStatsContext db = new SportStatsContext();

        // GET: odata/StatTypes
        [EnableQuery]
        public IQueryable<StatType> GetStatTypes()
        {
            return db.StatTypes;
        }

        // GET: odata/StatTypes(5)
        [EnableQuery]
        public SingleResult<StatType> GetStatType([FromODataUri] int key)
        {
            return SingleResult.Create(db.StatTypes.Where(statType => statType.Id == key));
        }

        // PUT: odata/StatTypes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<StatType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StatType statType = await db.StatTypes.FindAsync(key);
            if (statType == null)
            {
                return NotFound();
            }

            patch.Put(statType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(statType);
        }

        // POST: odata/StatTypes
        public async Task<IHttpActionResult> Post(StatType statType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StatTypes.Add(statType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StatTypeExists(statType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(statType);
        }

        // PATCH: odata/StatTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<StatType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StatType statType = await db.StatTypes.FindAsync(key);
            if (statType == null)
            {
                return NotFound();
            }

            patch.Patch(statType);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(statType);
        }

        // DELETE: odata/StatTypes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            StatType statType = await db.StatTypes.FindAsync(key);
            if (statType == null)
            {
                return NotFound();
            }

            db.StatTypes.Remove(statType);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/StatTypes(5)/StatValues
        [EnableQuery]
        public IQueryable<StatValue> GetStatValues([FromODataUri] int key)
        {
            return db.StatTypes.Where(m => m.Id == key).SelectMany(m => m.StatValues);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StatTypeExists(int key)
        {
            return db.StatTypes.Count(e => e.Id == key) > 0;
        }
    }
}
