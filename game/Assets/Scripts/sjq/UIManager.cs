using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;   // 开始界面
    public GameObject gameOverPanel; // 游戏失败界面
    public GameObject victoryPanel;  // 胜利界面
    public Text scoreText;           // 计分文本

    private void Start()
    {
        ShowStartUI();
    }

    public void ShowStartUI()
    {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void HideStartUI()
    {
        startPanel.SetActive(false);
    }
    public void HideAllUI()
    {
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }


    public void ShowGameOverUI()
    {
        gameOverPanel.SetActive(true);
    }

    public void ShowVictoryUI()
    {
        victoryPanel.SetActive(true);
    }

    public void UpdateScore(int score, int total)
    {
        scoreText.text = $"分数: {score}";
    }
    public void LoadQUITScene()
    {
        SceneManager.LoadScene("ThreeOD"); // 加载场景
    }

}
