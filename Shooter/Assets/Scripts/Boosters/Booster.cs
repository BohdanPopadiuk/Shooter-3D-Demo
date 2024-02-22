using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Boosters
{
    public abstract class Booster: MonoBehaviour
    {
        [SerializeField, Min(1)] protected float booster = 1.5f;
        [SerializeField, Min(0)] protected float boosterDuration = 5f;
    
        [Header("Anim")]
        [SerializeField, Min(0)] private float hideAnimDuration = .5f;
        [SerializeField, Min(0)] private float scaleLoopDuration = 1;
        [SerializeField, Min(0)] private float rotationLoopDuration = 1;
    
        [Space(10)]
        [SerializeField, Min(1)] private float idleAnimScaleDelta = 1.5f;
    
        [Space(10)]
        [SerializeField] private Ease scaleAnimEase = Ease.InOutBack; 
        [SerializeField] private Ease hideAnimEase = Ease.InOutCubic;

        private readonly List<Tween> _idleAnims = new List<Tween>();

        private bool _boosterUsed;

        private void Start()
        {
            IdleAnimation();
        }
    
        private void OnValidate()
        {
            if(boosterDuration < hideAnimDuration)
                Debug.LogWarning("'boosterDuration' must be greater than 'hideAnimDuration'");
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_boosterUsed) return;
        
            if (other.CompareTag("Player"))
            {
                _boosterUsed = true;
            
                ActivateBooster();
                HideAnimation();
            }
        }

        protected virtual void ActivateBooster()
        {
            
        } 
    
        private void IdleAnimation()
        {
            Vector3 targetScale = transform.localScale * idleAnimScaleDelta;
            Vector3 targetRotation = new Vector3(0, 360, 0);

            //scale
            _idleAnims.Add(
                transform
                    .DOScale(targetScale, scaleLoopDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(scaleAnimEase)
            );
        
            //rotation
            _idleAnims.Add(
                transform
                    .DORotate(targetRotation, rotationLoopDuration, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear)
            );
        }

        private void HideAnimation()
        {
            foreach (Tween tween in _idleAnims)
                tween.Kill();

            transform.DOScale(Vector3.zero, hideAnimDuration)
                .SetEase(hideAnimEase)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }
    }
}