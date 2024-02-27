using UnityEngine;

namespace Effects
{
    public class ArmSwayEffect : MonoBehaviour
    {
        [SerializeField] private float smooth = 8;

        private PlayerInput _input;
        private Vector2 HeadRotation => _input.Base.HeadRotation.ReadValue<Vector2>();

        private void Awake()
        {
            _input = new PlayerInput();
            _input.Enable();
        }

        private void Update()
        {

            float scaledSmooth =  smooth * Time.deltaTime;
            
            Quaternion targetRotation = Quaternion.AngleAxis(HeadRotation.x, Vector3.up);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, scaledSmooth);
        }
    }
}