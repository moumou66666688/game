// VictoryPanelController.cs
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanelController : MonoBehaviour
{
    [Header("�Ӿ�Ԫ��")]
    public Text titleText;
    public Image celebrationEffect;

    void Start()
    {
        // ��ʼ����
        celebrationEffect.gameObject.SetActive(false);
        titleText.color = new Color(1, 1, 1, 0);

        // ������ʾ
        StartCoroutine(ShowAnimation());
    }

    System.Collections.IEnumerator ShowAnimation()
    {
        // ���⵭��
        float duration = 1.5f;
        float timer = 0;
        while (timer < duration)
        {
            titleText.color = Color.Lerp(Color.clear, Color.white, timer / duration);
            timer += Time.unscaledDeltaTime; // ����timescaleӰ��
            yield return null;
        }

        // ������Ч
        celebrationEffect.gameObject.SetActive(true);
    }
}
