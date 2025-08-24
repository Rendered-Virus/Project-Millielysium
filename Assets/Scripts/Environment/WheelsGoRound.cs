using System;
using UnityEngine;

public class WheelsGoRound : MonoBehaviour
{
   [SerializeField] private Transform _frontWheel, _rearWheel;
   [SerializeField] private float _wheelSpeed;

   private void Update()
   {
      var rot = _frontWheel.eulerAngles;
      rot.z += _wheelSpeed *  Time.deltaTime;
      
      _frontWheel.eulerAngles = rot;
      _rearWheel.eulerAngles = rot;
   }
}
