using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public class TuningDisplayManual : MonoBehaviour
{
    [Header("父节点 (空 RectTransform)")]
    public RectTransform stringsRoot;              // 拖 StringsRoot
    public Font uiFont;                            // 可留空，脚本用内置 Arial
    public GameObject scoreButton;                 // 拖入预设按钮对象（默认隐藏）

    [Header("行视觉")]
    public int fontSize = 12;                      // 普通行字号
    public int rowGap = 2;                         // 普通行行间距
    public Color textColor = Color.black;

    readonly string[] names = { "宫", "商", "角", "徵", "羽", "文", "武" };
    readonly List<Text> rows = new();
    const float refreshPeriod = .5f;

    void OnEnable()
    {
        if (rows.Count == 0) BuildRows();
        Refresh();
        InvokeRepeating(nameof(Refresh), refreshPeriod, refreshPeriod);
    }

    void OnDisable() => CancelInvoke(nameof(Refresh));

    void BuildRows()
    {
        if (!uiFont) uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

        float lineH = fontSize + 2f;
        float y = 0f;

        for (int i = 0; i < 7; i++)
        {
            RectTransform rt = CreateRow(names[i] + "：");

            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0, 1);

            rt.offsetMin = new Vector2(0, -lineH);
            rt.offsetMax = new Vector2(0, 0);
            rt.anchoredPosition = new Vector2(0, -y);

            y += lineH + rowGap;
        }
        SetRootHeight(y - rowGap);
    }

    RectTransform CreateRow(string init)
    {
        GameObject go = new GameObject("Row", typeof(RectTransform));
        go.transform.SetParent(stringsRoot, false);

        Text txt = go.AddComponent<Text>();
        txt.font = uiFont;
        txt.fontSize = fontSize;
        txt.color = textColor;
        txt.alignment = TextAnchor.UpperLeft;
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;
        txt.supportRichText = true;
        txt.text = init;

        rows.Add(txt);
        return txt.rectTransform;
    }

    void Refresh()
    {
        var gm = GameManager.Instance;
        if (!gm) return;

        bool allCorrect = true;
        float lineH = fontSize + 2f;

        for (int i = 0; i < 7; i++)
        {
            var d = gm.stringDatas[i];
            int diff = d.correctPitch - d.currentPitch;

            if (diff != 0) allCorrect = false;

            string tip = diff == 0 ? "<color=#D4AF37>已为准确音</color>"
                         : diff > 0 ? "<color=#6AAE9C>需要放松些</color>"
                                    : "<color=#8C3B3B>需要拧紧些</color>";

            rows[i].text = $"{names[i]}： {diff:+0;-0;0}   {tip}";
            rows[i].fontSize = fontSize;
            rows[i].rectTransform.sizeDelta =
                new Vector2(rows[i].rectTransform.sizeDelta.x, lineH);
        }

        if (allCorrect)
        {
            string[] score = {
                "    5 ?6 ?5 ?3 ?|",
                "    2 ?–4 ?–?|",
                "    4 ?6 ?5? 7 ?|",
                "    7?–?–?–?||"
            };

            for (int i = 0; i < 4; i++)
            {
                rows[i].fontSize = 28;
                rows[i].text = "<b><color=#4B3228>" + score[i] + "</color></b>";
                rows[i].rectTransform.sizeDelta =
                    new Vector2(rows[i].rectTransform.sizeDelta.x, 32);
            }

            for (int i = 4; i < 7; i++)
            {
                rows[i].text = "";
                rows[i].rectTransform.sizeDelta =
                    new Vector2(rows[i].rectTransform.sizeDelta.x, 0);
            }

            SetRootHeight(32 * 4 + rowGap * 3);

            //  显示按钮
            if (scoreButton != null)
                scoreButton.SetActive(true);
        }
        else
        {
            SetRootHeight(7 * lineH + 6 * rowGap);

            // 隐藏按钮
            if (scoreButton != null)
                scoreButton.SetActive(false);
        }
    }

    void SetRootHeight(float h)
    {
        stringsRoot.anchorMin = stringsRoot.anchorMax = new Vector2(0, 1);
        stringsRoot.pivot = new Vector2(0, 1);
        stringsRoot.sizeDelta = new Vector2(stringsRoot.sizeDelta.x, h);
    }
}
