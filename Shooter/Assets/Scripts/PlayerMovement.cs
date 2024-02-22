using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public static Action<float, float> UpdateBooster;//booster && boosterDuration

    #region Variables

    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float moveAccelerator = 3f;
    [SerializeField] private float jumpForce = 5;
    
    [Header("GroundCheck")]
    [SerializeField] private float groundCheckRadius = .2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    [Header("Head")]
    [SerializeField, Range(0, 1)] private float headSensitivity = 1;
    [SerializeField] private float minHeadAngle = -90;
    [SerializeField] private float maxHeadAngle = 90;
    [SerializeField] private Transform head;
    
    private Coroutine _disableBoosterRoutine;
    private PlayerInput _input;
    private Rigidbody _rigidbody;
    private Vector3 _headAngle;
    private Vector2 _moveInput;
    private Vector2 _headInputOffset;
    private bool _isAcceleratedMove;
    private float _booster = 1;

    #endregion

    #region Properties

    private Vector2 HeadRotationInput => _input.Base.HeadRotation.ReadValue<Vector2>();
    private float CalculatedMoveAccelerator => (_isAcceleratedMove ? moveAccelerator : 1) * _booster;
    private float CalculatedSpeed => moveSpeed * CalculatedMoveAccelerator;
    private float CalculatedJumpForce => jumpForce * _booster;
    private float DisabledBooster => 1;
    private bool IsGround => Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

    #endregion

    #region Monobehaviour methods

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        //input
        _input = new PlayerInput();
        _input.Base.Jump.performed += _ => Jump();
        _input.Base.Run.performed += _ => SwitchAccelerationMove();
    }

    private void OnEnable()
    {
        UpdateBooster += SetBooster;
        
        _input.Enable();
        _headInputOffset = HeadRotationInput;
    }

    private void OnDisable()
    {
        UpdateBooster -= SetBooster;
        
        _input.Disable();
    }

    private void Update()
    {
        //input
        _moveInput = _input.Base.Move.ReadValue<Vector2>();

        HeadRotation();
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

    #endregion

    #region Private methods

    private void Move()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);
        direction = head.TransformDirection(direction);
        direction = direction.normalized;
        
        Vector3 calculatedVelocity = direction * CalculatedSpeed;
        calculatedVelocity.y = _rigidbody.velocity.y;
        
        _rigidbody.velocity = calculatedVelocity;
    }
    
    private void Jump()
    {
        if(!IsGround) return;

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
        _isAcceleratedMove = !_isAcceleratedMove;
    }

    private void SetBooster(float booster, float duration)
    {
        _booster = booster;
        
        if (_disableBoosterRoutine != null)
        {
            StopCoroutine(_disableBoosterRoutine);
        }
        _disableBoosterRoutine = StartCoroutine(DisableBoosterWithDelay(duration));
    }

    #endregion

    #region Coroutines

    private IEnumerator DisableBoosterWithDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        _disableBoosterRoutine = null;
        _booster = DisabledBooster;
    }

    #endregion
}
