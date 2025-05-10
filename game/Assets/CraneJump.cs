using UnityEngine;

public class CraneJump : MonoBehaviour
{
    [Header("��Ծ����")]
    public float jumpHeight = 1f; // ��Ծ�߶�
    public float jumpSpeed = 2f;  // ��Ծ�ٶ�

    private Vector3 originalPos;
    private bool isJumping = false;

    void Start()
    {
        originalPos = transform.position; // ��¼��ʼλ��
    }

    public void Jump()
    {
        if (!isJumping) StartCoroutine(JumpRoutine());
    }

    System.Collections.IEnumerator JumpRoutine()
    {
        isJumping = true;
        float progress = 0;

        // ������Ծ
        Vector3 targetPos = originalPos + Vector3.up * jumpHeight;
        while (progress < 1)
        {
            transform.position = Vector3.Lerp(originalPos, targetPos, progress);
            progress += Time.deltaTime * jumpSpeed;
            yield return null;
        }

        // ����ԭλ
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