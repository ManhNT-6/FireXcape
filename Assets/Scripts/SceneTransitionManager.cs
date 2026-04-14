using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [Header("UI References")]
        [Tooltip("Kéo CanvasGroup của màn hình đen dùng để chuyển cảnh vào đây")]
        public CanvasGroup fadeCanvasGroup; 
        public float fadeDuration = 1f;

        // Pattern Singleton để gọi từ bất kỳ đâu: SceneTransitionManager.Instance.LoadScene(...)
        public static SceneTransitionManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Giữ Object này tồn tại khi qua Scene mới
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Đảm bảo khi bắt đầu game màn hình sẽ trong suốt
            if(fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 0f;
                fadeCanvasGroup.blocksRaycasts = false;
            }
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(TransitionToScene(sceneName));
        }

        private IEnumerator TransitionToScene(string sceneName)
        {
            // 1. Bật tương tác của màn hình đen để chặn người dùng bấm lung tung khi đang load
            fadeCanvasGroup.blocksRaycasts = true;

            // 2. Hiệu ứng Fade Out (Màn hình tối dần)
            float time = 0;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, time / fadeDuration);
                yield return null;
            }

            // 3. Load Scene bất đồng bộ (Async) để không bị đơ (lag) game
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone)
            {
                // Sau này có thể thêm thanh Loading Bar ở đây: loadingBar.value = asyncLoad.progress;
                yield return null; 
            }

            // 4. Hiệu ứng Fade In (Màn hình sáng dần lên khi load xong)
            time = 0;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, time / fadeDuration);
                yield return null;
            }

            // 5. Tắt tương tác màn hình đen
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }
}