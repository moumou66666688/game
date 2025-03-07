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

    public Slider volumeSlider;
    public AudioSource bgMusicAudioSource;
  
    private void Start()
    {
        TipsPanel.SetActive(false);

        Qbutton.onClick.AddListener(showTipsPanel);
        CloseButton.onClick.AddListener(closeTipPanel);
        ReturnButton.onClick.AddListener(Return);

        volumeSlider.value = bgMusicAudioSource.volume;

        // ���� Slider ��ֵ�仯
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
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
        Debug.Log("�л�����");
        SceneManager.LoadScene("end");
    }

    private void OnVolumeChanged(float newVolume)
    {
        bgMusicAudioSource.volume = newVolume;
    }

}
