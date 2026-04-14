using UnityEngine;

namespace Core
{
    public class PlayerSensor : MonoBehaviour
    {
        [Header("Settings")]
        public float interactCooldown;
        private float _lastInteractTime = -1f;

        private void OnEnable()
        {
            FireEvents.OnPickUpRequest += ResetCooldown;
        }

        private void OnDisable()
        {
            FireEvents.OnPickUpRequest -= ResetCooldown;
        }

        private void ResetCooldown(GameObject obj)
        {
            _lastInteractTime = Time.time;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Time.time < _lastInteractTime + interactCooldown) return;

            if (other.CompareTag("Interactable"))
            {
                Debug.Log("---Manh--- Sensor: Near " + other.name);
                FireEvents.OnNearbyInteractable?.Invoke(other.gameObject);
            }

            if (other.CompareTag("Fire"))
            {
                FireEvents.OnObjectNearFire?.Invoke(null, other.gameObject); 
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Time.time < _lastInteractTime + interactCooldown) return;

            if (other.CompareTag("Interactable"))
            {
                FireEvents.OnNearbyInteractable?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Interactable") || other.CompareTag("Fire"))
            {
                Debug.Log("---Manh--- Sensor: Gone " + other.name);
            }
        }
    }
}