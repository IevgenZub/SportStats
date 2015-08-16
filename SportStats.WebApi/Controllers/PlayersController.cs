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
    builder.EntitySet<Player>("Players");
    builder.EntitySet<StatValue>("StatValues"); 
    builder.EntitySet<Team>("Teams"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PlayersController : ODataController
    {
        private SportStatsContext db = new SportStatsContext();

        // GET: odata/Players
        [EnableQuery]
        public IQueryable<Player> GetPlayers()
        {
            return db.Players;
        }

        // GET: odata/Players(5)
        [EnableQuery]
        public SingleResult<Player> GetPlayer([FromODataUri] int key)
        {
            return SingleResult.Create(db.Players.Where(player => player.Id == key));
        }

        // PUT: odata/Players(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Player> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Player player = await db.Players.FindAsync(key);
            if (player == null)
            {
                return NotFound();
            }

            patch.Put(player);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(player);
        }

        // POST: odata/Players
        public async Task<IHttpActionResult> Post(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Players.Add(player);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PlayerExists(player.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(player);
        }

        // PATCH: odata/Players(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Player> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Player player = await db.Players.FindAsync(key);
            if (player == null)
            {
                return NotFound();
            }

            patch.Patch(player);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(player);
        }

        // DELETE: odata/Players(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Player player = await db.Players.FindAsync(key);
            if (player == null)
            {
                return NotFound();
            }

            db.Players.Remove(player);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Players(5)/StatValues
        [EnableQuery]
        public IQueryable<StatValue> GetStatValues([FromODataUri] int key)
        {
            return db.Players.Where(m => m.Id == key).SelectMany(m => m.StatValues);
        }

        // GET: odata/Players(5)/Team
        [EnableQuery]
        public SingleResult<Team> GetTeam([FromODataUri] int key)
        {
            return SingleResult.Create(db.Players.Where(m => m.Id == key).Select(m => m.Team));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerExists(int key)
        {
            return db.Players.Count(e => e.Id == key) > 0;
        }
    }
}
