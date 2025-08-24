using System.Collections;
using UnityEngine;

public class Uno_Shoot : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Uno>().Shoot();
    }
}
