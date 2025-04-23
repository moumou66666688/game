// 文件名：StringRipple.cs
using UnityEngine;
using System.Collections;

public class StringRipple : MonoBehaviour
{
    public GameObject ripplePrefab; // 拖入Hierarchy中的RippleEffect对象
    public float rippleSpeed = 3f;
    public float maxSize = 2f;

    private void Start()
    {
        // 初始化点击检测
        gameObject.AddComponent<BoxCollider2D>();
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnMouseDown()
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
            ripple.transform.localScale = new Vector3(currentSize, currentSize, 1);

            // 透明度衰减
            Color c = ripple.GetComponent<SpriteRenderer>().color;
            c.a = Mathf.Lerp(0.8f, 0f, currentSize / maxSize);
            ripple.GetComponent<SpriteRenderer>().color = c;

            yield return null;
        }
        Destroy(ripple);
    }
}