using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class TimerUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Slider timerSlider;
        [SerializeField] private Image fillImage;

        [Header("Settings")]
        [SerializeField] private Color normalColor = Color.green;
        [SerializeField] private Color warningColor = Color.red;
        [Range(0, 1)] [SerializeField] private float warningThreshold = 0.3f;

        private void OnEnable()
        {
            FireEvents.OnTimerUpdated += UpdateSlider;
            FireEvents.OnTrainingResult += HideOnFinish;
        }

        private void OnDisable()
        {
            FireEvents.OnTimerUpdated -= UpdateSlider;
            FireEvents.OnTrainingResult -= HideOnFinish;
        }

        private void UpdateSlider(float progress)
        {
            if (!timerSlider) return;

            float remainingValue = 1f - progress;
            timerSlider.value = remainingValue;

            if (fillImage)
            {
                fillImage.color = (remainingValue <= warningThreshold) ? warningColor : normalColor;
            }
        }

        private void HideOnFinish(bool success, string message)
        {
            gameObject.SetActive(false);
        }
    }
}