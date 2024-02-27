using System;
using UnityEngine;
using Player;

namespace UI
{
    [Serializable]
    public struct PlayerStateIcon
    {
        public Sprite icon;
        public MoveState moveState;
    }
}