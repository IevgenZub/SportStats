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
    builder.EntitySet<City>("Cities");
    builder.EntitySet<Country>("Countries"); 
    builder.EntitySet<Team>("Teams"); 
    builder.EntitySet<Tournament>("Tournaments"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CitiesController : ODataController
    {
        private SportStatsContext db = new SportStatsContext();

        // GET: odata/Cities
        [EnableQuery]
        public IQueryable<City> GetCities()
        {
            return db.Cities;
        }

        // GET: odata/Cities(5)
        [EnableQuery]
        public SingleResult<City> GetCity([FromODataUri] int key)
        {
            return SingleResult.Create(db.Cities.Where(city => city.Id == key));
        }

        // PUT: odata/Cities(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<City> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            City city = await db.Cities.FindAsync(key);
            if (city == null)
            {
                return NotFound();
            }

            patch.Put(city);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(city);
        }

        // POST: odata/Cities
        public async Task<IHttpActionResult> Post(City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cities.Add(city);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CityExists(city.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(city);
        }

        // PATCH: odata/Cities(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<City> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            City city = await db.Cities.FindAsync(key);
            if (city == null)
            {
                return NotFound();
            }

            patch.Patch(city);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(city);
        }

        // DELETE: odata/Cities(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            City city = await db.Cities.FindAsync(key);
            if (city == null)
            {
                return NotFound();
            }

            db.Cities.Remove(city);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Cities(5)/Country
        [EnableQuery]
        public SingleResult<Country> GetCountry([FromODataUri] int key)
        {
            return SingleResult.Create(db.Cities.Where(m => m.Id == key).Select(m => m.Country));
        }

        // GET: odata/Cities(5)/Teams
        [EnableQuery]
        public IQueryable<Team> GetTeams([FromODataUri] int key)
        {
            return db.Cities.Where(m => m.Id == key).SelectMany(m => m.Teams);
        }

        // GET: odata/Cities(5)/Tournaments
        [EnableQuery]
        public IQueryable<Tournament> GetTournaments([FromODataUri] int key)
        {
            return db.Cities.Where(m => m.Id == key).SelectMany(m => m.Tournaments);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityExists(int key)
        {
            return db.Cities.Count(e => e.Id == key) > 0;
        }
    }
}
