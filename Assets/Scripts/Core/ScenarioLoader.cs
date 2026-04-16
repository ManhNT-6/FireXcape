using System.Collections.Generic;
using Kitchen;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Core
{
    public class ScenarioLoader : MonoBehaviour
    {
        [Header("Data Source")]
        public List<ScenarioData> listData;

        [Header("UI Components")]
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI instructionText;

        public enum Language { English, Vietnamese }
        public Language language = Language.Vietnamese;
        
        private ScenarioData data;

        private void OnEnable()
        {
            LoadScenarioBySceneIndex();
            if (data != null)
            {
                UpdateUI();
            }
        }

        private void LoadScenarioBySceneIndex()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            int targetIndex = currentSceneIndex - 1;

            if (listData != null && targetIndex >= 0 && targetIndex < listData.Count)
            {
                data = listData[targetIndex];
            }
        }
        public void UpdateUI()
        {
            titleText.text = data.scenarioName;
            string content = data.instructionEN;
    
            var typewriter = instructionText.GetComponent<TypewriterEffect>();
            if (typewriter != null)
            {
                typewriter.SetupAndStart(content);
            }
            else if (instructionText != null)
            {
                instructionText.text = content;
            }
        }
    }
}