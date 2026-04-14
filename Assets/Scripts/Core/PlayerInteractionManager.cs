using UnityEngine;

namespace Core
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        public Transform handAnchor; // The position the object will stick.
        private GameObject _currentHeldObject;
        private GameObject _currentNearbyFire;

        private void OnEnable()
        {
            FireEvents.OnPickUpRequest += PickUp;
            FireEvents.OnUseObjectOnFire += UseItemOnFire;
            FireEvents.OnObjectNearFire += (item, fire) => { _currentNearbyFire = fire; };

            FireEvents.OnNearbyInteractable += (obj) => { /* Logic saving the object you intend to hold. */ };
        }

        private void PickUp(GameObject target)
        {
            Debug.Log($"---Manh--- Pickup {target.name}");
            _currentHeldObject = target;
            target.transform.SetParent(handAnchor);
            target.transform.localPosition = Vector3.zero;
            target.transform.localRotation = Quaternion.identity;
        
            if(target.TryGetComponent<Rigidbody>(out var rb)) rb.isKinematic = true;
        }

        private void UseItemOnFire()
        {
            if (_currentHeldObject == null || _currentNearbyFire == null) return;
            
            var fireLogic = _currentNearbyFire.GetComponent<FireBase>();
            fireLogic?.ProcessInteraction(_currentHeldObject);
        }
    }
}