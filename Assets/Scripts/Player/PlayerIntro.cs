using System;
using UnityEngine;

public class PlayerIntro : MonoBehaviour
{
    [SerializeField] private string[] _startClips;
    [SerializeField] private Animator _animator;
    
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;
    
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
        
        GameManager.Instance.OnIntroBegin.AddListener(Jump);
    }
    
    private void Jump()
    {
        GameManager.Instance.OnGameBegin?.Invoke();
    }
}
