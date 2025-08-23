using UnityEngine;
using UnityEngine.AI;

public class WalkingNPC : NPC
{
    [SerializeField] private string _runningAnimation;
    [SerializeField] private float _walkingSwitchTime;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _walkRange;
    private float _timeToSwitchWalking;
    private float _runSpeed;
    private bool _begun;
    
    
    protected override void Start()
    {
        base.Start();
        
        GameManager.Instance.OnIntroBegin.AddListener(()=> _begun = true);
        
        _runSpeed = _agent.speed;
        _animator.CrossFade(_runningAnimation, 0);
    }
    protected override void Update()
    {
        if(!_begun) return;
        
        base.Update();

        if (!_panic && !_isScratching && (_timeToSwitchWalking <= 0 || _agent.remainingDistance <= _agent.stoppingDistance))
        {
            _agent.speed = _walkSpeed;
            _agent.SetDestination(GetRandomPos(_walkRange));
            _timeToSwitchWalking = _walkingSwitchTime;
        }
        else _agent.speed = _runSpeed;
    }

}
