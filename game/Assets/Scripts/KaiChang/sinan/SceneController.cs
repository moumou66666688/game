using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public float transitionTime = 4f; // 3�����ת

    void Start()
    {
        Invoke("LoadDialogueScene", transitionTime);
    }

    void LoadDialogueScene()
    {
        SceneManager.LoadScene("DuiHua"); // ȷ���Ի�����������ȷ
    }
}
