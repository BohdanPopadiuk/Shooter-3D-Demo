using System;
using Player;
using UnityEngine;

namespace Effects
{
    [Serializable]
    public struct BobbingParameters
    {
        public MoveState moveState;
        public float bobbingEffectMultiplier;
        public bool playBobbingAnimation;
        public Vector3 armsPivotOffset;
    }
}