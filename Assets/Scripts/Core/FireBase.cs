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
    
        protected float timer = 0f;
        protected bool isEnded = false;

        protected virtual void Update()
        {
            if (isEnded) return;
            timer += Time.deltaTime;
            
            float progress = Mathf.Clamp01(timer / timeToFail);
            FireEvents.OnTimerUpdated?.Invoke(progress);
        
            if (timer > timeToFail) HandleTimeOut();
        }

        public abstract void ProcessInteraction(GameObject tool);
        protected abstract void HandleTimeOut();
    
        public virtual void FinishTraining(bool success, string message)
        {
            isEnded = true;
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