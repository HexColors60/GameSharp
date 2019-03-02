using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.Entities;
using GameSharp.Tests.Helpers;

namespace GameSharp.Tests.Abstract
{
    public interface IBackgroundHelper
    {
        Task<GameRoom> CreateRoomAsync(CancellationToken token = default);

        Task<GameRoomPlayer> PlayerJoinAsync(GameRoom room,
            string username = PlayerServiceSeedHelper.SecondPlayerUsername,
            CancellationToken token = default);

        Task<GameData> StartGameAsync(GameRoom room, CancellationToken token = default);
    }
}