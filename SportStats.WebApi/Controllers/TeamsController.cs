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
    builder.EntitySet<Team>("Teams");
    builder.EntitySet<City>("Cities"); 
    builder.EntitySet<Player>("Players"); 
    builder.EntitySet<Sport>("Sports"); 
    builder.EntitySet<TeamInGame>("TeamInGames"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TeamsController : ODataController
    {
        private SportStatsContext db = new SportStatsContext();

        // GET: odata/Teams
        [EnableQuery]
        public IQueryable<Team> GetTeams()
        {
            return db.Teams;
        }

        // GET: odata/Teams(5)
        [EnableQuery]
        public SingleResult<Team> GetTeam([FromODataUri] int key)
        {
            return SingleResult.Create(db.Teams.Where(team => team.Id == key));
        }

        // PUT: odata/Teams(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Team> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Team team = await db.Teams.FindAsync(key);
            if (team == null)
            {
                return NotFound();
            }

            patch.Put(team);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(team);
        }

        // POST: odata/Teams
        public async Task<IHttpActionResult> Post(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teams.Add(team);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeamExists(team.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(team);
        }

        // PATCH: odata/Teams(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Team> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Team team = await db.Teams.FindAsync(key);
            if (team == null)
            {
                return NotFound();
            }

            patch.Patch(team);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(team);
        }

        // DELETE: odata/Teams(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Team team = await db.Teams.FindAsync(key);
            if (team == null)
            {
                return NotFound();
            }

            db.Teams.Remove(team);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Teams(5)/City
        [EnableQuery]
        public SingleResult<City> GetCity([FromODataUri] int key)
        {
            return SingleResult.Create(db.Teams.Where(m => m.Id == key).Select(m => m.City));
        }

        // GET: odata/Teams(5)/Players
        [EnableQuery]
        public IQueryable<Player> GetPlayers([FromODataUri] int key)
        {
            return db.Teams.Where(m => m.Id == key).SelectMany(m => m.Players);
        }

        // GET: odata/Teams(5)/Sport
        [EnableQuery]
        public SingleResult<Sport> GetSport([FromODataUri] int key)
        {
            return SingleResult.Create(db.Teams.Where(m => m.Id == key).Select(m => m.Sport));
        }

        // GET: odata/Teams(5)/TeamInGames
        [EnableQuery]
        public IQueryable<TeamInGame> GetTeamInGames([FromODataUri] int key)
        {
            return db.Teams.Where(m => m.Id == key).SelectMany(m => m.TeamInGames);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeamExists(int key)
        {
            return db.Teams.Count(e => e.Id == key) > 0;
        }
    }
}
