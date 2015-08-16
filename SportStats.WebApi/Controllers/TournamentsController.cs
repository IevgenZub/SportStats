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
    builder.EntitySet<Tournament>("Tournaments");
    builder.EntitySet<City>("Cities"); 
    builder.EntitySet<Game>("Games"); 
    builder.EntitySet<Sport>("Sports"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TournamentsController : ODataController
    {
        private SportStatsContext db = new SportStatsContext();

        // GET: odata/Tournaments
        [EnableQuery]
        public IQueryable<Tournament> GetTournaments()
        {
            return db.Tournaments;
        }

        // GET: odata/Tournaments(5)
        [EnableQuery]
        public SingleResult<Tournament> GetTournament([FromODataUri] int key)
        {
            return SingleResult.Create(db.Tournaments.Where(tournament => tournament.Id == key));
        }

        // PUT: odata/Tournaments(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Tournament> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tournament tournament = await db.Tournaments.FindAsync(key);
            if (tournament == null)
            {
                return NotFound();
            }

            patch.Put(tournament);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tournament);
        }

        // POST: odata/Tournaments
        public async Task<IHttpActionResult> Post(Tournament tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tournaments.Add(tournament);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TournamentExists(tournament.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(tournament);
        }

        // PATCH: odata/Tournaments(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Tournament> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Tournament tournament = await db.Tournaments.FindAsync(key);
            if (tournament == null)
            {
                return NotFound();
            }

            patch.Patch(tournament);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tournament);
        }

        // DELETE: odata/Tournaments(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Tournament tournament = await db.Tournaments.FindAsync(key);
            if (tournament == null)
            {
                return NotFound();
            }

            db.Tournaments.Remove(tournament);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Tournaments(5)/City
        [EnableQuery]
        public SingleResult<City> GetCity([FromODataUri] int key)
        {
            return SingleResult.Create(db.Tournaments.Where(m => m.Id == key).Select(m => m.City));
        }

        // GET: odata/Tournaments(5)/Games
        [EnableQuery]
        public IQueryable<Game> GetGames([FromODataUri] int key)
        {
            return db.Tournaments.Where(m => m.Id == key).SelectMany(m => m.Games);
        }

        // GET: odata/Tournaments(5)/Sport
        [EnableQuery]
        public SingleResult<Sport> GetSport([FromODataUri] int key)
        {
            return SingleResult.Create(db.Tournaments.Where(m => m.Id == key).Select(m => m.Sport));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TournamentExists(int key)
        {
            return db.Tournaments.Count(e => e.Id == key) > 0;
        }
    }
}
