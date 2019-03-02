using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Abstract;
using GameSharp.Core.Abstract;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Impl
{
    internal sealed class PlayerTurnsService : IPlayerTurnsService
    {
        private readonly GameSharpDbContext _db;
        private readonly IListRandomizer _randomizer;

        public PlayerTurnsService(GameSharpDbContext db,
            IListRandomizer randomizer)
        {
            _db = db;
            _randomizer = randomizer;
        }

        public async Task<IEnumerable<PlayerData>> ChoosePlayers(GameData game,
            CancellationToken token = default(CancellationToken))
        {
            var randomTurns = ChooseTurnsRandomly().ToList();

            game.FirstPlayer = randomTurns.First();
            PlayerData lastTurn = null;
            randomTurns.ForEach(p =>
            {
                if (lastTurn != null)
                    lastTurn.Next = p;
                lastTurn = p;

            });
            await _db.PlayersData.AddRangeAsync(randomTurns, token);
            return randomTurns;

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
}