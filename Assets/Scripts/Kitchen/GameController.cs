using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public UIController ui;

    private bool isGameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 0f;
        ui.ShowInstruction();
    }

    public void StartTraining()
    {
        Debug.Log("---Manh--- Start training");
        Time.timeScale = 1f;
        ui.HideInstruction();
    }

    public void Success(string message)
    {
        if (isGameEnded) return;

        isGameEnded = true;
        Time.timeScale = 0f;

        ui.ShowResult(true, message);
    }

    public void Fail(string message)
    {
        Debug.Log("---Manh--- Check Fail");
        if (isGameEnded) return;
        
        Debug.Log("---Manh--- Show Popup result");
        isGameEnded = true;
        //Time.timeScale = 0f;

        ui.ShowResult(false, message);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}