using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.Entities;
using GameSharp.Tests.Helpers;

namespace GameSharp.Tests.Abstract
{
    public interface IPlayerServiceSeedHelper
    {
        Task<Player> SeedAndLoginAsync(string username = PlayerServiceSeedHelper.FirstPlayerUsername,
            CancellationToken token = default);
        Task<Player> LoginPlayerAsync(string username = PlayerServiceSeedHelper.FirstPlayerUsername,
            CancellationToken token = default);
    }
}