using System;
using Player;
using UnityEngine;

namespace Effects
{
    public class ArmBobbingEffect: MonoBehaviour
    {
        [SerializeField] private BobbingParameters[] animationParametersArray;
        
        [SerializeField] private float effectIntensity = 0.02f;
        [SerializeField] private float effectIntensityX = 1;
        [SerializeField] private float effectSpeed = 10;

        [SerializeField, Range(0, 1)] private float armPivotMoveDuration = 1;
        [SerializeField] private Transform armPivot;
        
        [SerializeField] private Vector3 offset;
        
        private BobbingParameters _currentAnimationParameters;
        private Vector3 _originalOffset;
        private float _sinTime;

        private void Start()
        {
            _originalOffset = offset;
        }
    
    
        private void OnEnable()
        {
            PlayerStateManager.NewMoveState += SetCurrentMoveState;
        }

        private void OnDisable()
        {
            PlayerStateManager.NewMoveState += SetCurrentMoveState;
        }

        private void Update()
        {
            Bobbing();
        }

        private void SetCurrentMoveState(MoveState moveState)
        {
            foreach (BobbingParameters parameters in animationParametersArray)
            {
                if (parameters.moveState == moveState)
                {
                    _currentAnimationParameters = parameters;
                    break;
                }
            }
        }
        
        private void Bobbing()
        {
            Vector3 pivotTargetPos = _currentAnimationParameters.armsPivotOffset;
            armPivot.localPosition = Vector3.Slerp(armPivot.localPosition, pivotTargetPos, armPivotMoveDuration);
            
            Vector3 targetPos = transform.parent.position;
            transform.position = targetPos + offset;
            
            if (!_currentAnimationParameters.playBobbingAnimation)
            {
                _sinTime = 0;
                return;
            }

            float multiplier = _currentAnimationParameters.bobbingEffectMultiplier;
            
            _sinTime += effectSpeed * multiplier * Time.deltaTime;

            float sinAmountY = -Math.Abs(effectIntensity * MathF.Sin(_sinTime));
            Vector3 sinAmountX = transform.right * (effectIntensity * MathF.Cos(_sinTime) * effectIntensityX);

            offset = _originalOffset + (Vector3.up * sinAmountY);

            offset += sinAmountX;
        }
    }
}