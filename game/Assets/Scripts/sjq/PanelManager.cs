using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject panel1; // 拖入 panel1 的引用

    // 初始化时注册按钮事件
    void Start()
    {
        // 如果你的按钮是通过代码获取的（可选）
        // Button button = GetComponent<Button>();
        // button.onClick.AddListener(ShowPanel1);
    }

    // 显示 panel1 的方法
    public void ShowPanel1()
    {
        if (panel1 != null)
        {
            panel1.SetActive(true); // 激活 panel1
        }
    }
}