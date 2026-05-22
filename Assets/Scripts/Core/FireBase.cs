using System.Collections;
using UnityEngine;

namespace Core
{
    public abstract class FireBase : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] protected GameObject fireObject;
        [SerializeField] protected Transform[] firePoints;
        [SerializeField] protected GameObject firePrefab;
        public float extinguishDuration;
        public float timeToFail;
        private const float WarningTime = 30f;

        private float _timer;
        private bool _isEnded;
        private bool _isWarning;

        private bool _isStartTraining;
        private bool _isPlayerInSafeZone;

        protected virtual void OnEnable()
        {
            FireEvents.OnSafeZoneStateChanged += UpdatePlayerSafeState;
            FireEvents.OnTrainingStart += UpdateTrainingState;
        }

        protected virtual void OnDisable()
        {
            FireEvents.OnSafeZoneStateChanged -= UpdatePlayerSafeState;
            FireEvents.OnTrainingStart -= UpdateTrainingState;
        }

        private void UpdatePlayerSafeState(bool isSafe)
        {
            _isPlayerInSafeZone = isSafe;
            if (isSafe) HandleTimeOut();
        }

        private void UpdateTrainingState()
        {
            _isStartTraining = true;
        }

        protected virtual void Update()
        {
            if (_isEnded) return;
            if (!_isStartTraining) return;
            _timer += Time.deltaTime;
            
            float progress = Mathf.Clamp01(_timer / timeToFail);
            FireEvents.OnTimerUpdated?.Invoke(progress);
            
            CheckWarning();
        
            if (_timer > timeToFail) HandleTimeOut();
        }

        public abstract void ProcessInteraction(GameObject tool);

        private void CheckWarning()
        {
            if (_timer < WarningTime) return;
            if (_isWarning) return;
            
            _isWarning = true;
            StartCoroutine(I_Blazing());
            FireEvents.OnDangerWarning?.Invoke(); // update ui canh bao 
        }

        private void HandleTimeOut()
        {
            if (_isPlayerInSafeZone)
            {
                FinishTraining(true, "Bạn đã an toàn! Hoàn thành xuất sắc.");
            }
            else
            {
                FinishTraining(false, "Hết thời gian. Bạn đã bị mắc kẹt trong đám cháy.");
            }
        }
    
        public virtual void FinishTraining(bool success, string message)
        {
            _isEnded = true;
            StartCoroutine(!success ? I_Blazing() : I_ExtinguishAndShowResult());
            FireEvents.OnTrainingResult?.Invoke(success, message);
        }
        
        private IEnumerator I_ExtinguishAndShowResult()
        {
            if (fireObject == null) yield break;
            
            Vector3 startScale = fireObject.transform.localScale;
            float elapsed = 0f;

            while (elapsed < extinguishDuration)
            {
                elapsed += Time.deltaTime;
                fireObject.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsed / extinguishDuration);
                yield return null;
            }
            fireObject.SetActive(false);
        }

        private IEnumerator I_Blazing()
        {
            foreach (var point in firePoints)
            {
                Instantiate(firePrefab, point.position, Quaternion.identity);
            }
            
            yield return new WaitForSeconds(3f);
        }
    }
}