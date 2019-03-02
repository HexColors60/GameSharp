using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
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
        private readonly IGameRoomPlayerServices _roomPlayerServices;
        public event AsyncEventHandler<GameRoom> OnRoomCreatedEvent = delegate { return Task.CompletedTask; };

        public GameRoomServices(GameSharpDbContext db,
            IPlayerProvider playerProvider,
            IGameRoomPlayerServices roomPlayerServices)
        {
            _db = db;
            _playerProvider = playerProvider;
            _roomPlayerServices = roomPlayerServices;
        }

        public async Task<GameRoom> CreateAsync(CancellationToken token = default(CancellationToken))
        {
            var player = await _playerProvider.GetCurrentPlayerAsync();
            if (player == null)
                throw new UnauthorizedAccessException();

            var room = new GameRoom
            {
                IsAcceptingPlayers = true,
                GameIdentifier = Guid.NewGuid(),
                CreatedBy = player
            };
            await _db.GameRooms.AddAsync(room, token);
            await _roomPlayerServices.AddPlayersAsync(room, true, player, token);

            await _db.SaveChangesAsync(token);
            await OnRoomCreatedEvent.Invoke(this, room, token);
            return room;
        }
    }
}