using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroSceneFade : MonoBehaviour
{
    public Image fadeImage;  // 黑色遮罩

    void Start()
    {
        // 进入场景后，淡出黑色遮罩
        fadeImage.DOFade(0, 1f);
    }
}
