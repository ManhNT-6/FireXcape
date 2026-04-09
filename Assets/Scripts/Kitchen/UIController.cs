using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Instruction UI")]
    public GameObject instructionPanel;

    [Header("Result UI")]
    public GameObject resultPanel;
    public TMP_Text resultTitle;
    public TMP_Text resultMessage;

    public void ShowInstruction()
    {
        instructionPanel.SetActive(true);
        resultPanel.SetActive(false);
    }

    public void HideInstruction()
    {
        instructionPanel.SetActive(false);
    }

    public void ShowResult(bool isSuccess, string message)
    {
        Debug.Log("---Check--- Show Result");
        resultPanel.SetActive(true);

        resultTitle.text = isSuccess ? "SUCCESS" : "FAIL";
        resultMessage.text = message;
    }
}