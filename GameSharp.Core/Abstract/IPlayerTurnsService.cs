using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Abstract
{
    public interface IPlayerTurnsService
    {
        Task<IEnumerable<PlayerData>> ChoosePlayers(GameData game, CancellationToken token = default(CancellationToken));
    }
}