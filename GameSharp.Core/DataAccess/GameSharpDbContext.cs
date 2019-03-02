using GameSharp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameSharp.Core.DataAccess
{
    public class GameSharpDbContext : DbContext
    {
        public GameSharpDbContext()
        {
        }

        public GameSharpDbContext(DbContextOptions options) :
            base(options)
        {
        }

        public DbSet<GameData> GameDatas { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<GameRoomPlayer> GameRoomPlayers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerData> PlayersData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Player>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<Player>()
                .HasIndex(p => p.Username)
                .IsUnique();
            modelBuilder
                .Entity<Player>()
                .Property(p => p.Username)
                .IsRequired();

            modelBuilder
                .Entity<GameRoom>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<GameRoom>()
                .HasOne(p => p.CreatedBy)
                .WithMany(p => p.CreatedRooms)
                .IsRequired();

            modelBuilder
                .Entity<GameRoom>()
                .HasOne(p => p.GameData)
                .WithOne(p => p.Room)
                .HasForeignKey<GameRoom>(r => r.GameDataId)
                .IsRequired(false);

            modelBuilder
                .Entity<GameRoomPlayer>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<GameRoomPlayer>()
                .HasOne(p => p.GameRoom)
                .WithMany(r => r.RoomPlayers)
                .HasForeignKey(p => p.RoomId)
                .IsRequired();
            modelBuilder
                .Entity<GameRoomPlayer>()
                .HasOne(p => p.Player)
                .WithMany(r => r.GameRoomPlayers)
                .HasForeignKey(r => r.PlayerId)
                .IsRequired();
            modelBuilder
                .Entity<GameRoomPlayer>()
                .HasIndex(p => new { p.PlayerId, p.RoomId })
                .IsUnique();

            modelBuilder
                .Entity<GameData>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<GameData>()
                .HasOne(p => p.FirstPlayer)
                .WithMany(r => r.FirstPlayers)
                .IsRequired();
            modelBuilder
                .Entity<GameData>()
                .HasOne(p => p.CurrentTurn)
                .WithMany(r => r.CurrentTurns)
                .IsRequired(false);
            modelBuilder
                .Entity<GameData>()
                .Ignore(p => p.Turns);

            modelBuilder
                .Entity<PlayerData>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder
                .Entity<PlayerData>()
                .HasOne(p => p.Player)
                .WithMany(r => r.PlayerTurns)
                .IsRequired();
        }
    }
}