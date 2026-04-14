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

        // ================= XỬ LÝ HIỆN POPUP =================
        private void ShowInteract(GameObject obj)
        {
            if (obj.transform.parent != null && obj.transform.parent.name == "HandAnchor") 
            {
                return;
            }

            _currentItem = obj; 
            popupInteract.SetActive(true);
        }
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
        public void BtnClick_PickUp() 
        { 
            if (_currentItem != null)
            {
                FireEvents.OnPickUpRequest?.Invoke(_currentItem); 
                _currentItem = null; 
            }
            popupInteract.SetActive(false); 
        }
        public void BtnClick_Ignore() { _currentItem = null; popupInteract.SetActive(false); }
        public void BtnClick_UseFire() { FireEvents.OnUseObjectOnFire?.Invoke(); popupFire.SetActive(false); }
        public void BtnClick_CancelFire() { popupFire.SetActive(false); }
        public void BtnClick_Continue() { popupResult.SetActive(false); popupSummary.SetActive(true); }
        public void BtnClick_BackMenu() { /* Gọi script TransitManager hôm nọ ở đây */ }
    }
}