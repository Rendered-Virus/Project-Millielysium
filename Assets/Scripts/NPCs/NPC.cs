using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour, IScratchable
{
    [SerializeField] private string _scratchedAnimation = "Scratch";
    [SerializeField] private string _panicRunAnimation = "Panic";

    [SerializeField] private GameObject _bodyParts;
    [SerializeField] private float _heightWhenScratched;
    [SerializeField] private float _runRange;
    [SerializeField] private float _panicTargetSwitchTime;
    
    
    protected bool _panic;
    protected Animator _animator;
    protected bool _isScratching;
    protected NavMeshAgent _agent;
    private float _timeToSwitch;

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
        NPCManager.Instance.Killed();
        
        Instantiate(_bodyParts, transform.position, transform.rotation);
        
        Destroy(gameObject);
    }

    public void BeginScratch(Transform scratcher)
    {
        _agent.enabled = false;
        _isScratching = true;
        
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

    protected virtual void Update()
    {
        if (_panic && !_isScratching && (_timeToSwitch <= 0 || _agent.remainingDistance <= _agent.stoppingDistance))
        {
            _agent.SetDestination(GetRandomPos(_runRange));
            _timeToSwitch = _panicTargetSwitchTime;
        }
    }
    
    protected Vector3 GetRandomPos(float range) {
        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas)) {
                return hit.position;
            }
        }
        print("Not found");
        return Vector3.zero;
    }
}
