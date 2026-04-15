using UnityEngine;

namespace Core
{
    public class AudioManager : MonoBehaviour
    {
        // Biến toàn cục (Singleton) giúp các file khác dễ dàng gọi AudioManager
        public static AudioManager Instance; 

        [Header("Bộ Phát Âm Thanh (Audio Sources)")]
        public AudioSource sfxSource; // Kênh phát tiếng động (nhặt đồ, thắng, thua)
        public AudioSource uiSource;  // Kênh phát tiếng UI (bấm nút)

        [Header("File Âm Thanh (Audio Clips)")]
        public AudioClip pickUpClip;     // Tiếng "cạch" khi nhấc đồ
        public AudioClip successClip;    // Tiếng chuông báo thành công
        public AudioClip failClip;       // Tiếng còi báo động thất bại
        public AudioClip buttonClickClip; // Tiếng "tít" khi bấm UI

        private void Awake()
        {
            // BƯỚC QUAN TRỌNG: Đảm bảo chỉ có 1 AudioManager duy nhất, và KHÔNG BỊ XÓA khi chuyển Scene
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            // Lắng nghe sự kiện từ FireEvents để tự động phát tiếng
            FireEvents.OnPickUpRequest += PlayPickUp;
            FireEvents.OnTrainingResult += PlayResult;
        }

        private void OnDisable()
        {
            // Ngừng lắng nghe khi object bị tắt
            FireEvents.OnPickUpRequest -= PlayPickUp;
            FireEvents.OnTrainingResult -= PlayResult;
        }

        private void PlayPickUp(GameObject obj) 
        { 
            if (pickUpClip != null && sfxSource != null) 
                sfxSource.PlayOneShot(pickUpClip); 
        }

        private void PlayResult(bool success, string msg) 
        { 
            // Tắt ngay các tiếng động cũ (như tiếng nhặt đồ đang dở)
            if (sfxSource != null) sfxSource.Stop(); 
            
            if(success && successClip != null) 
                sfxSource.PlayOneShot(successClip); 
            else if (!success && failClip != null) 
                sfxSource.PlayOneShot(failClip); 
        }

        // Hàm này dành riêng để UIManager gọi khi người chơi bấm nút
        public void PlayUIClick() 
        { 
            if (buttonClickClip != null && uiSource != null) 
                uiSource.PlayOneShot(buttonClickClip); 
        }
    }
}