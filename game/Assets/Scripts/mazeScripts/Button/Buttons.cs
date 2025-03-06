using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public Button Qbutton;
    public GameObject TipsPanel;
    public Button CloseButton;
    public Button ReturnButton;

    private void Start()
    {
        TipsPanel.SetActive(false);

        Qbutton.onClick.AddListener(showTipsPanel);
        CloseButton.onClick.AddListener(closeTipPanel);
        ReturnButton.onClick.AddListener(Return);
    }
    void showTipsPanel()
    {
        TipsPanel.SetActive(true);
    }
    void closeTipPanel()
    {
        TipsPanel.SetActive(false);
    }
    void Return()
    {
        SceneManager.LoadScene(1);
    }

}
