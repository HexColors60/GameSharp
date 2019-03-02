using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Events;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Abstract
{
    public interface IGameRoomPlayerServices
    {
        Task<GameRoomPlayer> JoinAsync(int roomId,
            bool isPlayer,
            CancellationToken token = default(CancellationToken));

        event AsyncEventHandler<GameRoomPlayer> OnPlayerJoinEvent;
    }
}