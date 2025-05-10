// ������Ҫ�������ռ�����
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // �ؼ��޸���

public class VictoryAnimator : MonoBehaviour
{
    public Text titleText;
    public ParticleSystem particles;

    void Start()
    {
        // ��ȫУ��
        if (titleText == null)
            titleText = GetComponentInChildren<Text>();

        if (particles == null)
            particles = GetComponentInChildren<ParticleSystem>();

        // ��ʼ״̬
        titleText.color = new Color(1, 1, 1, 0);
        if (particles != null) particles.Stop();

        // ��������
        StartCoroutine(ShowAnimation());
    }

    // ��ȷ������������ΪIEnumerator
    IEnumerator ShowAnimation()
    {
        // ���ֵ���
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
            yield return null; // ÿ֡��ͣ
        }

        // ��ȫ��������
        if (particles != null)
        {
            particles.Play();
        }
        else
        {
            Debug.LogWarning("����ϵͳ���ö�ʧ");
        }
    }
}