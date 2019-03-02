using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Abstract;
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
        protected readonly IPlayerProvider _playerProvider;
        protected readonly GameSharpDbContext _db;
        protected readonly IGameConfigurationProvider _configurationProvider;
        protected readonly IStateMachineProvider<GameState, GameTransitions> _stateMachineProvider;
        protected readonly IPlayerTurnsService _playerTurnService;
        public abstract event AsyncEventHandler<TGame> OnGameStartEvent;

        public GameDataServices(IPlayerProvider playerProvider,
            GameSharpDbContext db,
            IGameConfigurationProvider configurationProvider,
            IStateMachineProvider<GameState, GameTransitions> stateMachineProvider,
            IPlayerTurnsService playerTurnService)
        {
            _playerProvider = playerProvider;
            _db = db;
            _configurationProvider = configurationProvider;
            _stateMachineProvider = stateMachineProvider;
            _playerTurnService = playerTurnService;
        }

        public virtual async Task<TGame> StartGameAsync(int roomId,
            bool acceptMorePlayer = false,
            CancellationToken token = default(CancellationToken))
        {
            var roomEntity = await Validate();

            roomEntity.IsAcceptingPlayers = acceptMorePlayer;

            var game = new TGame
            {
                Room = roomEntity
            };
            _stateMachineProvider.ChangeState(game, GameTransitions.START_GAME);
            await _playerTurnService.ChoosePlayers(game, token);

            if (game.CurrentState != GameState.PLAYING)
                throw new InvalidGameStateException();

            game.NextTurn();
            return game;

            async Task<GameRoom> Validate()
            {
                var player = await _playerProvider.GetCurrentPlayerAsync();
                if (player == null)
                    throw new UnauthorizedAccessException();

                var room = await _db.GameRooms
                    .Include(p => p.RoomPlayers)
                    .ThenInclude(rp => rp.Player)
                    .Include(r => r.GameData)
                    .SingleOrDefaultAsync(p => p.Id == roomId, token);

                if (room == null)
                    throw new EntityNotFoundException("The room do not exists");

                if (room.GameData != null)
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
