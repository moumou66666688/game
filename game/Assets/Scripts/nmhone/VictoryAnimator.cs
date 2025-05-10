// 新增必要的命名空间引用
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 关键修复！

public class VictoryAnimator : MonoBehaviour
{
    public Text titleText;
    public ParticleSystem particles;

    void Start()
    {
        // 安全校验
        if (titleText == null)
            titleText = GetComponentInChildren<Text>();

        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        // 初始状态
        titleText.color = new Color(1, 1, 1, 0);
        if (particles != null) particles.Stop();

        // 启动动画
        StartCoroutine(ShowAnimation());
    }

    // 明确声明返回类型为IEnumerator
    IEnumerator ShowAnimation()
    {
        // 文字淡入
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            titleText.color = Color.Lerp(
                Color.clear,
                Color.white,
                elapsed / duration
            );
            elapsed += Time.deltaTime;
            yield return null; // 每帧暂停
        }

        // 安全播放粒子
        if (particles != null)
        {
            particles.Play();
        }
        else
        {
            Debug.LogWarning("粒子系统引用丢失");
        }
    }
}