using System;
using Player;

namespace Effects
{
    [Serializable]
    public struct BobbingParameters
    {
        public MoveState moveState;
        public float bobbingEffectMultiplier;
        public bool playBobbingAnimation;
    }
}