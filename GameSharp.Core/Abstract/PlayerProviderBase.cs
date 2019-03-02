using System;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Abstract
{
    public abstract class PlayerProviderBase : IPlayerProvider
    {
        protected readonly GameSharpDbContext Db;

        protected PlayerProviderBase(GameSharpDbContext db)
        {
            Db = db;
        }

        public async Task<Player> AddAsync(CancellationToken token = default(CancellationToken))
        {
            var player = await GetCurrentPlayerAsync();
            await Db.Players.AddAsync(player, token);
            await Db.SaveChangesAsync(token);
            return player;
        }

        public async Task<Player> Challenge()
        {
            var player = await GetCurrentPlayerAsync();
            if (player == null)
                throw new UnauthorizedAccessException();
            return player;
        }

        public abstract Task<Player> GetCurrentPlayerAsync();
    }
}