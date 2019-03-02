using System;
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
    public class TurnsTests
    {
        public TurnsTests()
        {
        }

        public TurnsTests(IBackgroundHelper backgroundHelper)
        {
            _backgroundHelper = backgroundHelper;
        }

        private readonly IBackgroundHelper _backgroundHelper;

        [Fact]
        public async Task When_choosing_the_turns_should_not_be_repeated()
        {
            //TODO: Implement this test
            //Background or When
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room);
            var game = await _backgroundHelper.StartGameAsync(room);

            //Then
            //game.Turns
            //    .Select(p => p.Player.Username)
            //    .ShouldBeUnique();
        }

        [Fact]
        public async Task When_game_start_with_2_player_then_player_2_should_be_the_current_player()
        {
            //Background or When
            var room = await _backgroundHelper.CreateRoomAsync();
            await _backgroundHelper.PlayerJoinAsync(room);
            var game = await _backgroundHelper.StartGameAsync(room);

            //Then
            game.CurrentEntity
                .Player
                .Username
                .ShouldBe(PlayerServiceSeedHelper.SecondPlayerUsername);
        }
    }
}