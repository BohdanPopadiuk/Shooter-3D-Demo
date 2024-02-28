using System;
using System.Collections;
using Boosters;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IBoosterClient
    {
        [SerializeField] private float moveSpeed = 3;
        [SerializeField] private float moveAccelerator = 3f;
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private float maxSlopeAngle = 30;
    
        [Header("GroundCheck")]
        [SerializeField] private float groundCheckRadius = .2f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundMask;

        [Header("Head")]
        [SerializeField, Range(0, 1)] private float headSensitivity = 1;
        [SerializeField] private float minHeadAngle = -90;
        [SerializeField] private float maxHeadAngle = 90;
        [SerializeField] private Transform head;

        private PlayerInput _input;
    
        private Rigidbody _rigidbody;
        private Vector3 _headAngle;
        private Vector2 _headInputOffset;
        private RaycastHit _slopeHit;
        private bool _lockGroundCheck;

        public BoosterType BoosterType { get; } = BoosterType.MovementBooster;
        private Vector2 HeadRotationInput => _input.Base.HeadRotation.ReadValue<Vector2>();
        public Vector2 MoveInput => _input.Base.Move.ReadValue<Vector2>();
        public float Booster { get; set; } = 1;
        private float CalculatedMoveAccelerator => (IsAcceleratedMove ? moveAccelerator : 1) * Booster;
        private float CalculatedSpeed => moveSpeed * CalculatedMoveAccelerator;
        private float CalculatedJumpForce => jumpForce * Booster;
        public bool IsGround => !_lockGroundCheck &&
                                Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        public bool IsAcceleratedMove { get; private set; }
        

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _input = new PlayerInput();
            _input.Base.Jump.performed += _ => Jump();
        }

        private void Start()
        {
            _headInputOffset = HeadRotationInput;
        }

        private void OnEnable()
        {
            _input.Enable();
            _headInputOffset = HeadRotationInput;
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            HeadRotation();
            SwitchAccelerationMove();
        }

        private void FixedUpdate()
        {
            Move();
        }
    
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }

        private void Move()
        {
            Vector3 calculatedVelocity = CalculateMoveDirection() * CalculatedSpeed;
            calculatedVelocity.y = _rigidbody.velocity.y;
        
            _rigidbody.velocity = calculatedVelocity;
        }
    
        private void Jump()
        {
            if(!IsGround) return;

            StartCoroutine(BlockGroundCheck());
            Vector3 force = Vector3.up * CalculatedJumpForce;
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private void HeadRotation()
        {
            _headAngle.x -= HeadRotationInput.y + _headInputOffset.y;
            _headAngle.x = Mathf.Clamp(_headAngle.x, minHeadAngle, maxHeadAngle);
            _headAngle.y += HeadRotationInput.x + _headInputOffset.x;

            Quaternion targetRotation = Quaternion.Euler(_headAngle.x, _headAngle.y, 0);
            head.localRotation = Quaternion.Slerp(head.localRotation, targetRotation, headSensitivity);
        }

        private void SwitchAccelerationMove()
        {
            IsAcceleratedMove = (_input.Base.Run.ReadValue<float>() > 0.1f);
        }
        
        private Vector3 CalculateMoveDirection()
        {
            Vector3 direction = new Vector3(MoveInput.x, 0, MoveInput.y);
            direction = head.TransformDirection(direction);
            
            if(OnSlope())
            {
                direction = Vector3.ProjectOnPlane(direction, _slopeHit.normal);
            }
            
            return direction.normalized;
        }

        private bool OnSlope()
        {
            if(Physics.Raycast(groundCheck.position, Vector3.down, out _slopeHit, groundCheckRadius))
            {
                float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }
            return false;
        }

        private IEnumerator BlockGroundCheck()
        {
            _lockGroundCheck = true;
            yield return new WaitForSeconds(.3f);
            _lockGroundCheck = false;
        }
    }
}
