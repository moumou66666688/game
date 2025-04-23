using UnityEngine;
using System.Collections;

public class StringController : MonoBehaviour
{
    [Header("基本设置")]
    [Tooltip("对应琴弦索引(0-6)")]
    public int stringIndex;

    [Tooltip("对应的千纸鹤Transform")]
    public Transform craneTransform;

    [Header("跳跃参数")]
    [Tooltip("跳跃高度")]
    public float jumpHeight = 50f;
    [Tooltip("跳跃持续时间")]
    public float jumpDuration = 0.5f;

    private Vector3 originalPosition; // 原始位置
    private bool isJumping = false;   // 跳跃状态

    void Start()
    {
        // 记录初始位置
        if (craneTransform != null)
            originalPosition = craneTransform.position;
    }

    void OnMouseDown()
    {
        if (!isJumping &&
           GameManager.Instance.stringDatas[stringIndex].currentPitch ==
           GameManager.Instance.stringDatas[stringIndex].correctPitch)
        {
            StartCoroutine(JumpAnimation());
        }
    }

    IEnumerator JumpAnimation()
    {
        isJumping = true;
        float elapsed = 0f;
        Vector3 startPos = craneTransform.position;
        Vector3 peakPos = startPos + Vector3.up * jumpHeight;

        // 上升阶段
        while (elapsed < jumpDuration / 2)
        {
            craneTransform.position = Vector3.Lerp(
                startPos,
                peakPos,
                elapsed / (jumpDuration / 2)
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 下降阶段
        elapsed = 0f;
        while (elapsed < jumpDuration / 2)
        {
            craneTransform.position = Vector3.Lerp(
                peakPos,
                originalPosition,
                elapsed / (jumpDuration / 2)
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        craneTransform.position = originalPosition;
        isJumping = false;
    }
}