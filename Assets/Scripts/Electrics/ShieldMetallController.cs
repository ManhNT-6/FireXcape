using System;
using System.Collections;
using UnityEngine;

namespace Electrics
{
    public class ShieldMetallController : MonoBehaviour
    {
        [SerializeField] private GameObject mainKnob;
        [SerializeField] private float rotateSpeed;
        
        public static ShieldMetallController Instance;
        public static Action OnMainKnobTurnedOff;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
        }

        public void ToggleMainKnob()
        {
            if (transform.parent == null || transform.parent.name != "PlayerRightHand")
            {
                return;
            }
            StartCoroutine(I_ToggleMainKnob());
        }

        private IEnumerator I_ToggleMainKnob()
        {
            float currentX; 
            float targetX = 0f;
            float duration = 1.0f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float percent = elapsed / duration;

                currentX = Mathf.Lerp(-100f, targetX, percent);

                mainKnob.transform.localRotation = Quaternion.Euler(currentX, 0, 0);
        
                yield return null;
            }

            mainKnob.transform.localRotation = Quaternion.Euler(targetX, 0, 0);
            OnMainKnobTurnedOff?.Invoke();
        }
    }
}