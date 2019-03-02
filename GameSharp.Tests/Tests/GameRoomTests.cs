using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameSharp.Core.Abstract;
using GameSharp.Tests.Abstract;
using GameSharp.Tests.Helpers;
using Shouldly;
using Xunit;
using Xunit.Ioc.Autofac;

namespace GameSharp.Tests.Tests
{
    [UseAutofacTestFramework]
    public sealed class GameRoomTests
    {
        public GameRoomTests()
        {
        }

        public GameRoomTests(IBackgroundHelper backgroundHelper,
            IPlayerServiceSeedHelper playerHelperService,
            IGameRoomPlayerServices gameRoomPlayerServices)
        {
            _backgroundHelper = backgroundHelper;
            _playerHelperService = playerHelperService;
            _gameRoomPlayerServices = gameRoomPlayerServices;
        }

        private readonly IBackgroundHelper _backgroundHelper;
        private readonly IPlayerServiceSeedHelper _playerHelperService;
        private readonly IGameRoomPlayerServices _gameRoomPlayerServices;

        [Fact]
        public async Task When_a_player_join_as_viewer_then_two_player_should_exists_and_player_2_should_be_viewer()
        {
            //Background
            var room = await _backgroundHelper.CreateRoomAsync();
            await _playerHelperService.SeedAndLoginAsync(PlayerServiceSeedHelper.SecondPlayerUsername);

            //When
            var gameRoomPlayer = await _gameRoomPlayerServices
                .JoinAsync(room.Id, false, CancellationToken.None);

            //Then
            gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                p.Player.Username == PlayerServiceSeedHelper.FirstPlayerUsername).IsPlayer.ShouldBeTrue();
            gameRoomPlayer.GameRoom.RoomPlayers.Single(p =>
                p.Player.Username == PlayerServiceSeedHelper.SecondPlayerUsername).IsPlayer.ShouldBeFalse();
        }

        [Fact]
        public async Task When_a_second_player_joins_then_two_player_should_exists()
        {
            //Background
            var room = await _backgroundHelper.CreateRoomAsync();

            //When
            var roomPlayer = await _backgroundHelper.PlayerJoinAsync(room);

            //Then
            roomPlayer.GameRoom.RoomPlayers.Count.ShouldBe(2);
            roomPlayer.GameRoom
                .RoomPlayers.Select(p => p.IsPlayer)
                .ShouldAllBe(p => p);
        }
    }
}