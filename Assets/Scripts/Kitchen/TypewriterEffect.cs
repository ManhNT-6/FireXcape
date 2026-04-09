using System.Collections;
using TMPro;
using UnityEngine;

namespace Kitchen
{
    public class TypewriterEffect : MonoBehaviour
    {
        public TMP_Text textUI;
        [TextArea] public string fullText;

        public float typingSpeed = 0.03f;

        private Coroutine typingCoroutine;
        private bool isTyping = false;

        public void OnEnable()
        {
            StartTyping();
        }
        
        public void StartTyping()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeText());
        }

        private IEnumerator TypeText()
        {
            isTyping = true;
            textUI.text = "";

            for (int i = 0; i < fullText.Length; i++)
            {
                textUI.text += fullText[i];
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            isTyping = false;
        }

        public void SkipTyping()
        {
            if (!isTyping)
            {
                GameController.Instance.StartTraining();
                return;
            }

            StopCoroutine(typingCoroutine);
            textUI.text = fullText;
            isTyping = false;
        }
    }
}