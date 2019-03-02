using System;
using System.Collections.Generic;
using Dutil.Core.Abstract;
using GameSharp.Core.Entities.Enums;

namespace GameSharp.Core.Entities
{
    [Serializable]
    public class GameData : EventArgs, IEntity, ITransitable<GameState>
    {
        public PlayerData FirstPlayer { get; set; }
        public PlayerData CurrentTurn { get; private set; }
        public int Id { get; set; }
        public GameState CurrentState { get; set; }
        public GameRoom Room { get; set; }

        public IEnumerable<PlayerData> Turns
        {
            get
            {
                var turn = FirstPlayer;
                while (turn != null)
                {
                    yield return turn;
                    turn = turn.Next;
                }
            }
        }

        public PlayerData NextTurn() =>
            CurrentTurn = CurrentTurn == null || CurrentTurn.Next == null ? FirstPlayer : CurrentTurn.Next;
    }
}