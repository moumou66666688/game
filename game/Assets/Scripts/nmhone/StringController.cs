using UnityEngine;
using System.Collections;

public class StringController : MonoBehaviour
{
    [Header("��������")]
    [Tooltip("��Ӧ��������(0-6)")]
    public int stringIndex;

    [Tooltip("��Ӧ��ǧֽ��Transform")]
    public Transform craneTransform;

    [Header("��Ծ����")]
    [Tooltip("��Ծ�߶�")]
    public float jumpHeight = 50f;
    [Tooltip("��Ծ����ʱ��")]
    public float jumpDuration = 0.5f;

    private Vector3 originalPosition; // ԭʼλ��
    private bool isJumping = false;   // ��Ծ״̬

    void Start()
    {
        // ��¼��ʼλ��
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

        // �����׶�
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

        // �½��׶�
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