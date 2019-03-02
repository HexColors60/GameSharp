using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Events;
using Dutil.Core.Exceptions;
using GameSharp.Core.Abstract;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;
using GameSharp.Core.Entities.Enums;
using GameSharp.Core.Impl.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GameSharp.Core.Impl
{
    public abstract class GameDataServices<TGame> :
        IGameDataServices<TGame>
        where TGame : GameData, new()
    {
        private readonly IPlayerProvider _playerProvider;
        protected readonly GameSharpDbContext Db;
        private readonly IGameConfigurationProvider _configurationProvider;
        private readonly IGameDataFactory<TGame> _factory;
        public abstract event AsyncEventHandler<TGame> OnGameStartEvent;

        protected GameDataServices(IPlayerProvider playerProvider,
            GameSharpDbContext db,
            IGameConfigurationProvider configurationProvider,
            IGameDataFactory<TGame> factory)
        {
            _playerProvider = playerProvider;
            Db = db;
            _configurationProvider = configurationProvider;
            _factory = factory;
        }
        
        public virtual async Task<TGame> StartGameAsync(int roomId,
            bool acceptMorePlayer = false,
            CancellationToken token = default(CancellationToken))
        {
            var player = await _playerProvider.Challenge();
            var roomEntity = await ValidateEntity();

            roomEntity.IsAcceptingPlayers = acceptMorePlayer;
            return _factory.Create(roomEntity);

            async Task<GameRoom> ValidateEntity()
            {
                var room = await Db.GameRooms
                    .Include(p => p.RoomPlayers)
                    .ThenInclude(rp => rp.Player)
                    .Include(r => r.GameData)
                    .FirstOrDefaultAsync(p => p.Id == roomId, token);

                if (room == null)
                    throw new EntityNotFoundException("The room do not exists");

                if (room.GameData?.CurrentState > GameState.NONE)
                    throw new GameAlreadyStartedException();

                if (room.RoomPlayers.All(r => r.Player.Id != player.Id))
                    throw new UnauthorizedAccessException("Forbidden. You can't start a game in a room where you are not a player");

                if (room.RoomPlayers.Count(r => r.IsPlayer) < _configurationProvider.MinRoomPlayers())
                    throw new NotEnoughPlayerInGameSession();

                return room;
            }
        }
    }
}
