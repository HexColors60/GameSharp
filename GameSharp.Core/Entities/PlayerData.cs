using System;
using System.Collections.Generic;
using Dutil.Core.Abstract;

namespace GameSharp.Core.Entities
{
    [Serializable]
    public class PlayerData : IEntity,
        INode<PlayerData>
    {
        public Player Player { get; set; }
        public PlayerData Next { get; set; }

        public ICollection<GameData> CurrentTurns { get; } = new HashSet<GameData>();
        public int Id { get; set; }
    }
}