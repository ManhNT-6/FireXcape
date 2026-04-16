using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "NewScenario", menuName = "FireXcape/Scenario Data")]
    public class ScenarioData : ScriptableObject
    {
        [Header("Thông tin cơ bản")]
        public string scenarioName;
        
        [Header("Hướng dẫn (English)")]
        [TextArea(3, 10)]
        public string instructionEN; 

        [Header("Cấu hình kỹ thuật")]
        public float timeLimit = 60f; // Thời gian tối đa cho scene này
        public GameObject firePrefab; // Prefab lửa tương ứng
    }
}