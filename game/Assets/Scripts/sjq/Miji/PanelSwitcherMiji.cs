using UnityEngine;

public class PanelSwitcherMiji : MonoBehaviour
{
    [Tooltip("点击按钮后要显示的 Panel")]
    public GameObject targetPanel;

    public void ShowPanel()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("未设置 targetPanel！");
        }
    }
}
