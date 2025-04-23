// VictoryPanelController.cs
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanelController : MonoBehaviour
{
    [Header("视觉元素")]
    public Text titleText;
    public Image celebrationEffect;

    void Start()
    {
        // 初始隐藏
        celebrationEffect.gameObject.SetActive(false);
        titleText.color = new Color(1, 1, 1, 0);

        // 渐变显示
        StartCoroutine(ShowAnimation());
    }

    System.Collections.IEnumerator ShowAnimation()
    {
        // 标题淡入
        float duration = 1.5f;
        float timer = 0;
        while (timer < duration)
        {
            titleText.color = Color.Lerp(Color.clear, Color.white, timer / duration);
            timer += Time.unscaledDeltaTime; // 不受timescale影响
            yield return null;
        }

        // 粒子特效
        celebrationEffect.gameObject.SetActive(true);
    }
}
