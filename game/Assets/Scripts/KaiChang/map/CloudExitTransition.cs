using System.Collections;
using UnityEngine;

public class CloudExitTransition : MonoBehaviour
{
    public RectTransform leftCloud;
    public RectTransform rightCloud;
    public float transitionTime = 1.5f; // 云朵散开的时间

    private Vector2 leftStartPos;
    private Vector2 rightStartPos;
    private Vector2 leftTargetPos;
    private Vector2 rightTargetPos;
    private float screenWidth;

    void Start()
    {
        if (leftCloud == null || rightCloud == null)
        {
            Debug.LogError("CloudExitTransition: 云朵未正确分配！");
            return;
        }

        // **获取屏幕宽度**
        screenWidth = Screen.width;

        // 记录初始位置（云朵从中间开始）
        leftStartPos = leftCloud.anchoredPosition;
        rightStartPos = rightCloud.anchoredPosition;

        // **计算目标位置（云朵完全散开）**
        leftTargetPos = new Vector2(-screenWidth*1.5f, leftStartPos.y);
        rightTargetPos = new Vector2(screenWidth*1.5f, rightStartPos.y);

        // **进入场景时自动播放动画**
        StartCoroutine(CloudMoveToEdges());
    }

    IEnumerator CloudMoveToEdges()
    {
        float elapsedTime = 0;

        // **云朵向两侧移动**
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);

            leftCloud.anchoredPosition = Vector2.Lerp(leftStartPos, leftTargetPos, t);
            rightCloud.anchoredPosition = Vector2.Lerp(rightStartPos, rightTargetPos, t);

            yield return null;
        }

        // 确保云朵完全散开
        leftCloud.anchoredPosition = leftTargetPos;
        rightCloud.anchoredPosition = rightTargetPos;
    }
}
