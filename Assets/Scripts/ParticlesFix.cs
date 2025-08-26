using UnityEngine;

public class ParticlesFix : MonoBehaviour
{
    [SerializeField] private Transform _doomPoint;
    [SerializeField] private GameObject _doom;
    private GameObject _doomed;
    [SerializeField] private Transform _explosionPoint;
    [SerializeField] private GameObject _explosion;

    public void Doom()
    {
        _doomed = Instantiate(_doom, _doomPoint);
        _doomed.transform.localPosition = Vector3.zero;
        _doomed.transform.localEulerAngles = Vector3.zero;
    }

    public void Explode()
    {
        var exp = Instantiate(_explosion, _explosionPoint);
        exp.transform.localPosition = Vector3.zero;
        exp.transform.localEulerAngles = Vector3.zero;
        
        Destroy(_doomed);
    }
}
