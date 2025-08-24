using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private string[] _startClips;
    [SerializeField] private Animator _mllie;
    [SerializeField] private Animator _uno;

    [SerializeField] private TMP_InputField _code;
    [SerializeField] private Button _secret;
    private const string CODE = "password123";
    
    public void SetCharacter(int i)
    {
        var character = i == 0 ? _mllie : _uno;
        GetComponent<PlayerAttack>()._animator = character;
        GetComponent<PlayerMovement>()._animator = character;
        GetComponent<PlayerIntro>()._animator = character;
        
        _mllie.gameObject.SetActive(false);
        _uno.gameObject.SetActive(false);
        
        character.gameObject.SetActive(true);
        character.CrossFade(_startClips[Random.Range(0,_startClips.Length)],0);
    }
    private void Start()
    {
        SetCharacter(0);
        _secret.onClick.AddListener(()=> _code.gameObject.SetActive(!_code.gameObject.activeInHierarchy));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && _code.text == CODE)
        {
            SetCharacter(1);
        }
    }

}
