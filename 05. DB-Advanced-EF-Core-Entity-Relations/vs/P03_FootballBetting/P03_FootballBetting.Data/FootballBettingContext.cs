namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data.Config;
    using P03_FootballBetting.Data.Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {

        }
        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder
                    .UseSqlServer(DataSettings.ConnectionString);
            }
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder entity)
        {
            entity
                .Entity<Team>(team =>
                {
                    team
                    .HasKey(k => k.TeamId);

                    team
                    .Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsRequired(false)
                    .IsUnicode(false);

                    team
                    .Property(p => p.Initials)
                    .HasMaxLength(3)
                    .IsRequired(true)
                    .IsUnicode(true);

                    team
                    .Property(p => p.LogoUrl)
                    .HasMaxLength(250)
                    .IsRequired(true)
                    .IsUnicode(false);

                    team
                    .Property(p => p.Budget)
                    .IsRequired();


                    team
                    .HasOne(t => t.PrimaryKitColor)
                    .WithMany(color => color.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId);

                    team
                     .HasOne(t => t.SecondaryKitColor)
                     .WithMany(color => color.SecondaryKitTeams)
                     .HasForeignKey(t => t.SecondaryKitColor);

                    team
                    .HasOne(town => town.Town)
                    .WithMany(t => t.Teams)
                    .HasForeignKey(town => town.TownId);

                });

            entity
                .Entity<Color>(color =>
                {
                    color
                    .HasKey(k => k.ColorId);

                    color
                    .Property(p => p.Name)
                    .HasMaxLength(30)
                    .IsRequired(true)
                    .IsUnicode(true);

                });

            entity
                .Entity<Town>(town =>
                {
                    town.HasKey(k => k.TownId);

                    town
                    .Property(p => p.Name)
                    .HasMaxLength(50)
                    .IsRequired(true)
                    .IsUnicode(true);

                    town
                    .HasOne(t => t.Country)
                    .WithMany(c => c.Towns)
                    .HasForeignKey(t => t.CountryId);
                });

            entity
                .Entity<Country>(country =>
                {
                    country
                    .HasKey(c => c.CountryId);

                    country
                    .Property(c => c.Name)
                    .HasMaxLength(50)
                    .IsRequired(true)
                    .IsUnicode(true);
                });

            entity
                .Entity<Player>(player =>
                {
                    player.HasKey(p => p.PlayerId);

                    player
                    .Property(p => p.Name)
                    .HasMaxLength(30)
                    .IsRequired(true)
                    .IsUnicode(true);
                });
        }
    }
}
