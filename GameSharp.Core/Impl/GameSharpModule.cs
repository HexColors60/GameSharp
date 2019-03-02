using Autofac;
using Dutil.Core.Impl;
using GameSharp.Core.Abstract;
using GameSharp.Core.Entities.Enums;

namespace GameSharp.Core.Impl
{
    public sealed class GameSharpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GameRoomPlayerServices>().As<IGameRoomPlayerServices>();
            builder.RegisterType<GameRoomServices>().As<IGameRoomServices>();
            builder.RegisterType<GameConfigurationProvider>().As<IGameConfigurationProvider>();
            builder.RegisterType<DefautlGameDataFactory>().AsImplementedInterfaces();

            builder.Register(context => new StateMachine<GameState, GameTransitions>()
                    .AddTransition(GameState.NONE, GameState.PLAYING, GameTransitions.START_GAME)
                    .AddTransition(GameState.PLAYING, GameState.FINISHED, GameTransitions.FINISH_GAME)
                    .AddTransition(GameState.PLAYING, GameState.ABORTED, GameTransitions.ABORT_GAME))
                .AsImplementedInterfaces();
        }
    }
}