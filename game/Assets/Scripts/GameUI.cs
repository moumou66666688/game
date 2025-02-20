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
        frequencyText.text = "��ǰ����Ƶ��: " + StringController.Instance.GetCurrentFrequency().ToString("F2");
        resonanceText.text = "ֽ�˹���Ƶ��: " + PaperMan.Instance.resonanceFrequency.ToString("F2");
    }
}