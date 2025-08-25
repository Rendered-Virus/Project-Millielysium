using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private CanvasGroup _DialogueGroup;
    [SerializeField] private Image _rightIcon, _leftIcon;
    [SerializeField] private TextMeshProUGUI _leftNameText, _rightNameText;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _characterDuration;

    private AudioSource _textAudio;
    private bool _playing;

    private void Start()
    {
        _textAudio = GetComponent<AudioSource>();
        _textAudio.enabled = false;
    }

    public void PlayDialogue(DialogueSequence sequence, bool right)
    {
        if(_playing) return;
        
        _rightIcon.gameObject.SetActive(right);
        _leftIcon.gameObject.SetActive(!right);
        
        _rightNameText.transform.parent.gameObject.SetActive(right);
        _leftNameText.transform.parent.gameObject.SetActive(!right);

        var nameText = right? _rightNameText : _leftNameText;
        nameText.text = sequence.Name;
        
        var icon =  right? _rightIcon : _leftIcon;
        icon.sprite = sequence.Icon;
        
        _playing = true;
        StartCoroutine(DialogueCoroutine(sequence));
    }

    private IEnumerator DialogueCoroutine(DialogueSequence sequence)
    {
        _DialogueGroup.alpha = 1;
        foreach (var line in sequence.Lines)
        {
            _text.text = "";
            _textAudio.enabled = true;
            
            foreach (var letter in line)
            {
                _text.text += letter;
                yield return new WaitForSeconds(_characterDuration);
            }

            _textAudio.enabled = false;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        
        _playing = false;
        _DialogueGroup.alpha = 0;
        sequence.OnEnd?.Invoke();
    }
}

[System.Serializable]
public class DialogueSequence
{
    public string Name;
    public Sprite Icon;
    public UnityEvent OnEnd;
    [TextArea] public string[] Lines;
}
