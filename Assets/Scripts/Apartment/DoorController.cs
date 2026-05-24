using System.Collections;
using Core;
using UnityEngine;

namespace Apartment
{
    public class DoorController : MonoBehaviour
    {
        [Header("Door Leafs")]
        [SerializeField] private Transform leftDoor;
        [SerializeField] private Transform rightDoor;

        [Header("Settings")]
        [SerializeField] private float openSpeed;

        private bool _isOpen;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OpenTheDoor();
            }
        }

        private void OpenTheDoor()
        {
            if (_isOpen) return;
            _isOpen = true;
            
            if (leftDoor != null)
            {
                StartCoroutine(I_SmoothOpen(leftDoor, 90f));
            }
            
            if (rightDoor != null)
            {
                StartCoroutine(I_SmoothOpen(rightDoor, -90f));
            }
        }

        private IEnumerator I_SmoothOpen(Transform door, float targetYAngle)
        {
            Quaternion startRot = door.rotation;
            Quaternion targetRot = Quaternion.Euler(startRot.eulerAngles.x, targetYAngle, startRot.eulerAngles.z);
            
            float timeElapsed = 0f;

            while (timeElapsed < 2f)
            {
                timeElapsed += Time.deltaTime;
                door.rotation = Quaternion.Lerp(startRot, targetRot, timeElapsed);
                yield return null;
            }
            
            door.rotation = targetRot;
        }
    }
}