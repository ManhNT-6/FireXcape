using System.Collections;
using UnityEngine;

namespace Core
{
    public abstract class FireBase : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected Transform[] firePoints;
        [SerializeField] protected GameObject firePrefab;
        public float timeToDanger;
        public float timeToFail;
    
        protected float timer = 0f;
        protected bool isEnded = false;

        protected virtual void Update()
        {
            if (isEnded) return;
            timer += Time.deltaTime;
        
            if (timer > timeToFail) HandleTimeOut();
        }

        public abstract void ProcessInteraction(GameObject tool);
        protected abstract void HandleTimeOut();
    
        public virtual void FinishTraining(bool success, string message)
        {
            isEnded = true;
            if (!success) StartCoroutine(I_Blazing());
            FireEvents.OnTrainingResult?.Invoke(success, message);
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