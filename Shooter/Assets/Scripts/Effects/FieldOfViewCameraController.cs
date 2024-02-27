using Player;
using UnityEngine;

namespace Effects
{
    public class FieldOfViewCameraController : MonoBehaviour
    {
        [SerializeField, Range(1, 1.5f)] private float fieldOfViewMultiplier = 1.3f;
        [SerializeField, Range(1, 1.5f)] private float fieldOfViewBoostedMultiplier = 1.5f;
        [SerializeField, Min(0)] private float effectSpeed = 5f;
        
        private float _defaultFieldOfView;
        private float _multiplier = 1;
        private Camera _camera;

        private float TargetFieldOfView => _defaultFieldOfView * _multiplier;
        private float CalculatedDuration => effectSpeed * Time.deltaTime;

        void Start()
        {
            _camera = GetComponent<Camera>();
            _defaultFieldOfView = _camera.fieldOfView;
        }

        private void OnEnable()
        {
            PlayerStateManager.NewMoveState += SetMultiplier;
        }

        private void OnDisable()
        {
            PlayerStateManager.NewMoveState -= SetMultiplier;
        }

        private void Update()
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, TargetFieldOfView, CalculatedDuration);
        }

        private void SetMultiplier(MoveState moveState)
        {
            if (moveState == MoveState.BoostedRun)
            {
                _multiplier = fieldOfViewBoostedMultiplier;
                return;
            }
            if (moveState == MoveState.Run)
            {
                _multiplier = fieldOfViewMultiplier;
                return;;
            }
            _multiplier = 1;
        }
    }
}
