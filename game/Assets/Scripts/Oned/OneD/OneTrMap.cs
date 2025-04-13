using UnityEngine;
using UnityEngine.SceneManagement;

public class OneTrMap : MonoBehaviour
{
    public void LoadSceneMap()
    {
        SceneManager.LoadScene("OneOD");  // 确保 "Two" 场景名称正确
    }
}
