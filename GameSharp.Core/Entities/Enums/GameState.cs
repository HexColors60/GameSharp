using System;

namespace GameSharp.Core.Entities.Enums
{
    [Serializable]
    public enum GameState : byte
    {
        NONE,
        PLAYING,
        FINISHED,
        ABORTED
    }
}