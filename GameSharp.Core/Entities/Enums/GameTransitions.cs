using System;

namespace GameSharp.Core.Entities.Enums
{
    [Serializable]
    public enum GameTransitions : byte
    {
        START_GAME,
        FINISH_GAME,
        ABORT_GAME
    }
}