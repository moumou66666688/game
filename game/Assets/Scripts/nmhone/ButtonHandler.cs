using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("��������")]
    public int stringIndex;
    public CanvasGroup targetPanel;

    void Start()
    {
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(ShowPanel);
            Debug.Log($"��ť{stringIndex}��ʼ�����");
        }
        else
        {
            Debug.LogError("δ�ҵ�Button�����");
        }
    }

    void ShowPanel()
    {
        if (targetPanel == null)
        {
            Debug.LogError("Ŀ�����δ���䣡");
            return;
        }

        targetPanel.alpha = 1;
        targetPanel.interactable = true;
        targetPanel.blocksRaycasts = true;
        Debug.Log($"��ʾ��壺{targetPanel.name}");

        Rotator rotator = targetPanel.GetComponentInChildren<Rotator>(true);
        if (rotator != null)
        {
            rotator.Initialize(stringIndex);
        }
        else
        {
            Debug.LogError("δ�ҵ�Rotator�����");
        }
    }
}