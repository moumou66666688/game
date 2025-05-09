using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("基础参数")]
    public float scaleFactor = 2f;     // 放大倍数
    public float maxScale = 3f;        // 最大放大尺寸
    public float growSpeed = 0.5f;     // 放大速度
    public float shrinkSpeed = 0.5f;   // 恢复速度
    private Vector3 originalScale;
    private bool isGrowing = false;
    private bool isShrinking = false;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (isGrowing)
        {
            // 限制最大尺寸
            Vector3 targetScale = originalScale * scaleFactor;
            targetScale = Vector3.Min(targetScale, originalScale * maxScale);  // 限制最大值
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
            {
                isGrowing = false;
            }
        }
        else if (isShrinking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, shrinkSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                isShrinking = false;
            }
        }
    }

    public void GrowFire()
    {
        if (!isGrowing)
        {
            isGrowing = true;
            isShrinking = false;
        }
    }

    public void ShrinkFire()
    {
        if (!isShrinking)
        {
            isShrinking = true;
            isGrowing = false;
        }
    }
}
