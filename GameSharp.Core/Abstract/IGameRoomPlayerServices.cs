using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Events;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Abstract
{
    public interface IGameRoomPlayerServices
    {
        Task<GameRoomPlayer> AddPlayersAsync(GameRoom room,
            bool isPlayer,
            Player player,
            CancellationToken token = default(CancellationToken));

        Task<GameRoomPlayer> AddAndSavePlayersAsync(int roomId,
            bool isPlayer,
            CancellationToken token = default(CancellationToken));

        event AsyncEventHandler<GameRoomPlayer> OnPlayerJoinedEvent;
    }
}