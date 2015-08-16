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
using System.Web.OData;
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
    builder.EntitySet<StatValue>("StatValues");
    builder.EntitySet<Game>("Games"); 
    builder.EntitySet<Player>("Players"); 
    builder.EntitySet<StatType>("StatTypes"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StatValuesController : ODataController
    {
        private SportStatsContext db = new SportStatsContext();

        // GET: odata/StatValues
        [EnableQuery]
        public IQueryable<StatValue> GetStatValues()
        {
            return db.StatValues;
        }

        // GET: odata/StatValues(5)
        [EnableQuery]
        public SingleResult<StatValue> GetStatValue([FromODataUri] int key)
        {
            return SingleResult.Create(db.StatValues.Where(statValue => statValue.Id == key));
        }

        // PUT: odata/StatValues(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<StatValue> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StatValue statValue = await db.StatValues.FindAsync(key);
            if (statValue == null)
            {
                return NotFound();
            }

            patch.Put(statValue);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatValueExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(statValue);
        }

        // POST: odata/StatValues
        public async Task<IHttpActionResult> Post(StatValue statValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StatValues.Add(statValue);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StatValueExists(statValue.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(statValue);
        }

        // PATCH: odata/StatValues(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<StatValue> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StatValue statValue = await db.StatValues.FindAsync(key);
            if (statValue == null)
            {
                return NotFound();
            }

            patch.Patch(statValue);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatValueExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(statValue);
        }

        // DELETE: odata/StatValues(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            StatValue statValue = await db.StatValues.FindAsync(key);
            if (statValue == null)
            {
                return NotFound();
            }

            db.StatValues.Remove(statValue);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/StatValues(5)/Game
        [EnableQuery]
        public SingleResult<Game> GetGame([FromODataUri] int key)
        {
            return SingleResult.Create(db.StatValues.Where(m => m.Id == key).Select(m => m.Game));
        }

        // GET: odata/StatValues(5)/Player
        [EnableQuery]
        public SingleResult<Player> GetPlayer([FromODataUri] int key)
        {
            return SingleResult.Create(db.StatValues.Where(m => m.Id == key).Select(m => m.Player));
        }

        // GET: odata/StatValues(5)/StatType
        [EnableQuery]
        public SingleResult<StatType> GetStatType([FromODataUri] int key)
        {
            return SingleResult.Create(db.StatValues.Where(m => m.Id == key).Select(m => m.StatType));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StatValueExists(int key)
        {
            return db.StatValues.Count(e => e.Id == key) > 0;
        }
    }
}
