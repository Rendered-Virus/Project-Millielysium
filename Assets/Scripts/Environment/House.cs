using UnityEngine;

public class House : MonoBehaviour
{
    public void Open()
    {
        GetComponent<Animator>().CrossFade("Open", 0f);
    }
}
