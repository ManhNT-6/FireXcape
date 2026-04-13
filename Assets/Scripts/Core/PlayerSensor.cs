using UnityEngine;

namespace Core
{
    public class PlayerSensor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // 1. Detects proximity to interactive objects
            if (other.CompareTag("Interactable"))
            {
                Debug.Log("---Manh--- Check: Near " + other.name);
                FireEvents.OnNearbyInteractable?.Invoke(other.gameObject);
            }

            // 2. Discovering to get close to Fire while holding an item.
            if (other.CompareTag("Fire"))
            {
                FireEvents.OnObjectNearFire?.Invoke(null, other.gameObject); 
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Interactable") || other.CompareTag("Fire"))
            {
                // Call OnExit event to hide the UI.
                Debug.Log("---Manh--- Check: Gone " + other.name);
            }
        }
    }
}