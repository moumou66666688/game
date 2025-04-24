using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//提示panel
public class TiShi : MonoBehaviour
{
    public Text dialogueText;
    public Button startButton;

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public float typingSpeed = 0.15f;
    public float lineDelay = 1.0f;

    private void Start()
    {
        startButton.onClick.AddListener(StartDialogue);
    }

    void StartDialogue()
    {
        startButton.gameObject.SetActive(false);
        StartCoroutine(PlayDialogueSequence());
    }

    IEnumerator PlayDialogueSequence()
    {
        int maxLine = 4; // ✅ 设置最多播放几句

        for (int i = 0; i < Mathf.Min(dialogueLines.Length, maxLine); i++)
        {
            yield return StartCoroutine(TypeSentence(dialogueLines[i]));

            if (i < maxLine - 1)
            {
                yield return new WaitForSeconds(lineDelay);
            }
        }

        
    }



    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
