using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoDManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;
    public TwoDData dialogueData;
    public Button dialogueButton;  // 对话按钮
    public Button sceneTransitionButton; // 切换场景按钮

    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;

    public float typingSpeed = 0.1f;

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
        if (isDialogueActive) return;  // **避免重复调用**
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
        if (!isDialogueActive) return;  // 确保对话还在进行

        // **确保 currentIndex 在范围内**
        if (currentIndex >= dialogueData.dialogues.Count)
        {
            return;
        }

        TwoDLine line = dialogueData.dialogues[currentIndex];
        nameText.text = line.speaker;

        // **如果是第三句话（索引2），切换按钮**
        if (currentIndex == 5)
        {
            dialogueButton.gameObject.SetActive(false);   // 隐藏对话按钮
            sceneTransitionButton.gameObject.SetActive(true);  // 显示场景切换按钮
        }

        currentIndex++;  // **索引递增**

        // 停止上一次的打字效果
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(line.content));
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

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        dialogueButton.gameObject.SetActive(false);
        dialogueButton.interactable = false;
        dialogueButton.onClick.RemoveAllListeners();  // **解绑点击事件**
        sceneTransitionButton.gameObject.SetActive(true);
    }



}
