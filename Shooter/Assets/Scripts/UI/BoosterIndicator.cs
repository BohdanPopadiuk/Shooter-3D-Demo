using System.Collections;
using Boosters;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BoosterIndicator : MonoBehaviour
    {

        [SerializeField] public BoosterType boosterType;
        [SerializeField] private Image timeBar;
        [SerializeField] private float showDuration;
        [SerializeField] private Ease showEase = Ease.InOutCubic;
        
        private float _duration;
        private float _maxDuration;
        private bool _freeIndicator = true;
        private Vector3 _defaultScale;
        
        public BoosterType GetBoosterType => boosterType;
        private float DurationDelta => _duration / _maxDuration;

        private void Awake()
        {
            _defaultScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void Run(float duration)
        {
            _maxDuration = duration;
            _duration = duration;
            
            if(_freeIndicator)
            {
                StartCoroutine(RunIndicator());
            }
        }

        private IEnumerator RunIndicator()
        {
            _freeIndicator = false;
            
            transform
                .DOScale(_defaultScale, showDuration)
                .SetEase(showEase);

            while (_duration >= 0)
            {
                _duration -= Time.deltaTime;
                timeBar.fillAmount = DurationDelta;
                yield return null;
            }

            transform
                .DOScale(0, showDuration)
                .SetEase(showEase)
                .OnComplete(() =>
                {
                    _freeIndicator = true;
                });
        }
    }
}