using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("��������")]
    public float scaleFactor = 2f;     // �Ŵ���
    public float maxScale = 3f;        // ���Ŵ�ߴ�
    public float growSpeed = 0.5f;     // �Ŵ��ٶ�
    public float shrinkSpeed = 0.5f;   // �ָ��ٶ�
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
            // �������ߴ�
            Vector3 targetScale = originalScale * scaleFactor;
            targetScale = Vector3.Min(targetScale, originalScale * maxScale);  // �������ֵ
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
