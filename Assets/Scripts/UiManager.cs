using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _firstDelay;
    [SerializeField] private float _transitionDuration;
    private bool _canKill;
    [SerializeField] private int _killsRequired;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        GameManager.Instance.OnGameBegin.AddListener(()=> StartCoroutine(Begin()));
    }

    private IEnumerator Begin()
    {
        yield return new WaitForSeconds(_firstDelay);
        _text.text = "Stomp makes die";
        _text.DOFade(1, _transitionDuration);
        yield return new WaitForSeconds(3);

        _canKill = true;
        _text.text = "Killed :" + NPCManager.Instance.killCount + "/" + _killsRequired;
        NPCManager.Instance.OnKilled.AddListener(()=>
        {
            _text.text = "Killed :" + NPCManager.Instance.killCount + "/" + _killsRequired;
            
            if (NPCManager.Instance.killCount >= _killsRequired)
                GameManager.Instance.NextScene();
        });
    }
}
