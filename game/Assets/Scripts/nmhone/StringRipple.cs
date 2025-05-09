using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]          // 方便提醒
public class StringRipple : MonoBehaviour
{
    [Header("波纹设置")]
    public GameObject ripplePrefab;       // 拖 RippleEffect 预制
    public float rippleSpeed = 3f;
    public float maxSize = 2f;

    void Awake()
    {
        /* 若编辑模式下已手动添加 Collider，则不再重复添加 */
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (!col)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
        }
        else
        {
            col.isTrigger = true;         // 确保保持触发器状态
        }
    }

    void OnMouseDown()
    {
        Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        StartCoroutine(CreateRipple(clickPos));
    }

    IEnumerator CreateRipple(Vector2 center)
    {
        GameObject ripple = Instantiate(ripplePrefab, center, Quaternion.identity);
        float currentSize = 0.1f;

        while (currentSize < maxSize)
        {
            currentSize += Time.deltaTime * rippleSpeed;
            ripple.transform.localScale = new Vector3(currentSize, currentSize, 1f);

            // 透明度衰减
            SpriteRenderer sr = ripple.GetComponent<SpriteRenderer>();
            Color c = sr.color;
            c.a = Mathf.Lerp(0.8f, 0f, currentSize / maxSize);
            sr.color = c;

            yield return null;
        }
        Destroy(ripple);
    }
}
