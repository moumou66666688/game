using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("必须配置")]
    public int stringIndex;
    public CanvasGroup targetPanel;

    void Start()
    {
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(ShowPanel);
            Debug.Log($"按钮{stringIndex}初始化完成");
        }
        else
        {
            Debug.LogError("未找到Button组件！");
        }
    }

    void ShowPanel()
    {
        if (targetPanel == null)
        {
            Debug.LogError("目标面板未分配！");
            return;
        }

        targetPanel.alpha = 1;
        targetPanel.interactable = true;
        targetPanel.blocksRaycasts = true;
        Debug.Log($"显示面板：{targetPanel.name}");

        Rotator rotator = targetPanel.GetComponentInChildren<Rotator>(true);
        if (rotator != null)
        {
            rotator.Initialize(stringIndex);
        }
        else
        {
            Debug.LogError("未找到Rotator组件！");
        }
    }
}