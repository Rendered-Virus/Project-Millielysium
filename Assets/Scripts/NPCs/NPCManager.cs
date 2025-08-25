using UnityEngine;
using UnityEngine.Events;

public class NPCManager : Singleton<NPCManager>
{
   public UnityEvent OnPanicStart;
   private bool _panic;
   public UnityEvent OnKilled;
   public int killCount;

   public void BeginPanic()
   {
      if(_panic) return;
      
      OnPanicStart?.Invoke();
      _panic = true;
   }

   public void Killed()
   {
      killCount++;
      OnKilled?.Invoke();
   }
}
