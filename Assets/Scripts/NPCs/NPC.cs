using System;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour, IScratchable
{
    [SerializeField] private string _scratchedAnimation = "Scratch";
    [SerializeField] private string _panicRunAnimation = "Panic";
    
    [SerializeField] private float _heightWhenScratched;
    
    protected bool _panic;
    protected Animator _animator;
    protected bool _isScratching;
    protected NavMeshAgent _agent;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        NPCManager.Instance.OnPanicStart.AddListener(Panic);
    }

    public void TakeDamage(int damage)
    {
        NPCManager.Instance.BeginPanic();
        NPCManager.Instance.OnPanicStart.RemoveListener(Panic);
        Destroy(gameObject);
    }

    public void BeginScratch(Transform scratcher)
    {
        var dir = scratcher.position - transform.position;
        transform.position += Vector3.up * _heightWhenScratched;
        
        transform.rotation = Quaternion.LookRotation(dir);
        _animator.CrossFade(_scratchedAnimation,0);
    }
 
    public void Panic()
    {
        _panic = true;
        _animator.CrossFade(_panicRunAnimation,0);
    }
}
