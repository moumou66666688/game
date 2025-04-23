using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BookAnimation : MonoBehaviour
{
    public Image bookImage;  // �鼮 UI
    public Image[] items;    // С����б�
    public Image fadeImage;  // ��ɫ����

    void Start()
    {
        // ȷ�������ʼ��͸����
        fadeImage.color = new Color(0, 0, 0, 0);

        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        // 1. �鼮�Ӵ��С������
        bookImage.rectTransform.localScale = new Vector3(3f, 3f, 1f); // ��ʼ�Ŵ�
        bookImage.color = new Color(1, 1, 1, 0); // ��ʼ͸��

        bookImage.DOFade(1f, 1.5f);
        bookImage.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 1.5f);

        yield return new WaitForSeconds(2f); // �ȴ��鼮������� + ͣ�� 1s

        // 2. С������γ���
        for (int i = 0; i < items.Length; i++)
        {
            yield return new WaitForSeconds(1.0f); // ÿ����Ʒ��� 1s
            items[i].color = new Color(1, 1, 1, 1); // ֱ����ʾ
        }

        // 3. ����С�����ʾ��ͣ�� 3 ��
        yield return new WaitForSeconds(3f);

        // 4. ���������ɫ����
        fadeImage.DOFade(1f, 1f); // 1 ���ڵ�������ɫ

        yield return new WaitForSeconds(1.2f); // �ȴ�������ȫ���Ǻ��л�����

        // 5. �л�����
        SceneManager.LoadScene("SiNan");
    }
}
