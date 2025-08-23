using System;
using DG.Tweening;
using UnityEngine;

public class BodyPartExploder : MonoBehaviour
{
    [SerializeField] private float _shrinkDelay;
    [SerializeField] private float _shrinkDuration;
    [SerializeField] private Vector2 _ShootForce;
    private Rigidbody[] _parts;
    private void Start()
    {
        Invoke(nameof(Shrink), _shrinkDelay);
        _parts = GetComponentsInChildren<Rigidbody>();
        foreach (var part in _parts)
        {
            part.AddExplosionForce(_ShootForce.magnitude,transform.position,_ShootForce.x,_ShootForce.y);
        }
    }

    private void Shrink()
    {
        foreach (var part in _parts)
        {
            part.transform.DOScale(0, _shrinkDuration).OnComplete(()=> Destroy(gameObject));
        }
    }
}
