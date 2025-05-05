using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TiaoGuoCloud : MonoBehaviour
{
    public RectTransform leftCloud;
    public RectTransform rightCloud;
    public float transitionTime = 1.5f; // 云朵移动到中间的时间
    public float waitTime = 0.3f; // 停留时间
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
            Debug.LogError("CloudTransition: 云朵未正确分配！");
            return;
        }

        // **获取屏幕宽度**
        screenWidth = Screen.width;

        // 记录初始位置
        leftStartPos = leftCloud.anchoredPosition;
        rightStartPos = rightCloud.anchoredPosition;

        // **计算目标位置（完全遮挡屏幕）**
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
            Debug.LogError("StartTransition: 云朵未正确赋值！");
            return;
        }
        StartCoroutine(CloudMoveToCenter());
    }

    IEnumerator CloudMoveToCenter()
    {
        float elapsedTime = 0;

        // **云朵向中间移动并重叠，遮住整个屏幕**
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);

            leftCloud.anchoredPosition = Vector2.Lerp(leftStartPos, leftTargetPos, t);
            rightCloud.anchoredPosition = Vector2.Lerp(rightStartPos, rightTargetPos, t);

            yield return null;
        }

        // 确保云朵完全覆盖屏幕
        leftCloud.anchoredPosition = leftTargetPos;
        rightCloud.anchoredPosition = rightTargetPos;

        // 停留一段时间
        //yield return new WaitForSeconds(waitTime);

        // 切换场景
        SceneManager.LoadScene("Map");
    }
}
