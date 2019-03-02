using System;
using System.Collections.Generic;
using Dutil.Core.Abstract;

namespace GameSharp.Core.Entities
{
    [Serializable]
    public class PlayerData : IEntity
    {
        public Player Player { get; set; }
        public PlayerData Next { get; set; }

        public ICollection<GameData> FirstPlayers { get; } = new HashSet<GameData>();
        public ICollection<GameData> CurrentTurns { get; } = new HashSet<GameData>();
        public int Id { get; set; }
    }
}