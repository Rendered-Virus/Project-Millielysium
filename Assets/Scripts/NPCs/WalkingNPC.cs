using UnityEngine;
using UnityEngine.AI;

public class WalkingNPC : NPC
{
    [SerializeField] private string _runningAnimation;
    [SerializeField] private string _panicAnimation = "Panic";
}
