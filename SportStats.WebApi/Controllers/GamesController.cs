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
    builder.EntitySet<Game>("Games");
    builder.EntitySet<StatValue>("StatValues"); 
    builder.EntitySet<TeamInGame>("TeamInGames"); 
    builder.EntitySet<Tournament>("Tournaments"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class GamesController : ODataController
    {
        private SportStatsContext db = new SportStatsContext();

        // GET: odata/Games
        [EnableQuery]
        public IQueryable<Game> GetGames()
        {
            return db.Games;
        }

        // GET: odata/Games(5)
        [EnableQuery]
        public SingleResult<Game> GetGame([FromODataUri] int key)
        {
            return SingleResult.Create(db.Games.Where(game => game.Id == key));
        }

        // PUT: odata/Games(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Game> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Game game = await db.Games.FindAsync(key);
            if (game == null)
            {
                return NotFound();
            }

            patch.Put(game);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(game);
        }

        // POST: odata/Games
        public async Task<IHttpActionResult> Post(Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Games.Add(game);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GameExists(game.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(game);
        }

        // PATCH: odata/Games(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Game> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Game game = await db.Games.FindAsync(key);
            if (game == null)
            {
                return NotFound();
            }

            patch.Patch(game);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(game);
        }

        // DELETE: odata/Games(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Game game = await db.Games.FindAsync(key);
            if (game == null)
            {
                return NotFound();
            }

            db.Games.Remove(game);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Games(5)/StatValues
        [EnableQuery]
        public IQueryable<StatValue> GetStatValues([FromODataUri] int key)
        {
            return db.Games.Where(m => m.Id == key).SelectMany(m => m.StatValues);
        }

        // GET: odata/Games(5)/TeamInGames
        [EnableQuery]
        public IQueryable<TeamInGame> GetTeamInGames([FromODataUri] int key)
        {
            return db.Games.Where(m => m.Id == key).SelectMany(m => m.TeamInGames);
        }

        // GET: odata/Games(5)/Tournament
        [EnableQuery]
        public SingleResult<Tournament> GetTournament([FromODataUri] int key)
        {
            return SingleResult.Create(db.Games.Where(m => m.Id == key).Select(m => m.Tournament));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GameExists(int key)
        {
            return db.Games.Count(e => e.Id == key) > 0;
        }
    }
}
