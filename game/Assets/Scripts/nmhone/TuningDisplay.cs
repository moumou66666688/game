using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public class TuningDisplayManual : MonoBehaviour
{
    [Header("���ڵ� (�� RectTransform)")]
    public RectTransform stringsRoot;              // �� StringsRoot
    public Font uiFont;                            // �����գ��ű������� Arial
    public GameObject scoreButton;                 // ����Ԥ�谴ť����Ĭ�����أ�

    [Header("���Ӿ�")]
    public int fontSize = 12;                      // ��ͨ���ֺ�
    public int rowGap = 2;                         // ��ͨ���м��
    public Color textColor = Color.black;

    readonly string[] names = { "��", "��", "��", "��", "��", "��", "��" };
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
            RectTransform rt = CreateRow(names[i] + "��");

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

            string tip = diff == 0 ? "<color=#D4AF37>��Ϊ׼ȷ��</color>"
                         : diff > 0 ? "<color=#6AAE9C>��Ҫ����Щ</color>"
                                    : "<color=#8C3B3B>��Ҫš��Щ</color>";

            rows[i].text = $"{names[i]}�� {diff:+0;-0;0}   {tip}";
            rows[i].fontSize = fontSize;
            rows[i].rectTransform.sizeDelta =
                new Vector2(rows[i].rectTransform.sizeDelta.x, lineH);
        }

        if (allCorrect)
        {
            string[] score = {
                "    5 ?6 ?5 ?3 ?|",
                "    2 ?�C4 ?�C?|",
                "    4 ?6 ?5? 7 ?|",
                "    7?�C?�C?�C?||"
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

            //  ��ʾ��ť
            if (scoreButton != null)
                scoreButton.SetActive(true);
        }
        else
        {
            SetRootHeight(7 * lineH + 6 * rowGap);

            // ���ذ�ť
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
