using System;
using System.Collections;
using UnityEngine;

public class endscene : MonoBehaviour
{
   [SerializeField] private float _delay;

   private IEnumerator Start()
   {
      yield return new WaitForSeconds(_delay);
      GameManager.Instance.Load(0);
   }
}
