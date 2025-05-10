using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Over : MonoBehaviour
{
    public Image bookImage;  // �鼮 UI
    
    //public Image fadeImage;  // ��ɫ����

    void Start()
    {
        // ȷ�������ʼ��͸����
        //fadeImage.color = new Color(0, 0, 0, 0);

        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        // 1. �鼮�Ӵ��С������
        bookImage.rectTransform.localScale = new Vector3(3f, 3f, 1f); // ��ʼ�Ŵ�
        bookImage.color = new Color(1, 1, 1, 0); // ��ʼ͸��

        bookImage.DOFade(1f, 1.5f);
        bookImage.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 1.5f);

        yield return new WaitForSeconds(20f); // �ȴ��鼮������� + ͣ�� 1s

       

       

        
    }
}
