using UnityEngine;

namespace Core
{
    public abstract class FireBase : MonoBehaviour
    {
        [Header("Settings")]
        public float timeToDanger = 10f;
        public float timeToFail = 20f;
    
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
            FireEvents.OnTrainingResult?.Invoke(success, message);
        }
    }
}