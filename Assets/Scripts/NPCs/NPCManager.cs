using UnityEngine;
using UnityEngine.Events;

public class NPCManager : Singleton<NPCManager>
{
   public UnityEvent OnPanicStart;
   private bool _panic;
   
   public void BeginPanic()
   {
      if(_panic) return;
      
      OnPanicStart?.Invoke();
      _panic = true;
   }
}
