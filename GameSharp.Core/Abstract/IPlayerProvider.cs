using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Abstract
{
    public interface IPlayerProvider
    {
        Task<Player> GetCurrentPlayerAsync();
        Task<Player> AddAsync(CancellationToken token = default(CancellationToken));
        Task<Player> Challenge();
    }
}