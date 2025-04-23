using UnityEngine;
using UnityEngine.UI;

public class BGMUIManager : MonoBehaviour
{
    [Header("组件绑定")]
    public GameObject bgmPanel;         // BGM 设置面板
    public Slider volumeSlider;         // 滑块组件
    public AudioSource audioSource;     // 播放音乐的 AudioSource

    void Start()
    {
        // 初始化滑块默认值为当前音量
        volumeSlider.value = audioSource.volume;

        // 绑定滑动事件
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // 初始隐藏面板
        bgmPanel.SetActive(false);
    }

    public void TogglePanel()
    {
        // 打开/关闭面板
        bool isActive = bgmPanel.activeSelf;
        bgmPanel.SetActive(!isActive);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}
