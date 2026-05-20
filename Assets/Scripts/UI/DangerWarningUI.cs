using System;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DangerWarningUI : MonoBehaviour
    {
        private TMP_Text _dangerWarningTxt;

        private void Start()
        {
            _dangerWarningTxt = GetComponent<TMP_Text>();
            _dangerWarningTxt.alpha = 0f;
        }

        private void OnEnable()
        {
            FireEvents.OnDangerWarning += UpdateUIDangerWarning;
        }

        private void OnDisable()
        {
            FireEvents.OnDangerWarning -= UpdateUIDangerWarning;
        }

        private void UpdateUIDangerWarning()
        {
            _dangerWarningTxt.alpha = 1f;
        }
    }
}