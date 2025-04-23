using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // ��������

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [System.Serializable]
    public class StringData
    {
        public int correctPitch;
        public int currentPitch;
    }

    public List<StringData> stringDatas = new List<StringData>(7);
    public bool IsStringTuned(int index)
    {
        if (index < 0 || index >= stringDatas.Count) return false;
        return stringDatas[index].currentPitch == stringDatas[index].correctPitch;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager��ʼ�����");
            InitializeStrings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeStrings()
    {
        stringDatas.Clear();
        for (int i = 0; i < 7; i++)
        {
            int correct = Random.Range(0, 8);
            int current = Random.Range(0, 8);
            Debug.Log($"����{i}: ��ȷ={correct}, ��ʼ={current}");
            stringDatas.Add(new StringData() { correctPitch = correct, currentPitch = current });
        }
    }
    // �޸�GameManager.cs
    [Header("ʤ�����")]
    public CanvasGroup victoryPanel;

    private bool isGameEnded; // ������Ϸ״̬��־

    public void CheckVictory()
    {
        if (isGameEnded) return; // ��ֹ�ظ�����

        // �ϸ�У�����
        foreach (var data in stringDatas)
        {
            if (data.currentPitch != data.correctPitch)
            {
                Debug.Log($"����{stringDatas.IndexOf(data)}δУ׼");
                return;
            }
        }

        // ʤ������
        ShowVictoryPanel();
        EndGame();
    }

    private void ShowVictoryPanel()
    {
        victoryPanel.alpha = 1;
        victoryPanel.interactable = true;
        victoryPanel.blocksRaycasts = true;
        Debug.Log("ʤ����弤��");
    }

    private void EndGame()
    {
        isGameEnded = true;
        Time.timeScale = 0;
        Debug.Log("��Ϸ�ѽ���");

        // �������н���
        var buttons = FindObjectsOfType<Button>();
        foreach (var btn in buttons) btn.interactable = false;
    }

    // ��UpdateCurrentPitch������

    [Header("��������")]
    public List<GameObject> stringObjects = new List<GameObject>(7);
    // ������GameManager�������
    //[Header("��Ч����")]
    //public List<GameObject> stringObjects; // �����������Ҷ���

    public void UpdateCurrentPitch(int stringIndex, int newValue)
    {
        if (stringIndex < 0 || stringIndex >= stringDatas.Count) return;
        stringDatas[stringIndex].currentPitch = Mathf.Clamp(newValue, 0, 7);
        Debug.Log($"����{stringIndex}����Ϊ��{newValue}");
        if (stringDatas[stringIndex].currentPitch == stringDatas[stringIndex].correctPitch)
        {
            if (stringIndex < stringObjects.Count && stringObjects[stringIndex] != null)
            {
                StringEffect effect = stringObjects[stringIndex].GetComponent<StringEffect>();
                if (effect != null)
                {
                    effect.StartFlashing();
                }
                else
                {
                    Debug.LogError($"����{stringIndex}ȱ��StringEffect���");
                }
            }
        }
        //if (stringIndex < stringObjects.Count)
       // {
            //stringObjects[stringIndex].GetComponent<StringClickHandler>().enabled = true;
        //}
        CheckVictory();
    }
}