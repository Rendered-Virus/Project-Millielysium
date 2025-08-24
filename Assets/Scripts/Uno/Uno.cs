using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Uno : MonoBehaviour,IScratchable
{
    [SerializeField] private Transform _player;
    
    [SerializeField][TabGroup("Shoot")] private GameObject _bulletPrefab;
    [SerializeField][TabGroup("Shoot")]  private GameObject _buildUp;
    [SerializeField][TabGroup("Shoot")] private float _buildupTime;
    [SerializeField][TabGroup("Shoot")] private Transform _shootPoint;
    [SerializeField][TabGroup("Shoot")] private int _bulletNumber;
    [SerializeField][TabGroup("Shoot")] private float _bulletAngle;
    [SerializeField][TabGroup("Shoot")] private float _bulletSpeed;
  
    [SerializeField][TabGroup("Idle")][MinMaxSlider(0,10,true)] private Vector2 _idleTime;
    
    [SerializeField][TabGroup("Jump")] private float _jumpDuration;
    [SerializeField][TabGroup("Jump")][MinMaxSlider(0,10,true)] private Vector2 _upDuration;
    [SerializeField][TabGroup("Jump")] private float _shiftRange;

    private Animator _animator;
    private Collider _collider;
    
    private float _defaultYScale;
    private Vector3 _target;
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _defaultYScale = transform.localScale.y;
    }

    public void Shoot()
    {
        Instantiate(_buildUp,_shootPoint.position,Quaternion.identity);
        _target = _player.position;
        Invoke(nameof(ShootBullets),_buildupTime);
    }

    private void ShootBullets()
    {
        var dir = _target - transform.position;
        dir.y = 0;
        transform.forward = dir;
        
        var startAngle = -_bulletAngle / 2 * (_bulletNumber - 1);
        
        for (float i =startAngle; i <= -startAngle; i += _bulletAngle)
        {
            print(i);
            var forward = _shootPoint.forward;
            forward.y = 0;
            forward = Quaternion.AngleAxis(i,Vector3.up) * forward;
          
            var b = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.LookRotation(forward)).GetComponent<Rigidbody>();
            
            b.linearVelocity = forward.normalized * _bulletSpeed;
        }
        _animator.CrossFade("Idle",0.1f);
    }

    public void WaitIdle(string next)
    {
        StartCoroutine(NextMove(next,Random.Range(_idleTime.x,_idleTime.y)));
    }

    private IEnumerator NextMove(string next, float time)
    {
        yield return new WaitForSeconds(time);
        _animator.CrossFade(next, 0);
    }
    private IEnumerator ComeDown()
    {
        var time = Random.Range(_upDuration.x, _upDuration.y);
        yield return new WaitForSeconds(time);
        transform.DOMoveY(0, _jumpDuration);
        transform.DOScaleY(_defaultYScale, _jumpDuration).OnComplete(()=>
        {
            _animator.CrossFade("Idle", 0);
            _collider.enabled = true;
        });
    }
    public void Jump()
    {
        _collider.enabled = false;
        transform.DOScaleY(0.5f,.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            transform.DOMoveY(100, _jumpDuration).OnComplete(() =>
            {
                var height = transform.position.y;
                var pos = new Vector3(_player.position.x + Random.insideUnitCircle.x * _shiftRange,height,_player.position.z +  Random.insideUnitCircle.y * _shiftRange); 
                transform.position = pos;
            });

            StartCoroutine(ComeDown());
        });
        
    }

    public void TakeDamage(int damage)
    {
        _animator.CrossFade("Idle", 0);
    }

    public void BeginScratch(Transform scratcher)
    {
        StopAllCoroutines();
       _animator.CrossFade("Scratch", 0);
    }
}