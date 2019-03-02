using Dutil.Core.Abstract;
using System;

namespace GameSharp.Core.Entities
{
    [Serializable]
    public class GameRoomPlayer : EventArgs, IEntity
    {
        public Player Player { get; set; }
        public bool IsPlayer { get; set; }
        public GameRoom GameRoom { get; set; }
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int RoomId { get; set; }
    }
}