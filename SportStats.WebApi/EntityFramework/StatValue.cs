namespace SportStats.WebApi.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatValue")]
    public partial class StatValue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int StatTypeId { get; set; }

        public int? Value { get; set; }

        public bool? Successful { get; set; }

        public string Comments { get; set; }

        public int PlayerId { get; set; }

        public int GameId { get; set; }

        public DateTime EventDate { get; set; }

        public virtual Game Game { get; set; }

        public virtual Player Player { get; set; }

        public virtual StatType StatType { get; set; }
    }
}
