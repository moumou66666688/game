using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeODManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;
    public ThreeODData dialogueData;
    public Button dialogueButton;  // 对话按钮
    public Button sceneTransitionButton; // 切换场景按钮

    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;

    [Header("淡入持续时间")]
    public float fadeInTime = 1.5f;

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
        if (currentIndex >= dialogueData.dialogues.Count) return;

        ThreeODLine line = dialogueData.dialogues[currentIndex];

        if (currentIndex == 6)  // 在第7句对话时切换按钮显示
        {
            dialogueButton.gameObject.SetActive(false);
            sceneTransitionButton.gameObject.SetActive(true);
        }

        currentIndex++;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(FadeInText(line.content, line.speaker));
    }

    IEnumerator FadeInText(string content, string speaker)
    {
        dialogueText.supportRichText = true;
        nameText.supportRichText = true;

        // 开始时文本的透明度为0
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeInTime;
            alpha = Mathf.Clamp01(alpha);

            // 使用 HTML 格式设置文本颜色，逐渐变为不透明
            Color fadeColor = new Color(0f, 0f, 0f, alpha);
            string hexColor = ColorUtility.ToHtmlStringRGBA(fadeColor);

            // 逐渐设置文本的颜色
            dialogueText.text = $"<color=#{hexColor}>{content}</color>";
            nameText.text = $"<color=#{hexColor}>{speaker}</color>";

            yield return null;
        }

        // 最终设置为完全不透明
        string fullHex = ColorUtility.ToHtmlStringRGBA(new Color(0f, 0f, 0f, 1f));
        dialogueText.text = $"<color=#{fullHex}>{content}</color>";
        nameText.text = $"<color=#{fullHex}>{speaker}</color>";
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
