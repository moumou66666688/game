using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("重新开始游戏，回到开始界面");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
