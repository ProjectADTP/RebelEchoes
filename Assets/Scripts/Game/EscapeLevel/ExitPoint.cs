using UnityEngine;
using System;

public class ExitPoint : MonoBehaviour
{
   public event Action Entered;
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.TryGetComponent<PlayerHealth>(out _))
         Entered?.Invoke();
   }
}