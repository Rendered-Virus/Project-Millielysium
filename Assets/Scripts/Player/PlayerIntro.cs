using System;
using DG.Tweening;
using UnityEngine;

public class PlayerIntro : MonoBehaviour
{
    [SerializeField] public Animator _animator;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private Material _awakeMaterial;
    

    private void Start()
    {
        DOTween.defaultUpdateType = UpdateType.Fixed;
        
        GameManager.Instance.OnIntroBegin.AddListener(Jump);
    }
    
    private void Jump()
    {
        _meshRenderer.material = _awakeMaterial;
        _animator.CrossFade("Launch", 0);
    }
                                  
    
}
