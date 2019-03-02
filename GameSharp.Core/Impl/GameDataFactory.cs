using System;
using System.Collections.Generic;
using System.Linq;
using Dutil.Core.Abstract;
using GameSharp.Core.Entities;
using GameSharp.Core.Entities.Enums;
using GameSharp.Core.Impl.Exceptions;

namespace GameSharp.Core.Impl
{
    public class GameDataFactory<TEntity> :
        IGameDataFactory<TEntity> where TEntity : GameData, new()
    {
        private readonly IStateMachineProvider<GameState, GameTransitions> _stateMachineProvider;
        private readonly IListRandomizer _randomizer;

        public GameDataFactory(IStateMachineProvider<GameState,
            GameTransitions> stateMachineProvider,
            IListRandomizer randomizer)
        {
            _stateMachineProvider = stateMachineProvider;
            _randomizer = randomizer;
        }

        public TEntity Create(GameRoom room)
        {
            var game = new TEntity
            {
                Room = room
            };
            _stateMachineProvider.ChangeState(game, GameTransitions.START_GAME);
            game.AddRange(ChooseTurnsRandomly());

            if (game.CurrentState != GameState.PLAYING)
                throw new InvalidGameStateException();

            if (!game.Next())
                throw new Exception("Unknown error");

            return game;

            IEnumerable<PlayerData> ChooseTurnsRandomly() =>
                _randomizer
                    .Generate(game.Room.RoomPlayers.Where(p => p.IsPlayer)
                        .Select(p => p.Player))
                    .Select(p => new PlayerData
                    {
                        Player = p
                    });
        }
    }
    internal class DefautlGameDataFactory :
        GameDataFactory<GameData>
    {
        public DefautlGameDataFactory(
            IStateMachineProvider<GameState, GameTransitions> stateMachineProvider,
            IListRandomizer randomizer) : base(stateMachineProvider,
            randomizer)
        {
        }
    }
}
