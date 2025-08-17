using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackRadius;
    [SerializeField] private float _maxHeightDifference;
    [SerializeField] private LayerMask _enemyLayer;
    
    [SerializeField] private float _upForce;
    [SerializeField] private float _lungeDuration;
    [SerializeField] private float _attackDuration;

    [SerializeField] private CinemachineInputAxisController _cameraController;
    
    private PlayerMovement _playerMovement;
    private bool _attacking;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_attacking) return;

        var targets = Physics.OverlapCapsule(transform.position - Vector3.up * _maxHeightDifference, transform.position,
            _attackRadius,_enemyLayer);
        
        if (targets.Length == 0) return;
        
        var target = targets[0];

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

        var rot = Quaternion.LookRotation(_cameraController.transform.forward);
        
        transform.DOMove(target.position,_lungeDuration).SetEase(Ease.InOutBack);
        transform.DORotateQuaternion(rot, _lungeDuration).SetEase(Ease.InOutBack);
        _cameraController.enabled = false;
        
        yield return new WaitForSeconds(_lungeDuration);
        
        yield return new WaitForSeconds(_attackDuration);

        _cameraController.enabled = true;
        _playerMovement.enabled = true;
        _rigidbody.isKinematic = false;
        
        _rigidbody.AddForce(Vector3.up * _upForce,ForceMode.Impulse);
        _attacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position,transform.position -  Vector3.up * _maxHeightDifference);
    }
}
