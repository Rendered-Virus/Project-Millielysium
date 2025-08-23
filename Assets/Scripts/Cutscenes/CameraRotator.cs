using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private Vector2 _endValue;
    [SerializeField] private float _duration;
    private CinemachineOrbitalFollow _cam;

    public void Begin()
    {
        _cam = GetComponent<CinemachineOrbitalFollow>();
      
        DOTween.To(() => _cam.HorizontalAxis.Value,x => _cam.HorizontalAxis.Value = x, _endValue.x, _duration);
        DOTween.To(() => _cam.VerticalAxis.Value,x => _cam.VerticalAxis.Value = x, _endValue.y, _duration).OnComplete((
            () =>
            {
                transform.GetChild(0).position = transform.position;
                transform.GetChild(0).parent = null;
            }));
    }
}
