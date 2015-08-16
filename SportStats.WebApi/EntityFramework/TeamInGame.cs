namespace SportStats.WebApi.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeamInGame")]
    public partial class TeamInGame
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int TeamId { get; set; }

        public int GameId { get; set; }

        public bool? Visitor { get; set; }

        public bool? Winner { get; set; }

        public bool? Technical { get; set; }

        public virtual Game Game { get; set; }

        public virtual Team Team { get; set; }
    }
}
