using System;
using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Events;
using GameSharp.Core.Abstract;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Impl
{
    internal sealed class GameRoomServices : IGameRoomServices
    {
        private readonly GameSharpDbContext _db;
        private readonly IPlayerProvider _playerProvider;
        public event AsyncEventHandler<GameRoom> OnRoomCreatedEvent = delegate { return Task.CompletedTask; };

        public GameRoomServices(GameSharpDbContext db,
            IPlayerProvider playerProvider)
        {
            _db = db;
            _playerProvider = playerProvider;
        }

        public async Task<GameRoom> CreateAsync(CancellationToken token = default(CancellationToken))
        {
            var player = await _playerProvider.Challenge();
            var room = new GameRoom
            {
                IsAcceptingPlayers = true,
                GameIdentifier = Guid.NewGuid(),
                CreatedBy = player,
                CreatedOn = DateTime.Now,
            };
            room.RoomPlayers.Add(new GameRoomPlayer
            {
                GameRoom = room,
                Player = player,
                IsPlayer = true
            });
            await _db.GameRooms.AddAsync(room, token);

            await _db.SaveChangesAsync(token);
            await OnRoomCreatedEvent.Invoke(this, room, token);
            return room;
        }
    }
}