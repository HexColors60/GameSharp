using GameSharp.Core.Abstract;

namespace GameSharp.Core.Impl
{
    internal class GameConfigurationProvider : IGameConfigurationProvider
    {
        public int MinRoomPlayers() => 2;
    }
}