using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Events;
using GameSharp.Core.Abstract;
using GameSharp.Core.DataAccess;
using GameSharp.Core.Entities;
using GameSharp.Core.Impl;

namespace GameSharp.Tests.Fakes
{
    internal class FakeGameDataServices : GameDataServices<GameData>
    {
        public FakeGameDataServices(IPlayerProvider playerProvider,
            GameSharpDbContext db,
            IGameConfigurationProvider configurationProvider,
            IGameDataFactory<GameData> factory) : base(playerProvider,
            db,
            configurationProvider,
            factory)
        {
        }

        public override event AsyncEventHandler<GameData> OnGameStartEvent = delegate { return Task.CompletedTask; };

        public override async Task<GameData> StartGameAsync(int roomId,
            bool acceptMorePlayer = false,
            CancellationToken token = default)
        {
            var game = await base.StartGameAsync(roomId, acceptMorePlayer, token);
            await Db.SaveChangesAsync(token);
            await OnGameStartEvent.Invoke(this, game, token);
            return game;
        }
    }
}