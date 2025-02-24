using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �����������ʱ����
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        collectManager.Instance.ResetAll();
    }

    // ���¿�ʼ��Ϸ
    public void RestartGame()
    {
        collectManager.Instance.ResetAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
