using UnityEngine;
using UnityEngine.SceneManagement;

public class ThreeTrMap : MonoBehaviour
{
    public void LoadSceneMap()
    {
        SceneManager.LoadScene("ThreeOD");  // 确保 "Map" 场景名称正确
    }
}
