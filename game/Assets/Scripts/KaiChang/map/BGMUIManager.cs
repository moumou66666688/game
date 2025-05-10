using UnityEngine;
using UnityEngine.UI;

public class BGMUIManager : MonoBehaviour
{
    [Header("�����")]
    public GameObject bgmPanel;         // BGM �������
    public Slider volumeSlider;         // �������
    public AudioSource audioSource;     // �������ֵ� AudioSource

    void Start()
    {
        // ��ʼ������Ĭ��ֵΪ��ǰ����
        volumeSlider.value = audioSource.volume;

        // �󶨻����¼�
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // ��ʼ�������
        bgmPanel.SetActive(false);
    }

    public void TogglePanel()
    {
        // ��/�ر����
        bool isActive = bgmPanel.activeSelf;
        bgmPanel.SetActive(!isActive);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}
