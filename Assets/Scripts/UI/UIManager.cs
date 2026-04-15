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

        [Header("Chuyển Cảnh (Dành cho Dev setup)")]
        [Tooltip("Gõ tên của Scene tiếp theo vào đây (VD: SceneOffice)")]
        public string nextSceneName = "";

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
        private void ShowInteract(GameObject obj) { _currentItem = obj; popupInteract.SetActive(true); }
        private void ShowFire(GameObject heldItem, GameObject fire) { popupFire.SetActive(true); }
        private void ShowResult(bool success, string msg)
        {
            HideAll();
            popupResult.SetActive(true);
            txtResultTitle.text = success ? "THÀNH CÔNG!" : "THẤT BẠI!";
            txtResultTitle.color = success ? Color.green : Color.red;
            txtResultMessage.text = msg;
        }

        // ================= CÁC NÚT BẤM GỌI VÀO ĐÂY =================
        public void BtnClick_Start() { popupStart.SetActive(false); }
        
        public void BtnClick_PickUp() { FireEvents.OnPickUpRequest?.Invoke(_currentItem); popupInteract.SetActive(false); }
        
        public void BtnClick_Ignore() { _currentItem = null; popupInteract.SetActive(false); }
        
        public void BtnClick_UseFire() { FireEvents.OnUseObjectOnFire?.Invoke(); popupFire.SetActive(false); }
        
        public void BtnClick_CancelFire() { popupFire.SetActive(false); }
        
        public void BtnClick_Continue() { popupResult.SetActive(false); popupSummary.SetActive(true); }
        
        // Nút Về Menu Chính
        public void BtnClick_BackMenu() 
        { 
            if (SceneTransitionManager.Instance != null)
                SceneTransitionManager.Instance.LoadScene("SceneMenu"); 
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene("SceneMenu");
        }

        // Nút Đi tới tình huống tiếp theo
        public void BtnClick_NextScene()
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                if (SceneTransitionManager.Instance != null)
                    SceneTransitionManager.Instance.LoadScene(nextSceneName);
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("Chưa nhập tên Scene tiếp theo trong UIManager!");
            }
        }
    }
}