using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using Unity.Cinemachine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _maxHeightDifference;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _minHeightDifference;
    
    [SerializeField] private float _upForce;
    [SerializeField] private float _lungeDuration;
    [SerializeField] private float _attackDuration;
    [SerializeField][Range(0f,1f)] private float _targetPositionInterpolant;

    [SerializeField] private CinemachineInputAxisController _cameraController;
    [SerializeField] private CinemachineCamera _cinemachineCam;
    [SerializeField] public Animator _animator;
    [SerializeField] private string _scratchClip;
    [SerializeField] private string _jumpClip;

    [SerializeField] private float _zoomInFov;
    [SerializeField] private float _zoomOutDuration;
    [SerializeField] private GameObject _slashEffects;
    [SerializeField] private int _damage;
    
    private PlayerMovement _playerMovement;
    private bool _attacking;
    private Rigidbody _rigidbody;
    private float _defaultFov;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody>();
        _defaultFov = _cinemachineCam.Lens.FieldOfView;
    }

    private void FixedUpdate()
    {
        if (_attacking) return;

        var targets = Physics.OverlapCapsule(transform.position - Vector3.up * _maxHeightDifference, transform.position,
            _attackRadius,_enemyLayer);
        
        if (targets.Length == 0) return;
        var target = targets[0];
        
        if(transform.position.y - target.transform.position.y < _minHeightDifference)
            return;
        
        if (_rigidbody.linearVelocity.y < -0.5f && target)
        {
            StartCoroutine(AttackCoroutine(target.transform));
        }
    }

    private IEnumerator AttackCoroutine(Transform target)
    {
        _attacking = true;
        _playerMovement.enabled = false;
        _rigidbody.isKinematic = true;
        _cameraController.enabled = false;
        
        var targetPos = Vector3.Lerp(transform.position, target.position, _targetPositionInterpolant);
        var rot = Quaternion.LookRotation(_cameraController.transform.forward);
        
        transform.DOMove(targetPos,_lungeDuration).SetEase(Ease.OutBack);
        transform.DORotateQuaternion(rot, _lungeDuration).SetEase(Ease.OutBack);
        
        DOTween.To(()=> _cinemachineCam.Lens.FieldOfView,x => _cinemachineCam.Lens.FieldOfView = x,
            _zoomInFov, _lungeDuration).SetEase(Ease.OutBack);
        
        _animator.CrossFade(_scratchClip, 0);
        target.GetComponent<IScratchable>().BeginScratch(transform);
        yield return new WaitForSeconds(_lungeDuration);
        
        _slashEffects.SetActive(true);
        
        yield return new WaitForSeconds(_attackDuration);
        
        _slashEffects.SetActive(false);
        
        target.GetComponent<IScratchable>().TakeDamage(_damage);
        _animator.CrossFade(_jumpClip, 0);

        _cameraController.enabled = true;
        _playerMovement.enabled = true;
        _rigidbody.isKinematic = false;
        
        DOTween.To(()=> _cinemachineCam.Lens.FieldOfView,x => _cinemachineCam.Lens.FieldOfView = x,
            _defaultFov, _zoomOutDuration);
        
        _rigidbody.AddForce(Vector3.up * _upForce,ForceMode.Impulse);
        _attacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position,transform.position -  Vector3.up * _maxHeightDifference);
    }
}
