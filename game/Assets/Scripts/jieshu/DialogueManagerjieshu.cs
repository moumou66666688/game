using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManagerjieshu : MonoBehaviour
{
    public Text nameText;                      // 人名
    public Text dialogueText;                  // 对话文本
    public GameObject dialoguePanel;           // 对话 UI Panel（对话按钮等）
    public DialogueDatajieshu dialogueData;    // 对话数据
    public GameObject textPanel;               // 过渡信息面板（TextPanel）
    public GameObject warnPanel;               // ✅ 最后出现的 WarnPanel

    public Button dialogueButton;              // 对话按钮
    public Button sceneTransitionButton;       // 切换按钮（第三句后显示）

    public Image leftCharacterImage;           // 左侧人物图像（沈括）
    public Image rightCharacterImage;          // 右侧人物图像
    public GameObject dialogueBox;             // 包含 nameText 和 dialogueText 的区域
    public Text finalTransitionText;           // 沈括介绍文本

    private int currentIndex = 0;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;

    [Header("淡入效果的持续时间")]
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

        finalTransitionText.gameObject.SetActive(false);

        if (textPanel != null)
            textPanel.SetActive(false);

        if (warnPanel != null)
            warnPanel.SetActive(false); // ✅ 初始隐藏 WarnPanel

        sceneTransitionButton.onClick.AddListener(OnSceneTransitionClicked);

        StartDialogue();
    }

    public void StartDialogue()
    {
        if (isDialogueActive) return;

        currentIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (!isDialogueActive || currentIndex >= dialogueData.dialogues.Count)
            return;

        DialogueLinejieshu line = dialogueData.dialogues[currentIndex];
        nameText.text = line.speaker;

        if (currentIndex == 8)
        {
            dialogueButton.gameObject.SetActive(false);
            sceneTransitionButton.gameObject.SetActive(true);
        }

        currentIndex++;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(FadeInText(line.content));
    }

    IEnumerator FadeInText(string content)
    {
        dialogueText.text = "";
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeInTime;
            alpha = Mathf.Clamp01(alpha);

            // 计算每个字的透明度变化
            Color fadeColor = new Color(0f, 0f, 0f, alpha);
            string hexColor = ColorUtility.ToHtmlStringRGBA(fadeColor);

            // 设置文本的颜色
            dialogueText.text = $"<color=#{hexColor}>{content}</color>";
            yield return null;
        }

        // 最终设为完全不透明
        string fullHex = ColorUtility.ToHtmlStringRGBA(new Color(0f, 0f, 0f, 1f));
        dialogueText.text = $"<color=#{fullHex}>{content}</color>";
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

    public void OnSceneTransitionClicked()
    {
        if (rightCharacterImage != null)
            rightCharacterImage.gameObject.SetActive(false);

        if (leftCharacterImage != null)
            StartCoroutine(ZoomInLeftCharacterAndShowFinalText());

        sceneTransitionButton.gameObject.SetActive(false);
    }

    IEnumerator ZoomInLeftCharacterAndShowFinalText()
    {
        Vector3 originalScale = leftCharacterImage.rectTransform.localScale;
        Vector3 targetScale = originalScale * 1.5f;
        float duration = 1.5f;
        float time = 0f;
        bool hasSwitchedUI = false;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            leftCharacterImage.rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);

            if (!hasSwitchedUI && t >= 0.2f)
            {
                hasSwitchedUI = true;

                if (dialogueBox != null)
                    dialogueBox.SetActive(false);
                if (dialogueText != null)
                    dialogueText.gameObject.SetActive(false);
                if (nameText != null)
                    nameText.gameObject.SetActive(false);

                if (dialoguePanel != null)
                    dialoguePanel.SetActive(false);
                if (textPanel != null)
                    textPanel.SetActive(true);

                if (finalTransitionText != null)
                {
                    finalTransitionText.gameObject.SetActive(true);
                    StartCoroutine(TypeFinalSentence(
                        "沈括（1031—1095），字存中，号梦溪丈人，北宋著名的科学家、政治家、文学家。他出生于浙江钱塘（今杭州），晚年隐居润州（今江苏镇江）梦溪园，并在此撰写了著名的笔记体著作《梦溪笔谈》。\n\n" +
                        "沈括博学多才，在天文、历法、数学、物理、化学、地理、生物、医学、工程技术等领域均有卓越贡献，被英国学者李约瑟誉为“中国整部科学史中最卓越的人物”。《梦溪笔谈》共30卷，内容涵盖自然科学、人文历史、社会风俗等，其中对活字印刷术、指南针用法、地质现象等的记载具有极高的科学史价值。\n\n" +
                        "沈括曾参与王安石变法，官至翰林学士、权三司使，后因政治变动遭贬谪。他晚年潜心著述，除《梦溪笔谈》外，还有《长兴集》《良方》等著作传世。其科学思想和实证精神对后世影响深远。"
                    ));
                }
            }

            yield return null;
        }
    }

    IEnumerator TypeFinalSentence(string sentence)
    {
        finalTransitionText.text = "";

        // 先逐字显示（透明状态）
        foreach (char letter in sentence)
        {
            finalTransitionText.text += $"<color=#00000000>{letter}</color>"; // 黑色但完全透明
        }

        // 然后整体淡入
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeInTime;
            alpha = Mathf.Clamp01(alpha);

            string visibleText = "";
            int visibleLength = Mathf.FloorToInt(alpha * sentence.Length);

            // 已显示部分应用当前alpha（黑色）
            Color fadeColor = new Color(0f, 0f, 0f, alpha);
            string hexColor = ColorUtility.ToHtmlStringRGBA(fadeColor);

            visibleText = $"<color=#{hexColor}>{sentence.Substring(0, visibleLength)}</color>";

            // 剩余部分保持透明
            if (visibleLength < sentence.Length)
            {
                visibleText += $"<color=#00000000>{sentence.Substring(visibleLength)}</color>";
            }

            finalTransitionText.text = visibleText;
            yield return null;
        }

        //// 最终显示（直接使用黑色文本）
        //finalTransitionText.text = sentence;

        //yield return new WaitForSeconds(5f);

        //if (textPanel != null)
        //    textPanel.SetActive(false);
        //if (finalTransitionText != null)
        //    finalTransitionText.gameObject.SetActive(false);

        //if (warnPanel != null)
        //    warnPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Over"); // 请确认 Over 场景已添加到 Build Settings 中

    }
}
