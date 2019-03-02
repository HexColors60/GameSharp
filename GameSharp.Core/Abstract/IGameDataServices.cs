using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Events;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Abstract
{
    public interface IGameDataServices<TGame>
        where TGame : GameData, new()
    {
        Task<TGame> StartGameAsync(int roomId,
            bool acceptMorePlayer = false,
            CancellationToken token = default(CancellationToken));
        event AsyncEventHandler<TGame> OnGameStartEvent;
    }
}
