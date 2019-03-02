using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Abstract
{
    public abstract class PlayerProviderBase : IPlayerProvider
    {
        protected readonly GameSharpDbContext _db;

        protected PlayerProviderBase(GameSharpDbContext db)
        {
            _db = db;
        }

        public async Task<Player> AddAsync(CancellationToken token = default(CancellationToken))
        {
            var player = await GetCurrentPlayerAsync();
            await _db.Players.AddAsync(player, token);
            await _db.SaveChangesAsync(token);
            return player;
        }

        public abstract Task<Player> GetCurrentPlayerAsync();
    }
}