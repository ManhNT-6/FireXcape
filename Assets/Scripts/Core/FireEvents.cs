using System;
using UnityEngine;

namespace Core
{
    public static class FireEvents
    {
        // When approach an object
        public static Action<GameObject> OnNearbyInteractable; 
    
        // When the user selects "Hold" on the UI.
        public static Action<GameObject> OnPickUpRequest;

        // When bringing an object close to a fire
        public static Action<GameObject, GameObject> OnObjectNearFire;

        // When the user selects "Use" the item in the fire.
        public static Action OnUseObjectOnFire;

        // When finished => show a Result popup.
        public static Action<bool, string> OnTrainingResult;
        
        public static Action OnLeaveInteractable;
        public static Action OnLeaveFireZone;
    }
}