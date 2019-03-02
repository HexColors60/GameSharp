using System;
using Dutil.Core.Abstract;
using GameSharp.Core.Entities.Enums;

namespace GameSharp.Core.Entities
{
    [Serializable]
    public class GameData : WriteLinkedListEntity<PlayerData>,
        IEntity,
        IEnumeratorEntity<PlayerData>,
        ITransitable<GameState>
    {
        public int Id { get; set; }
        public GameRoom Room { get; set; }
        public GameState CurrentState { get; set; }
        public PlayerData CurrentEntity { get; private set; }

        public bool Next()
        {
            if (LinkedList.Count <= 0)
                return false;
            CurrentEntity = CurrentEntity == null ? LinkedList.First.Value : CurrentEntity.Next;
            return CurrentEntity != null;
        }
    }
}