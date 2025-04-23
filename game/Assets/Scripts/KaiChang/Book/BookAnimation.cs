using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BookAnimation : MonoBehaviour
{
    public Image bookImage;  // 书籍 UI
    public Image[] items;    // 小物件列表
    public Image fadeImage;  // 黑色遮罩

    void Start()
    {
        // 确保遮罩最开始是透明的
        fadeImage.color = new Color(0, 0, 0, 0);

        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        // 1. 书籍从大变小并淡入
        bookImage.rectTransform.localScale = new Vector3(3f, 3f, 1f); // 初始放大
        bookImage.color = new Color(1, 1, 1, 0); // 初始透明

        bookImage.DOFade(1f, 1.5f);
        bookImage.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 1.5f);

        yield return new WaitForSeconds(2f); // 等待书籍动画完成 + 停滞 1s

        // 2. 小物件依次出现
        for (int i = 0; i < items.Length; i++)
        {
            yield return new WaitForSeconds(1.0f); // 每个物品间隔 1s
            items[i].color = new Color(1, 1, 1, 1); // 直接显示
        }

        // 3. 所有小物件显示后，停滞 3 秒
        yield return new WaitForSeconds(3f);

        // 4. 启动淡入黑色遮罩
        fadeImage.DOFade(1f, 1f); // 1 秒内淡入至黑色

        yield return new WaitForSeconds(1.2f); // 等待遮罩完全覆盖后切换场景

        // 5. 切换场景
        SceneManager.LoadScene("SiNan");
    }
}
