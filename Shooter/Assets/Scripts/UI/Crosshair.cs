using DG.Tweening;
using UnityEngine;
using Player;

namespace UI
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private CrosshairScale[] targetScales;
        [SerializeField, Min(0)] private float defaultScale = 30;
        
        [Header("Lines")]
        [SerializeField] private Transform topLine;
        [SerializeField] private Transform rightLine;
        [SerializeField] private Transform bottomLine;
        [SerializeField] private Transform leftLine;

        [Header("Anim")]
        [SerializeField] private float animDuration = .2f;
        [SerializeField] private Ease animEase = Ease.Linear;

        private Tween _tween;

        private void OnValidate()
        {
            topLine.localPosition = new Vector2(0, defaultScale);
            bottomLine.localPosition = new Vector2(0, -defaultScale);
            leftLine.localPosition = new Vector2(-defaultScale, 0);
            rightLine.localPosition = new Vector2(defaultScale, 0);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            PlayerStateManager.NewMoveState += ResizeCrosshair;
        }

        private void OnDisable()
        {
            PlayerStateManager.NewMoveState -= ResizeCrosshair;
        }

        private void ResizeCrosshair(MoveState moveState)
        {
            if(_tween != null)
                _tween.Kill();

            float targetPos = defaultScale;
            
            foreach (var targetScale in targetScales)
            {
                if (moveState == targetScale.moveState)
                {
                    targetPos = targetScale.scale;
                    break;
                }
            }

            _tween = DOTween.Sequence()
                .Join(topLine.DOLocalMoveY(targetPos, animDuration))
                .Join(bottomLine.DOLocalMoveY(-targetPos, animDuration))
                .Join(leftLine.DOLocalMoveX(-targetPos, animDuration))
                .Join(rightLine.DOLocalMoveX(targetPos, animDuration));

            _tween.SetEase(animEase);
        }
    }
}