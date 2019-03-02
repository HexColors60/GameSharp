using System;
using System.Collections.Generic;
using Dutil.Core.Abstract;

namespace GameSharp.Core.Entities
{
    [Serializable]
    public class GameRoom : EventArgs, IEntity
    {
        public ICollection<GameRoomPlayer> RoomPlayers { get; } = new HashSet<GameRoomPlayer>();
        public int Id { get; set; }
        public bool IsAcceptingPlayers { get; set; }
        public Guid GameIdentifier { get; set; }
        public GameData GameData { get; set; }
        public Player CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? GameDataId { get; set; }
    }
}