using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 挂在按钮上：点击后显示 targetPanel，
/// 并把 stringIndex 传进面板里的 Rotator
/// </summary>
[DisallowMultipleComponent]
public class ButtonHandler : MonoBehaviour
{
    [Header("必须配置")]
    [Tooltip("对应的琴弦序号 0-6")]
    public int stringIndex = 0;

    [Tooltip("要显示的目标面板 (带 CanvasGroup)")]
    public CanvasGroup targetPanel;

    // —— 缓存引用 —— //
    Button btn;
    Rotator rotator;      // 指向面板里唯一的 Rotator
    bool wired;        // 是否已绑定点击

    /*---------------- 生命周期 ----------------*/
    void Awake()
    {
        // 1. 缓存 Button；若缺失立即报错
        btn = GetComponent<Button>();
        if (!btn)
        {
            Debug.LogError($"{name} 缺少 Button 组件，ButtonHandler 失效");
            enabled = false;
            return;
        }

        // 2. 校验目标面板是否已分配
        if (!targetPanel)
        {
            Debug.LogError($"{name} 未分配 targetPanel！");
            enabled = false;
            return;
        }

        // 3. 找面板里的 Rotator（隐藏/不隐藏都能找到）
        rotator = targetPanel.GetComponentInChildren<Rotator>(true);
        if (!rotator)
            Debug.LogWarning($"{targetPanel.name} 内未找到 Rotator，点击只会显示面板");

        // 4. 只绑定一次监听
        if (!wired)
        {
            btn.onClick.AddListener(OnButtonClick);
            wired = true;
        }
    }

    /*---------------- 点击回调 ----------------*/
    void OnButtonClick()
    {
        // Panel 可见
        targetPanel.alpha = 1f;
        targetPanel.interactable = true;
        targetPanel.blocksRaycasts = true;

        // Panel 若被 SetActive(false) 过，也确保激活
        if (!targetPanel.gameObject.activeSelf)
            targetPanel.gameObject.SetActive(true);

        // 调 Rotator
        rotator?.Initialize(stringIndex);
    }

    /*---------------- 可选：统一隐藏 ----------------*/
    public void HidePanel()
    {
        if (!targetPanel) return;
        targetPanel.alpha = 0f;
        targetPanel.interactable = false;
        targetPanel.blocksRaycasts = false;
    }
}
