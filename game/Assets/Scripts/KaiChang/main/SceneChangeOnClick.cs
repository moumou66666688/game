using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 监听鼠标左键点击
        {
            SceneManager.LoadScene("Book"); // 加载名为 "sc1" 的场景
        }
    }
}
