using System;
using UnityEngine;

namespace GlobalSource
{
    public class TriggerDetector : MonoBehaviour
    {
        public event Action<Collider> OnTriggerExited; 
    
        private void OnTriggerExit(Collider other)
        {
            OnTriggerExited?.Invoke(other);
        }
    }
}