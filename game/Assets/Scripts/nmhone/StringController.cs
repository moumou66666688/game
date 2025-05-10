using UnityEngine;
using System.Collections;

public class StringController : MonoBehaviour
{
    [Header("��������")]
    public int stringIndex;             // 0-6
    public Transform craneTransform;

    [Header("��Ծ����")]
    public float jumpHeight = 50f;
    public float jumpDuration = 0.5f;

    Vector3 originalPosition;
    bool isJumping;

    AudioSource audioSrc;               // �� ��������

    void Start()
    {
        if (craneTransform) originalPosition = craneTransform.position;
        audioSrc = GetComponent<AudioSource>();               // ��
    }

    void OnMouseDown()
    {
        // ��������������ֻ�ڵ�׼ʱ���ţ���������ѡ
        audioSrc?.Play();                                      // ��

        // ��ֻ���׼ʱ���ţ��ɰ�����Ų�� if{} ��
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
        float half = jumpDuration / 2f;
        float t = 0;
        Vector3 start = craneTransform.position;
        Vector3 peak = start + Vector3.up * jumpHeight;

        while (t < half)
        {
            craneTransform.position = Vector3.Lerp(start, peak, t / half);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < half)
        {
            craneTransform.position = Vector3.Lerp(peak, originalPosition, t / half);
            t += Time.deltaTime;
            yield return null;
        }
        craneTransform.position = originalPosition;
        isJumping = false;
    }
}
