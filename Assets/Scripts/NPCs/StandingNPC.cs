using UnityEngine;

public class StandingNPC : NPC
{
   [SerializeField] private string _idleAnimation;

   protected override void Start()
   {
      base.Start();
      _animator.CrossFade(_idleAnimation, 0);
   }
}
