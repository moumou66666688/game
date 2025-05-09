using UnityEngine;
using System.Collections;

public class StringController : MonoBehaviour
{
    [Header("基本设置")]
    public int stringIndex;             // 0-6
    public Transform craneTransform;

    [Header("跳跃参数")]
    public float jumpHeight = 50f;
    public float jumpDuration = 0.5f;

    Vector3 originalPosition;
    bool isJumping;

    AudioSource audioSrc;               // ★ 缓存引用

    void Start()
    {
        if (craneTransform) originalPosition = craneTransform.position;
        audioSrc = GetComponent<AudioSource>();               // ★
    }

    void OnMouseDown()
    {
        // 播音：无条件或只在调准时播放，按需求挑选
        audioSrc?.Play();                                      // ★

        // 若只想调准时播放，可把上行挪进 if{} 里
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
