using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public float transitionTime = 4f; // 3秒后跳转

    void Start()
    {
        Invoke("LoadDialogueScene", transitionTime);
    }

    void LoadDialogueScene()
    {
        SceneManager.LoadScene("DuiHua"); // 确保对话场景名称正确
    }
}
