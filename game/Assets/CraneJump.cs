using UnityEngine;

public class CraneJump : MonoBehaviour
{
    [Header("跳跃参数")]
    public float jumpHeight = 1f; // 跳跃高度
    public float jumpSpeed = 2f;  // 跳跃速度

    private Vector3 originalPos;
    private bool isJumping = false;

    void Start()
    {
        originalPos = transform.position; // 记录初始位置
    }

    public void Jump()
    {
        if (!isJumping) StartCoroutine(JumpRoutine());
    }

    System.Collections.IEnumerator JumpRoutine()
    {
        isJumping = true;
        float progress = 0;

        // 向上跳跃
        Vector3 targetPos = originalPos + Vector3.up * jumpHeight;
        while (progress < 1)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, progress);
            progress += Time.deltaTime * jumpSpeed;
            yield return null;
        }

        // 返回原位
        progress = 0;
        while (progress < 1)
        {
            transform.position = Vector3.Lerp(targetPos, originalPos, progress);
            progress += Time.deltaTime * jumpSpeed;
            yield return null;
        }

        isJumping = false;
    }
}