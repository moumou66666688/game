using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroSceneFade : MonoBehaviour
{
    public Image fadeImage;  // ��ɫ����

    void Start()
    {
        // ���볡���󣬵�����ɫ����
        fadeImage.DOFade(0, 1f);
    }
}
