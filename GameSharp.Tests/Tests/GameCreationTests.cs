using System.Linq;
using System.Threading.Tasks;
using GameSharp.Tests.Abstract;
using GameSharp.Tests.Helpers;
using Shouldly;
using Xunit;
using Xunit.Ioc.Autofac;

namespace GameSharp.Tests.Tests
{
    [UseAutofacTestFramework]
    public class GameCreationTests
    {
        public GameCreationTests()
        {
        }

        public GameCreationTests(IBackgroundHelper backgroundHelper)
        {
            _backgroundHelper = backgroundHelper;
        }

        private readonly IBackgroundHelper _backgroundHelper;

        [Fact]
        public async Task When_create_game_room_should_accept_player()
        {
            //When
            var room = (await _backgroundHelper.CreateRoomAsync());

            //Then
            room.IsAcceptingPlayers.ShouldBeTrue();
        }

        [Fact]
        public async Task When_create_game_it_should_only_have_player_test()
        {
            //When
            var player = (await _backgroundHelper.CreateRoomAsync())
                .RoomPlayers
                .Select(r => r.Player)
                .SingleOrDefault();
            //Then
            player.ShouldNotBeNull();
            player.Username.ShouldBe(PlayerServiceSeedHelper.FirstPlayerUsername);
        }

        [Fact]
        public async Task When_create_game_its_first_player_should_be_a_player()
        {
            //When
            var player = (await _backgroundHelper.CreateRoomAsync())
                .RoomPlayers
                .SingleOrDefault();

            //Then
            player.ShouldNotBeNull();
            player.IsPlayer.ShouldBeTrue();
        }
    }
}