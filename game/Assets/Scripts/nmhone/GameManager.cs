using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // 新增引用

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
            Debug.Log("GameManager初始化完成");
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
            Debug.Log($"琴弦{i}: 正确={correct}, 初始={current}");
            stringDatas.Add(new StringData() { correctPitch = correct, currentPitch = current });
        }
    }
    // 修改GameManager.cs
    [Header("胜利面板")]
    public CanvasGroup victoryPanel;

    private bool isGameEnded; // 新增游戏状态标志

    public void CheckVictory()
    {
        if (isGameEnded) return; // 防止重复触发

        // 严格校验机制
        foreach (var data in stringDatas)
        {
            if (data.currentPitch != data.correctPitch)
            {
                Debug.Log($"琴弦{stringDatas.IndexOf(data)}未校准");
                return;
            }
        }

        // 胜利流程
        ShowVictoryPanel();
        EndGame();
    }

    private void ShowVictoryPanel()
    {
        victoryPanel.alpha = 1;
        victoryPanel.interactable = true;
        victoryPanel.blocksRaycasts = true;
        Debug.Log("胜利面板激活");
    }

    private void EndGame()
    {
        isGameEnded = true;
        Time.timeScale = 0;
        Debug.Log("游戏已结束");

        // 禁用所有交互
        var buttons = FindObjectsOfType<Button>();
        foreach (var btn in buttons) btn.interactable = false;
    }

    // 在UpdateCurrentPitch最后添加

    [Header("琴弦引用")]
    public List<GameObject> stringObjects = new List<GameObject>(7);
    // 在现有GameManager类中添加
    //[Header("特效设置")]
    //public List<GameObject> stringObjects; // 拖入所有琴弦对象

    public void UpdateCurrentPitch(int stringIndex, int newValue)
    {
        if (stringIndex < 0 || stringIndex >= stringDatas.Count) return;
        stringDatas[stringIndex].currentPitch = Mathf.Clamp(newValue, 0, 7);
        Debug.Log($"琴弦{stringIndex}更新为：{newValue}");
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
                    Debug.LogError($"琴弦{stringIndex}缺少StringEffect组件");
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