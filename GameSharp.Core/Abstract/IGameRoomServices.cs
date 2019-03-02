using System.Threading;
using System.Threading.Tasks;
using Dutil.Core.Events;
using GameSharp.Core.Entities;

namespace GameSharp.Core.Abstract
{
    public interface IGameRoomServices
    {
        Task<GameRoom> CreateAsync(CancellationToken token=default(CancellationToken));
        event AsyncEventHandler<GameRoom> OnRoomCreatedEvent;
    }
}
