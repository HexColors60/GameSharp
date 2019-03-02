using System;
using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Events;
using Dutil.Core.Exceptions;
using GameSharp.Core.Abstract;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;
using GameSharp.Core.Impl.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GameSharp.Core.Impl
{
    internal sealed class GameRoomPlayerServices : IGameRoomPlayerServices
    {
        private readonly GameSharpDbContext _db;
        private readonly IPlayerProvider _playerProvider;
        public event AsyncEventHandler<GameRoomPlayer> OnPlayerJoinedEvent = delegate
        {
            return Task.CompletedTask;
        };

        public GameRoomPlayerServices(GameSharpDbContext db,
            IPlayerProvider playerProvider)
        {
            _db = db;
            _playerProvider = playerProvider;
        }

        public async Task<GameRoomPlayer> AddPlayersAsync(GameRoom room,
            bool isPlayer,
            Player player,
            CancellationToken token = default(CancellationToken))
        {
            if (isPlayer && !room.IsAcceptingPlayers)
                throw new GameNotAcceptingMorePlayersException("The game is not accepting more players at the moment");

            var entity = new GameRoomPlayer
            {
                GameRoom = room,
                Player = player,
                IsPlayer = isPlayer
            };

            await _db.GameRoomPlayers.AddAsync(entity, token);
            return entity;
        }

        public async Task<GameRoomPlayer> AddAndSavePlayersAsync(int roomId, bool isPlayer, CancellationToken token = default(CancellationToken))
        {
            var player = await _playerProvider.GetCurrentPlayerAsync();
            if (player == null)
                throw new UnauthorizedAccessException();

            var room = await _db.GameRooms.Include(p => p.RoomPlayers)
                .SingleOrDefaultAsync(p => p.Id == roomId, token);

            if (room == null)
                throw new EntityNotFoundException("The room does not exists");

            var entity = await AddPlayersAsync(room, isPlayer, player, token);
            await _db.SaveChangesAsync(token);
            await OnPlayerJoinedEvent.Invoke(this, entity, token);
            return entity;
        }
    }
}