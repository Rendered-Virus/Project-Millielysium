using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Sirenix.OdinInspector;
using Cursor = UnityEngine.Cursor;

// ReSharper disable All

public class PlayerMovement : MonoBehaviour
{
   [TabGroup("Stats")]
   [SerializeField] private float _walkSpeed;
   [TabGroup("Stats")]
   [SerializeField] private float _midAirSpeedMultiplier;
   [TabGroup("Stats")]
   [SerializeField] private float _jumpForce;
   [TabGroup("Stats")]
   [SerializeField] private float _jumpDurationMin, _jumpDurationMax;

   [TabGroup("Camera")]
   [SerializeField] private CinemachineCamera _camera;
   [TabGroup("Camera")]
   [SerializeField] private Vector2 _aimSensitivity;
   [TabGroup("Camera")]
   [SerializeField] private float _aimSpeed;

   [TabGroup("Stats")]
   [SerializeField] private LayerMask _groundLayer;
   [TabGroup("Stats")]
   [SerializeField] private Vector3 _feetPositionOffset;
   [TabGroup("Stats")]
   [SerializeField] private float _feetRadius;
   [TabGroup("Stats")]
   [SerializeField] private int _numberOfJumps;
   [TabGroup("Stats")]
   [SerializeField] private float _extraGravity;
   [TabGroup("Stats")]
   [SerializeField] private float _jumpCooldown;
   
   [TabGroup("Animation")]
   [SerializeField] private Animator _animator;
   [TabGroup("Animation")]
   [SerializeField] private float _animationTransitionDuration;
   [TabGroup("Animation")]
   [SerializeField] private float _fallingTransitionDuration;
   [TabGroup("Animation")]
   [SerializeField] private float _fallingAnimationThreshold;
   [TabGroup("Animation")]
   [SerializeField] private string _runClip;
   [TabGroup("Animation")]
   [SerializeField] private string _idleClip;
   [TabGroup("Animation")]
   [SerializeField] private string _jumpClip;
   [TabGroup("Animation")]
   [SerializeField] private string _fallClip;
   
   [TabGroup("VFX")]
   [SerializeField] private GameObject _jumpEffectPrefab;
   
   private Rigidbody _rigidbody;
   private bool _grounded, _wasGrounded;
   private int _remainingJumps;
   private float _timeToJump;

   private float _timeSinceJump;
   private bool _isRunning, _wasRunning;
   
   public bool _isFalling {private set; get;}
   private bool _wasFalling;

   private void Start()
   {
      Cursor.lockState = CursorLockMode.Locked;
      UpdateStats();
      
      _rigidbody = GetComponent<Rigidbody>();
   }

   private void Update()
   {
      if (_timeToJump <= 0)
         JumpLoop();
      else
         _timeToJump -= Time.deltaTime;
      
   }

   private void JumpLoop()
   {
      _remainingJumps = _grounded? _numberOfJumps : _remainingJumps;
      
      if (Input.GetKeyDown(KeyCode.Space) && _remainingJumps > 0)
      {
         StartCoroutine(JumpCoroutine());
         _timeToJump = _jumpCooldown;
         _remainingJumps--;
      }
   }
   private void FixedUpdate()
   {
      Move();
      CheckIfGrounded();
      ExtraGravity();
   }
   private void Rotation(Vector3 forward)
   {
      var rot = Quaternion.LookRotation(forward);
      transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _aimSpeed);
   }

   private void LateUpdate()
   {
      _wasRunning = _isRunning;
      _wasGrounded = _grounded;
      _wasFalling = _isFalling;
   }

   private void Move()
   {
      var forwardDir = _camera.transform.forward;
      forwardDir.y = 0;
      forwardDir *= Input.GetAxisRaw("Vertical") * _walkSpeed * Time.deltaTime;
      forwardDir *= _grounded ? 1 : _midAirSpeedMultiplier;
      
      _rigidbody.AddForce(forwardDir,ForceMode.VelocityChange);
      
      _isRunning = forwardDir.sqrMagnitude > 0.1f;
      
      if(_isRunning)
         Rotation(forwardDir);

      if (!_grounded) return;
      
      if (_isRunning && !_wasRunning)
         _animator.CrossFade(_runClip,_animationTransitionDuration);
      else if(!_isRunning && _wasRunning)
         _animator.CrossFade(_idleClip,_animationTransitionDuration);
      
   }

   private IEnumerator JumpCoroutine()
   {
      _timeSinceJump = 0;
      
      var duration = _jumpDurationMin;
      _animator.CrossFade(_jumpClip, _animationTransitionDuration);

      if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 10, _groundLayer))
      {
         Instantiate(_jumpEffectPrefab,hit.point, Quaternion.LookRotation(hit.normal));
      }
      
      while (_timeSinceJump < duration && duration < _jumpDurationMax)
      {
         _rigidbody.AddForce(Vector3.up * _jumpForce * Time.deltaTime, ForceMode.Impulse);
         _timeSinceJump += Time.deltaTime;
         
         if(Input.GetKey(KeyCode.Space))
            duration+= Time.deltaTime;
         
         yield return null;
      }
   }

   private void OnValidate()
   {
      UpdateStats();
   }

   private void UpdateStats()
   {
      _camera.GetComponent<CinemachineInputAxisController>().Controllers[0].Input.Gain = _aimSensitivity.x;
      _camera.GetComponent<CinemachineInputAxisController>().Controllers[1].Input.Gain = _aimSensitivity.y;
   }

   private void CheckIfGrounded()
   {
      
      _grounded = Physics.CheckSphere(transform.position + transform.TransformVector(_feetPositionOffset), _feetRadius,_groundLayer);
      if (_grounded && !_wasGrounded)
      {
         if (_isRunning)
            _animator.CrossFade(_runClip,_animationTransitionDuration);
         else
            _animator.CrossFade(_idleClip,_animationTransitionDuration);
      }
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.DrawWireSphere(transform.position + transform.TransformVector(_feetPositionOffset), _feetRadius);
   }

   private void ExtraGravity()
   {
      _isFalling = _rigidbody.linearVelocity.y < _fallingAnimationThreshold;
      
      if (_isFalling && !_wasFalling && !_grounded)
         _animator.CrossFade(_fallClip,_fallingTransitionDuration);
      
      if(_rigidbody.linearVelocity.y < 0)
         _rigidbody.AddForce(Vector3.down * _extraGravity, ForceMode.Force);
   }
   
}
