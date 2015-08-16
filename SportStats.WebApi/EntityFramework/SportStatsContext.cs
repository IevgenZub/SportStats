namespace SportStats.WebApi.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SportStatsContext : DbContext
    {
        public SportStatsContext()
            : base("name=SportStatsContext")
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<StatType> StatTypes { get; set; }
        public virtual DbSet<StatValue> StatValues { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamInGame> TeamInGames { get; set; }
        public virtual DbSet<Tournament> Tournaments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasMany(e => e.Teams)
                .WithRequired(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.Tournaments)
                .WithRequired(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Cities)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasMany(e => e.StatValues)
                .WithRequired(e => e.Game)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasMany(e => e.TeamInGames)
                .WithRequired(e => e.Game)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.StatValues)
                .WithRequired(e => e.Player)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sport>()
                .HasMany(e => e.Teams)
                .WithRequired(e => e.Sport)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sport>()
                .HasMany(e => e.Tournaments)
                .WithRequired(e => e.Sport)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StatType>()
                .HasMany(e => e.StatValues)
                .WithRequired(e => e.StatType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Players)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.TeamInGames)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tournament>()
                .HasMany(e => e.Games)
                .WithRequired(e => e.Tournament)
                .WillCascadeOnDelete(false);
        }
    }
}
