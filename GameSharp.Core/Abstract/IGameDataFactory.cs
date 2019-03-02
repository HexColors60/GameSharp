using GameSharp.Core.Entities;

namespace GameSharp.Core.Impl
{
    public interface IGameDataFactory<out TEntity>
    where TEntity : class, new()
    {
        TEntity Create(GameRoom room);
    }
}
