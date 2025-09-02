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
  public UnityEvent OnGameEnd;

  [SerializeField] private Image _black;
  [SerializeField] private float _transitionDuration;
  [SerializeField] private CanvasGroup _menu;

  private void Start()
  {
    Application.targetFrameRate = 60;
    _black.DOFade(0,_transitionDuration).SetEase(Ease.InCubic).OnComplete(() => OnStart?.Invoke());
  }

  public void BeginGame()
  {
    OnGameBegin?.Invoke();
  }

  public void BeginIntro()
  {
    _menu.DOFade(0, .3f).OnComplete(() => OnIntroBegin?.Invoke());
    _menu.interactable = false;
    
  }

  public void NextScene()
  {
    _black.DOFade(1,_transitionDuration).SetEase(Ease.OutCubic).OnComplete(() =>
    {
      OnGameEnd?.Invoke();
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    });
  }

  public void Respawn()
  {
    _black.DOFade(1,_transitionDuration).SetEase(Ease.OutCubic).OnComplete(() =>
    {
      
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    });
  }

  public void Load(int i)
  {
    _black.DOFade(1,_transitionDuration).SetEase(Ease.OutCubic).OnComplete(() =>
    {
      
      SceneManager.LoadScene(i);
    });
  }
}
