using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 引入UI命名空间

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;
    public int totalObstacles = 10; // 总障碍物数量
    public bool gameStarted = false;
    private bool isGameOver = false; // 确认游戏是否结束

    public UIManager uiManager;
    public PlayerController playerController;
    public Slider progressBar; // 进度条引用

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        uiManager.ShowStartUI();
        progressBar.maxValue = totalObstacles; // 设置进度条的最大值
        progressBar.value = 0; // 初始值为0
    }

    public void StartGame()
    {
        gameStarted = true;
        isGameOver = false; // 确认游戏重新开始
        uiManager.HideStartUI();
        playerController.EnableMovement(); // 允许玩家开始移动
    }

    public void AddScore()
    {
        if (!gameStarted || isGameOver) return;

        score++;
        uiManager.UpdateScore(score, totalObstacles);

        // 更新进度条
        progressBar.value = score; // 根据分数更新进度条的值

        if (score >= totalObstacles)
        {
            WinGame();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return; // 避免重复调用
        isGameOver = true;
        gameStarted = false;

        StopPlayer();
        uiManager.ShowGameOverUI();
    }

    public void WinGame()
    {
        if (isGameOver) return; // 避免重复调用
        isGameOver = true;
        gameStarted = false;

        StopPlayer();
        uiManager.ShowVictoryUI();
    }

    private void StopPlayer()
    {
        if (playerController != null)
        {
            playerController.DisableMovement(); // 停止玩家移动
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
    public void RestartGame()
    {
        Debug.Log("游戏重新开始");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
