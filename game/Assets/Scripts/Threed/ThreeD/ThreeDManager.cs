using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeDManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;
    public ThreeDData dialogueData;
    public Button dialogueButton;  // 对话按钮
    public Button sceneTransitionButton; // 切换场景按钮

    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;

    [Header("淡入时间（秒）")]
    public float fadeInTime = 0.5f;  // 控制淡入速度

    void Start()
    {
        if (dialogueData == null)
        {
            Debug.LogError("⚠️ 对话数据为空，请检查绑定！");
            return;
        }

        if (dialogueData.dialogues == null || dialogueData.dialogues.Count == 0)
        {
            Debug.LogError("⚠️ 对话数据内容为空！");
            return;
        }

        dialoguePanel.SetActive(true);
        dialogueButton.gameObject.SetActive(true);
        sceneTransitionButton.gameObject.SetActive(false);

        StartDialogue();
    }

    public void StartDialogue()
    {
        if (isDialogueActive) return;
        if (dialogueData == null || dialogueData.dialogues.Count == 0)
        {
            Debug.LogError("⚠️ 无有效对话数据！");
            return;
        }

        currentIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (!isDialogueActive) return;

        if (currentIndex >= dialogueData.dialogues.Count)
        {
            return;
        }

        ThreeDLine line = dialogueData.dialogues[currentIndex];
        nameText.text = line.speaker;

        if (currentIndex == 3)
        {
            dialogueButton.gameObject.SetActive(false);
            sceneTransitionButton.gameObject.SetActive(true);
        }

        currentIndex++;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(line.content, line.speaker));
    }

    IEnumerator TypeSentence(string sentence, string speakerName)
    {
        dialogueText.supportRichText = true;
        nameText.supportRichText = true;

        float alpha = 0f;
        float fadeInDuration = fadeInTime;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeInDuration;
            alpha = Mathf.Clamp01(alpha);

            Color fadeColor = new Color(0f, 0f, 0f, alpha);
            string hexColor = ColorUtility.ToHtmlStringRGBA(fadeColor);

            dialogueText.text = $"<color=#{hexColor}>{sentence}</color>";
            nameText.text = $"<color=#{hexColor}>{speakerName}</color>";

            yield return null;
        }

        // 确保100%不透明显示
        string fullHex = ColorUtility.ToHtmlStringRGBA(new Color(0f, 0f, 0f, 1f));
        dialogueText.text = $"<color=#{fullHex}>{sentence}</color>";
        nameText.text = $"<color=#{fullHex}>{speakerName}</color>";
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        dialogueButton.gameObject.SetActive(false);
        dialogueButton.interactable = false;
        dialogueButton.onClick.RemoveAllListeners();
        sceneTransitionButton.gameObject.SetActive(true);
    }
}
