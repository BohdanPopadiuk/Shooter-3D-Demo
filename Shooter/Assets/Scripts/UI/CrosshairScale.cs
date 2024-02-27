using System;
using Player;
using UnityEngine;

namespace UI
{
    [Serializable]
    public struct CrosshairScale
    {
        [Min(0)] public float scale;
        public MoveState moveState;
    }
}