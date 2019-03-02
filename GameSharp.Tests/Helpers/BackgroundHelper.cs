using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.Abstract;
using GameSharp.Core.Entities;
using GameSharp.Tests.Abstract;

namespace GameSharp.Tests.Helpers
{
    internal class BackgroundHelper : IBackgroundHelper
    {
        private readonly IGameDataServices<GameData> _gameService;
        private readonly IPlayerServiceSeedHelper _playerSeedHelper;
        private readonly IGameRoomPlayerServices _roomPlayerService;
        private readonly IGameRoomServices _roomService;

        public BackgroundHelper(IGameDataServices<GameData> gameService,
            IGameRoomPlayerServices roomPlayerService,
            IPlayerServiceSeedHelper playerSeedHelper,
            IGameRoomServices roomService)
        {
            _gameService = gameService;
            _roomPlayerService = roomPlayerService;
            _playerSeedHelper = playerSeedHelper;
            _roomService = roomService;
        }

        public async Task<GameRoom> CreateRoomAsync(CancellationToken token = default)
        {
            await _playerSeedHelper.SeedAndLoginAsync(token: token);
            return await _roomService.CreateAsync(token);
        }

        public async Task<GameRoomPlayer> PlayerJoinAsync(GameRoom room,
            string username = PlayerServiceSeedHelper.SecondPlayerUsername,
            CancellationToken token = default)
        {
            await _playerSeedHelper.SeedAndLoginAsync(username, token);
            return await _roomPlayerService.AddAndSavePlayersAsync(room.Id, true, token);
        }

        public async Task<GameData> StartGameAsync(GameRoom room, CancellationToken token = default)
        {
            await _playerSeedHelper.LoginPlayerAsync(token: token);
            return await _gameService.StartGameAsync(room.Id, false, token);
        }
    }
}