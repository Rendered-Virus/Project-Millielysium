using UnityEngine;

public class Uno_Idle : StateMachineBehaviour
{
    private Uno _base;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _base = animator.GetComponent<Uno>();
        var next = Random.value > 0.7f ? "Jump" : "Shoot";
        _base.WaitIdle(next);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
