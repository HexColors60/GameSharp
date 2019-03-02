using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameSharp.Core.Abstract;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;
using GameSharp.Tests.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GameSharp.Tests.Fakes
{
    internal sealed class FakePlayerProvider : PlayerProviderBase, IFakePlayerProvider
    {
        private Player _currentPlayer;

        public FakePlayerProvider(GameSharpDbContext db)
            : base(db)
        {
        }

        public async Task<Player> Authenticate(Func<IQueryable<Player>, Task<Player>> action)
        {
            var player = (await action.Invoke(Db.Players));
            var exists = await Db.Players
                .FirstOrDefaultAsync(p => p.Id == player.Id);
            return _currentPlayer = exists ?? player;
        }

        public IEnumerable<Player> Players => Db.Players;

        public override async Task<Player> GetCurrentPlayerAsync() =>
            await Task.FromResult(_currentPlayer);
    }
}