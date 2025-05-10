using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TiaoGuoCloud : MonoBehaviour
{
    public RectTransform leftCloud;
    public RectTransform rightCloud;
    public float transitionTime = 1.5f; // �ƶ��ƶ����м��ʱ��
    public float waitTime = 0.3f; // ͣ��ʱ��
    public Button transitionButton;

    private Vector2 leftStartPos;
    private Vector2 rightStartPos;
    private Vector2 leftTargetPos;
    private Vector2 rightTargetPos;
    private float screenWidth;

    void Start()
    {
        if (leftCloud == null || rightCloud == null)
        {
            Debug.LogError("CloudTransition: �ƶ�δ��ȷ���䣡");
            return;
        }

        // **��ȡ��Ļ���**
        screenWidth = Screen.width;

        // ��¼��ʼλ��
        leftStartPos = leftCloud.anchoredPosition;
        rightStartPos = rightCloud.anchoredPosition;

        // **����Ŀ��λ�ã���ȫ�ڵ���Ļ��**
        leftTargetPos = new Vector2(-screenWidth / 2 + 960, leftStartPos.y);
        rightTargetPos = new Vector2(screenWidth / 2 - 960, rightStartPos.y);

        if (transitionButton != null)
        {
            transitionButton.gameObject.SetActive(true);
            transitionButton.onClick.AddListener(StartTransition);
        }
    }

    public void StartTransition()
    {
        if (leftCloud == null || rightCloud == null)
        {
            Debug.LogError("StartTransition: �ƶ�δ��ȷ��ֵ��");
            return;
        }
        StartCoroutine(CloudMoveToCenter());
    }

    IEnumerator CloudMoveToCenter()
    {
        float elapsedTime = 0;

        // **�ƶ����м��ƶ����ص�����ס������Ļ**
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);

            leftCloud.anchoredPosition = Vector2.Lerp(leftStartPos, leftTargetPos, t);
            rightCloud.anchoredPosition = Vector2.Lerp(rightStartPos, rightTargetPos, t);

            yield return null;
        }

        // ȷ���ƶ���ȫ������Ļ
        leftCloud.anchoredPosition = leftTargetPos;
        rightCloud.anchoredPosition = rightTargetPos;

        // ͣ��һ��ʱ��
        //yield return new WaitForSeconds(waitTime);

        // �л�����
        SceneManager.LoadScene("Map");
    }
}
