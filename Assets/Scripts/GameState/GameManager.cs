using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
  public UnityEvent OnIntroBegin;
  public UnityEvent OnGameBegin;

  public void BeginGame()
  {
    OnIntroBegin?.Invoke();
  }
}
