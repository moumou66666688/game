using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;
    public DialogueData dialogueData;
    public Button dialogueButton;  // 对话按钮
    public Button sceneTransitionButton; // 切换场景按钮

    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;

    public float typingSpeed = 0.05f; // 控制每个字开始淡入的间隔
    public float fadeInTime = 0.3f;   // 每个字淡入持续时间

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
        dialogueButton.gameObject.SetActive(true); // 初始显示对话按钮
        sceneTransitionButton.gameObject.SetActive(false); // 初始隐藏切换场景按钮

        StartDialogue();
    }

    public void StartDialogue()
    {
        if (isDialogueActive) return;  // 避免重复调用
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

        DialogueLine line = dialogueData.dialogues[currentIndex];
        nameText.text = line.speaker;

        // 切换按钮逻辑
        if (currentIndex == 7)
        {
            dialogueButton.gameObject.SetActive(false);
            sceneTransitionButton.gameObject.SetActive(true);
        }

        currentIndex++;

        // 停止之前的协程
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(line.content));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.supportRichText = true;
        nameText.supportRichText = true;

        string speakerName = nameText.text;

        float alpha = 0f;
        float fadeInDuration = fadeInTime;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeInDuration;
            alpha = Mathf.Clamp01(alpha);

            Color fadeColor = new Color(0f, 0f, 0f, alpha);
            string hexColor = ColorUtility.ToHtmlStringRGBA(fadeColor);

            // 整段对话文本淡入
            dialogueText.text = $"<color=#{hexColor}>{sentence}</color>";

            // 名字同步淡入
            nameText.text = $"<color=#{hexColor}>{speakerName}</color>";

            yield return null;
        }

        // 确保完全显示（100%不透明）
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
