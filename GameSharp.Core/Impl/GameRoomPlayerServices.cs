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
        public event AsyncEventHandler<GameRoomPlayer> OnPlayerJoinEvent = delegate
        {
            return Task.CompletedTask;
        };

        public GameRoomPlayerServices(GameSharpDbContext db,
            IPlayerProvider playerProvider)
        {
            _db = db;
            _playerProvider = playerProvider;
        }

        public async Task<GameRoomPlayer> JoinAsync(int roomId, bool isPlayer, CancellationToken token = default(CancellationToken))
        {
            var player = await _playerProvider.Challenge();

            var room = await _db.GameRooms.Include(p => p.RoomPlayers)
                .FirstOrDefaultAsync(p => p.Id == roomId, token);

            if (room == null)
                throw new EntityNotFoundException("The room does not exists");

            if (isPlayer && !room.IsAcceptingPlayers)
                throw new GameNotAcceptingMorePlayersException("The game is not accepting more players at the moment");

            var entity = new GameRoomPlayer
            {
                GameRoom = room,
                Player = player,
                IsPlayer = isPlayer
            };

            await _db.GameRoomPlayers.AddAsync(entity, token);
            await _db.SaveChangesAsync(token);
            await OnPlayerJoinEvent.Invoke(this, entity, token);
            return entity;
        }
    }
}