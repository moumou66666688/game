using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // ����UI�����ռ�

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;
    public int totalObstacles = 10; // ���ϰ�������
    public bool gameStarted = false;
    private bool isGameOver = false; // ȷ����Ϸ�Ƿ����

    public UIManager uiManager;
    public PlayerController playerController;
    public Slider progressBar; // ����������

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        uiManager.ShowStartUI();
        progressBar.maxValue = totalObstacles; // ���ý����������ֵ
        progressBar.value = 0; // ��ʼֵΪ0
    }

    public void StartGame()
    {
        gameStarted = true;
        isGameOver = false; // ȷ����Ϸ���¿�ʼ
        uiManager.HideStartUI();
        playerController.EnableMovement(); // ������ҿ�ʼ�ƶ�
    }

    public void AddScore()
    {
        if (!gameStarted || isGameOver) return;

        score++;
        uiManager.UpdateScore(score, totalObstacles);

        // ���½�����
        progressBar.value = score; // ���ݷ������½�������ֵ

        if (score >= totalObstacles)
        {
            WinGame();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return; // �����ظ�����
        isGameOver = true;
        gameStarted = false;

        StopPlayer();
        uiManager.ShowGameOverUI();
    }

    public void WinGame()
    {
        if (isGameOver) return; // �����ظ�����
        isGameOver = true;
        gameStarted = false;

        StopPlayer();
        uiManager.ShowVictoryUI();
    }

    private void StopPlayer()
    {
        if (playerController != null)
        {
            playerController.DisableMovement(); // ֹͣ����ƶ�
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
    public void RestartGame()
    {
        Debug.Log("��Ϸ���¿�ʼ");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
