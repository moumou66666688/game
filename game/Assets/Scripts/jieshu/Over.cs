using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Over : MonoBehaviour
{
    public Image bookImage;  // 书籍 UI
    
    //public Image fadeImage;  // 黑色遮罩

    void Start()
    {
        // 确保遮罩最开始是透明的
        //fadeImage.color = new Color(0, 0, 0, 0);

        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        // 1. 书籍从大变小并淡入
        bookImage.rectTransform.localScale = new Vector3(3f, 3f, 1f); // 初始放大
        bookImage.color = new Color(1, 1, 1, 0); // 初始透明

        bookImage.DOFade(1f, 1.5f);
        bookImage.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 1.5f);

        yield return new WaitForSeconds(20f); // 等待书籍动画完成 + 停滞 1s

       

       

        
    }
}
