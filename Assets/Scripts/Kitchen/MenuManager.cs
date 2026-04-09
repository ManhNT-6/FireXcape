using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kitchen
{
    public class MenuManager : MonoBehaviour
    {
        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}
