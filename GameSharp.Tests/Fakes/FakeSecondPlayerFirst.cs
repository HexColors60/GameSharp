using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.Abstract;
using GameSharp.Core.Entities;
using GameSharp.Tests.Abstract;

namespace GameSharp.Tests.Fakes
{
    internal sealed class FakeSecondPlayerFirst : IPlayerTurnsService
    {
        private readonly IFakePlayerProvider _playerProvider;

        public FakeSecondPlayerFirst(IFakePlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        public async Task<IEnumerable<PlayerData>> ChoosePlayers(GameData game, CancellationToken token = default(CancellationToken))
        {
            var playersInverted = _playerProvider.Players.ToList();
            playersInverted.Reverse();
            return playersInverted.Select(p => new PlayerData
            {
                Player = p
            });
        }
    }
}