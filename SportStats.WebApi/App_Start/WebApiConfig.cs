using SportStats.WebApi.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;

namespace SportStats.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<Sport>("Sports");
            builder.EntitySet<Team>("Teams");
            builder.EntitySet<Player>("Players");
            builder.EntitySet<TeamInGame>("TeamInGames");
            builder.EntitySet<Tournament>("Tournaments");
            builder.EntitySet<Game>("Games");
            builder.EntitySet<StatType>("StatTypes");
            builder.EntitySet<StatValue>("StatValues");
            builder.EntitySet<City>("Cities");
            builder.EntitySet<Country>("Countries");

            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}
