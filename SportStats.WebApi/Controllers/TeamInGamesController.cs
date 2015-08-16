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
    builder.EntitySet<TeamInGame>("TeamInGames");
    builder.EntitySet<Game>("Games"); 
    builder.EntitySet<Team>("Teams"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TeamInGamesController : ODataController
    {
        private SportStatsContext db = new SportStatsContext();

        // GET: odata/TeamInGames
        [EnableQuery]
        public IQueryable<TeamInGame> GetTeamInGames()
        {
            return db.TeamInGames;
        }

        // GET: odata/TeamInGames(5)
        [EnableQuery]
        public SingleResult<TeamInGame> GetTeamInGame([FromODataUri] int key)
        {
            return SingleResult.Create(db.TeamInGames.Where(teamInGame => teamInGame.Id == key));
        }

        // PUT: odata/TeamInGames(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<TeamInGame> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TeamInGame teamInGame = await db.TeamInGames.FindAsync(key);
            if (teamInGame == null)
            {
                return NotFound();
            }

            patch.Put(teamInGame);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamInGameExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(teamInGame);
        }

        // POST: odata/TeamInGames
        public async Task<IHttpActionResult> Post(TeamInGame teamInGame)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TeamInGames.Add(teamInGame);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeamInGameExists(teamInGame.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(teamInGame);
        }

        // PATCH: odata/TeamInGames(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<TeamInGame> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TeamInGame teamInGame = await db.TeamInGames.FindAsync(key);
            if (teamInGame == null)
            {
                return NotFound();
            }

            patch.Patch(teamInGame);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamInGameExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(teamInGame);
        }

        // DELETE: odata/TeamInGames(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            TeamInGame teamInGame = await db.TeamInGames.FindAsync(key);
            if (teamInGame == null)
            {
                return NotFound();
            }

            db.TeamInGames.Remove(teamInGame);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/TeamInGames(5)/Game
        [EnableQuery]
        public SingleResult<Game> GetGame([FromODataUri] int key)
        {
            return SingleResult.Create(db.TeamInGames.Where(m => m.Id == key).Select(m => m.Game));
        }

        // GET: odata/TeamInGames(5)/Team
        [EnableQuery]
        public SingleResult<Team> GetTeam([FromODataUri] int key)
        {
            return SingleResult.Create(db.TeamInGames.Where(m => m.Id == key).Select(m => m.Team));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamInGameExists(int key)
        {
            return db.TeamInGames.Count(e => e.Id == key) > 0;
        }
    }
}
