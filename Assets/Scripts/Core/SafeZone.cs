using System;
using UnityEngine;

namespace Core
{
    public class SafeZone : MonoBehaviour
    {
        private bool _isSafe;

        private void OnEnable()
        {
            _isSafe = false; // mac dinh dang nguy hiem
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))  return;

            _isSafe = !_isSafe;
            FireEvents.OnSafeZoneStateChanged?.Invoke(_isSafe);
        }
    }
}