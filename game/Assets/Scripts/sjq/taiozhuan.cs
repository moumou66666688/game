using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理命名空间

public class taiozhuan : MonoBehaviour
{
    // 这个方法会在按钮点击时被调用
    public void LoadSceneTwo()
    {
        // 加载名为"threesjq"的场景
        SceneManager.LoadScene("threesjq");
    }
}