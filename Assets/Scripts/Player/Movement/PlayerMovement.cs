using System;
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

   [TabGroup("Camera")]
   [SerializeField] private CinemachineCamera _camera;
   [TabGroup("Camera")]
   [SerializeField] private Vector2 _aimSensitivity;

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
   
   private Rigidbody _rigidbody;
   private bool _grounded;
   private int _remainingJumps;
   private float _timeToJump;

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
         Jump();
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
      transform.rotation = Quaternion.LookRotation(forward);
   }

   private void Move()
   {
      var moveDir = _camera.transform.forward;
      moveDir.y = 0;
      moveDir *= Input.GetAxisRaw("Vertical") * _walkSpeed * Time.fixedDeltaTime;
      moveDir *= _grounded ? 1 : _midAirSpeedMultiplier;
      
      _rigidbody.AddForce(moveDir,ForceMode.VelocityChange);

      if (moveDir.sqrMagnitude > 0.1f)
         Rotation(moveDir);
   }

   private void Jump()
   {
      _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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

   private void CheckIfGrounded() =>
      _grounded = Physics.CheckSphere(transform.position + transform.TransformVector(_feetPositionOffset), _feetRadius,_groundLayer);

   private void OnDrawGizmosSelected()
   {
      Gizmos.DrawWireSphere(transform.position + transform.TransformVector(_feetPositionOffset), _feetRadius);
   }

   private void ExtraGravity()
   {
      if(_rigidbody.linearVelocity.y < 0)
         _rigidbody.AddForce(Vector3.down * _extraGravity, ForceMode.Force);
   }
}
