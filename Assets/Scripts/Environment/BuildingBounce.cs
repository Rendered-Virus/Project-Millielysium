using System;
using DG.Tweening;
using UnityEngine;

public class BuildingBounce : MonoBehaviour
{
    [SerializeField] private float _timePeriod;
    [SerializeField] private float _maxHeight;

    private void Start()
    {
        transform.DOScaleY(_maxHeight, _timePeriod).SetEase(Ease.InQuad).SetLoops(-1, LoopType.Yoyo);
    }
}
