using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startPanel;   // ��ʼ����
    public GameObject gameOverPanel; // ��Ϸʧ�ܽ���
    public GameObject victoryPanel;  // ʤ������
    public Text scoreText;           // �Ʒ��ı�

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
        scoreText.text = $"����: {score}";
    }
    public void LoadQUITScene()
    {
        SceneManager.LoadScene("ThreeOD"); // ���س���
    }

}
