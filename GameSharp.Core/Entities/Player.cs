using System;
using System.Collections.Generic;
using Dutil.Core.Abstract;

namespace GameSharp.Core.Entities
{
    [Serializable]
    public class Player : IEntity
    {
        public string Username { get; set; }
        public ICollection<GameRoomPlayer> GameRoomPlayers { get; } = new HashSet<GameRoomPlayer>();
        public ICollection<PlayerData> PlayerTurns { get; } = new HashSet<PlayerData>();
        public int Id { get; set; }
        public ICollection<GameRoom> CreatedRooms { get; } = new HashSet<GameRoom>();
    }
}