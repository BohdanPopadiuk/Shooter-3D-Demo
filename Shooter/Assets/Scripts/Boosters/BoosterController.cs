﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Boosters
{
    public class BoosterController: MonoBehaviour
    {
        public static Action<BoosterController> NewBoosterActivated;

        [SerializeField] private BoosterType boosterType;
        [field: SerializeField, Min(0)] public float Booster { get; private set; } = 1.5f;
        [field: SerializeField, Min(0)] public float BoosterDuration { get; private set; } = 5;
    
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
        
        private IBoosterClient[] _clients;
        private Coroutine _disableBoosterRoutine;
        private const float DisabledBooster = 1;
        private bool _boosterUsed;

        public BoosterType GetBoosterType => boosterType;

        private void Start()
        {
            IdleAnimation();
        }

        private void OnEnable()
        {
            NewBoosterActivated += ContinueBooster;
        }

        private void OnDisable()
        {
            NewBoosterActivated -= ContinueBooster;
        }

        private void OnValidate()
        {
            if(BoosterDuration < hideAnimDuration)
                Debug.LogWarning("'boosterDuration' must be greater than 'hideAnimDuration'");
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_boosterUsed) return;
        
            if (other.CompareTag("Player"))
            {
                _boosterUsed = true;
                NewBoosterActivated?.Invoke(this);
                
                _clients = other.GetComponentsInChildren<IBoosterClient>();
                SetBoosters(Booster);
                
                _disableBoosterRoutine = StartCoroutine(DisableBoosterWithDelay());
                HideAnimation();
            }
        }

        private void SetBoosters(float value)
        {
            foreach (IBoosterClient client in _clients)
            {
                if (client.BoosterType == boosterType)
                    client.Booster = value;
            }
        }

        private IEnumerator DisableBoosterWithDelay()
        {
            yield return new WaitForSeconds(BoosterDuration);
        
            _disableBoosterRoutine = null;
            SetBoosters(DisabledBooster);
            gameObject.SetActive(false);
        }

        private void ContinueBooster(BoosterController boosterController)
        {
            if(boosterType != boosterController.boosterType) return;
            if(!_boosterUsed) return;

            if (_disableBoosterRoutine != null)
            {
                StopCoroutine(_disableBoosterRoutine);
                gameObject.SetActive(false);
            }
        }
        

        #region Animations

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

            transform
                .DOScale(Vector3.zero, hideAnimDuration)
                .SetEase(hideAnimEase);
        }

        #endregion
    }
}