using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoTrMap : MonoBehaviour
{
    public void LoadSceneMap()
    {
        SceneManager.LoadScene("TwoOD");  // 确保 "Map" 场景名称正确
    }
}
