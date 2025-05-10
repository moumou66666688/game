using System.Collections;
using UnityEngine;

public class CloudExitTransition : MonoBehaviour
{
    public RectTransform leftCloud;
    public RectTransform rightCloud;
    public float transitionTime = 1.5f; // �ƶ�ɢ����ʱ��

    private Vector2 leftStartPos;
    private Vector2 rightStartPos;
    private Vector2 leftTargetPos;
    private Vector2 rightTargetPos;
    private float screenWidth;

    void Start()
    {
        if (leftCloud == null || rightCloud == null)
        {
            Debug.LogError("CloudExitTransition: �ƶ�δ��ȷ���䣡");
            return;
        }

        // **��ȡ��Ļ���**
        screenWidth = Screen.width;

        // ��¼��ʼλ�ã��ƶ���м俪ʼ��
        leftStartPos = leftCloud.anchoredPosition;
        rightStartPos = rightCloud.anchoredPosition;

        // **����Ŀ��λ�ã��ƶ���ȫɢ����**
        leftTargetPos = new Vector2(-screenWidth*1.5f, leftStartPos.y);
        rightTargetPos = new Vector2(screenWidth*1.5f, rightStartPos.y);

        // **���볡��ʱ�Զ����Ŷ���**
        StartCoroutine(CloudMoveToEdges());
    }

    IEnumerator CloudMoveToEdges()
    {
        float elapsedTime = 0;

        // **�ƶ��������ƶ�**
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);

            leftCloud.anchoredPosition = Vector2.Lerp(leftStartPos, leftTargetPos, t);
            rightCloud.anchoredPosition = Vector2.Lerp(rightStartPos, rightTargetPos, t);

            yield return null;
        }

        // ȷ���ƶ���ȫɢ��
        leftCloud.anchoredPosition = leftTargetPos;
        rightCloud.anchoredPosition = rightTargetPos;
    }
}
