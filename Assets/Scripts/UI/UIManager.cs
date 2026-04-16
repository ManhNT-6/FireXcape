using Electrics;
using UnityEngine;
using TMPro;

namespace Core
{
    public class UIManager : MonoBehaviour
    {
        [Header("Khung Popup")]
        public GameObject popupStart;
        public GameObject popupInteract;
        public GameObject popupFire;
        public GameObject popupResult;
        public GameObject popupSummary;

        [Header("Chữ Kết Quả")]
        public TextMeshProUGUI txtResultTitle;
        public TextMeshProUGUI txtResultMessage;

        private GameObject _currentItem;

        private void OnEnable()
        {
            FireEvents.OnNearbyInteractable += ShowInteract;
            FireEvents.OnObjectNearFire += ShowFire;
            FireEvents.OnTrainingResult += ShowResult;
        }

        private void OnDisable()
        {
            FireEvents.OnNearbyInteractable -= ShowInteract;
            FireEvents.OnObjectNearFire -= ShowFire;
            FireEvents.OnTrainingResult -= ShowResult;
        }

        private void Start()
        {
            HideAll();
            if (popupStart) popupStart.SetActive(true);
        }

        private void HideAll()
        {
            if (popupStart) popupStart.SetActive(false);
            if (popupInteract) popupInteract.SetActive(false);
            if (popupFire) popupFire.SetActive(false);
            if (popupResult) popupResult.SetActive(false);
            if (popupSummary) popupSummary.SetActive(false);
        }

        // ================= XỬ LÝ HIỆN POPUP TỪ EVENT =================
        private void ShowInteract(GameObject obj)
        {
            _currentItem = obj;
            popupInteract.SetActive(true);
            SetCursorState(true);
        }

        private void ShowFire(GameObject heldItem, GameObject fire)
        {
            popupFire.SetActive(true);
            SetCursorState(true);
        }
        
        private void ShowResult(bool success, string msg)
        {
            HideAll();
            popupResult.SetActive(true);
            txtResultTitle.text = success ? "THÀNH CÔNG!" : "THẤT BẠI!";
            txtResultTitle.color = success ? Color.green : Color.red;
            txtResultMessage.text = msg;
        }

        // ================= CÁC NÚT BẤM GỌI VÀO ĐÂY =================
        public void BtnClick_Start()
        {
            AudioManager.Instance?.PlayUIClick(); 
            popupStart.SetActive(false);
            SetCursorState(false);
        }

        public void BtnClick_PickUp()
        {
            AudioManager.Instance?.PlayUIClick(); 
            FireEvents.OnPickUpRequest?.Invoke(_currentItem); 
            ShieldMetallController.Instance?.ToggleMainKnob();
            popupInteract.SetActive(false);
            SetCursorState(false);
        }

        public void BtnClick_Ignore()
        {
            AudioManager.Instance?.PlayUIClick(); 
            _currentItem = null; 
            popupInteract.SetActive(false);
            SetCursorState(false);
        }

        public void BtnClick_UseFire()
        {
            AudioManager.Instance?.PlayUIClick(); 
            FireEvents.OnUseObjectOnFire?.Invoke(); 
            popupFire.SetActive(false);
        }

        public void BtnClick_CancelFire()
        {
            AudioManager.Instance?.PlayUIClick();
            popupFire.SetActive(false);
            SetCursorState(false);
        }

        public void BtnClick_Continue()
        {
            AudioManager.Instance?.PlayUIClick();
            popupResult.SetActive(false); 
            popupSummary.SetActive(true);
            SetCursorState(true);
        }
        
        // Nút Về Menu Chính
        public void BtnClick_BackMenu() 
        { 
            AudioManager.Instance?.PlayUIClick();
            if (SceneTransitionManager.Instance != null)
                SceneTransitionManager.Instance.LoadScene("SceneMenu"); 
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene("SceneMenu");
        }

        // Nút Đi tới tình huống tiếp theo
        public void BtnClick_NextScene()
        {
            AudioManager.Instance?.PlayUIClick();
            HideAll(); // Đừng quên hàm giấu UI đi nhé (nếu file cậu đang thiếu thì giữ lại HideAll() của cậu)

            // 1. Lấy vị trí (Index) của màn hình hiện tại
            int currentIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;

            // 2. Kiểm tra xem đã đến màn cuối cùng chưa (chống lỗi văng game)
            if (nextIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                // Mẹo: Lấy đường dẫn của Scene bằng Index, rồi trích xuất ra Tên Scene để đẩy cho TransitManager
                string nextScenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(nextIndex);
                string nextName = System.IO.Path.GetFileNameWithoutExtension(nextScenePath);

                if (SceneTransitionManager.Instance != null)
                    SceneTransitionManager.Instance.LoadScene(nextName);
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene(nextIndex);
            }
            else
            {
                Debug.LogWarning("Đây là màn cuối cùng rồi! Đưa người chơi về Menu.");
                // 3. Nếu không còn màn nào nữa, cho tự động về Menu (Index 0)
                if (SceneTransitionManager.Instance != null)
                    SceneTransitionManager.Instance.LoadScene("SceneMenu");
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
        
        private void SetCursorState(bool isUIVisible)
        {
            Cursor.lockState = isUIVisible ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isUIVisible;
        }
    }
}