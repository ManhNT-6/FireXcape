using UnityEngine;

namespace Core
{
    public class PlayerInteractionManager : MonoBehaviour
    {
        public Transform handAnchor; 
        private GameObject _currentHeldObject;
        private GameObject _currentNearbyFire;

        private void OnEnable()
        {
            FireEvents.OnPickUpRequest += PickUp;
            FireEvents.OnUseObjectOnFire += UseItemOnFire;
            FireEvents.OnObjectNearFire += (item, fire) => { _currentNearbyFire = fire; };

            FireEvents.OnNearbyInteractable += (obj) => { /* Logic lưu obj tạm thời nếu cần */ };
        }

        private void PickUp(GameObject target)
        {
            if (target == null || target == _currentHeldObject) return;
            
            if (_currentHeldObject) DropCurrentObject(target.transform.position);

            _currentHeldObject = target;
            target.transform.SetParent(handAnchor);
            target.transform.localPosition = Vector3.zero;
            target.transform.localRotation = Quaternion.identity;
        
            if(target.TryGetComponent<Rigidbody>(out var rb)) 
            {
                rb.isKinematic = true;
            }
            if(target.TryGetComponent<Collider>(out var col)) col.enabled = false;
        }

        private void DropCurrentObject(Vector3 swapPosition)
        {
            if (_currentHeldObject == null) return;

            _currentHeldObject.transform.SetParent(null);
            _currentHeldObject.transform.position = swapPosition;

            if(_currentHeldObject.TryGetComponent<Rigidbody>(out var rb)) 
            {
                rb.isKinematic = false;
            }

            if(_currentHeldObject.TryGetComponent<Collider>(out var col)) col.enabled = true;
            _currentHeldObject = null;
        }

        private void UseItemOnFire()
        {
            if (_currentHeldObject == null || _currentNearbyFire == null) return;
            
            var fireLogic = _currentNearbyFire.GetComponent<FireBase>();
            fireLogic?.ProcessInteraction(_currentHeldObject);
        }
    }
}