using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] private DialogueSequence[] _sequences;
    [SerializeField] private bool _right;
    private int _currentSequence;

    public void Play()
    {
        DialogueManager.Instance.PlayDialogue(_sequences[_currentSequence],_right);
        _currentSequence++;
    }
}
