using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text frequencyText;
    public Text resonanceText;

    void Update()
    {
        frequencyText.text = "当前琴弦频率: " + StringController.Instance.GetCurrentFrequency().ToString("F2");
        resonanceText.text = "纸人固有频率: " + PaperMan.Instance.resonanceFrequency.ToString("F2");
    }
}