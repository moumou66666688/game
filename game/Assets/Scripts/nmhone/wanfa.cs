using UnityEngine;
using UnityEngine.UI;
//显示panel
public class wanfa : MonoBehaviour
{
    public GameObject panel1;

    // 初始化时注册按钮事件
    void Start()
    {

    }

    public void ShowPanel1()
    {
        if (panel1 != null)
        {
            panel1.SetActive(true); // 激活 panel1
        }
    }
}