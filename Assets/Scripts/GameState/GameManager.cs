using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
  public UnityEvent OnIntroBegin;
  public UnityEvent OnGameBegin;
  public UnityEvent OnStart;

  [SerializeField] private Image _black;
  [SerializeField] private float _transitionDuration;
  

  private void Start()
  {
    _black.DOFade(0,_transitionDuration).OnComplete(() => OnStart?.Invoke());
  }

  public void BeginGame()
  {
    OnGameBegin?.Invoke();
  }

  private void Update()
  {
    if(Input.GetKeyDown(KeyCode.R))
      OnIntroBegin?.Invoke();
  }

  public void NextScene()
  {
    _black.DOFade(0,_transitionDuration).OnComplete(() =>
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    });
  }
}
