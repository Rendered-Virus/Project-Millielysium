using UnityEngine;

public class Uno_Jump : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Uno>().Jump();
    }
}
