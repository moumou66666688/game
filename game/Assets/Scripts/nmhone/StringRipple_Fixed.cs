// StringRipple_Fixed.cs
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class StringRipple_Fixed : MonoBehaviour
{
    [Header("波纹参数")]
    public Sprite rippleSprite;    // 直接拖入圆形Sprite
    public Color rippleColor = new Color(0.5f, 0.8f, 1, 0.8f);
    public float growSpeed = 4f;
    public float maxSize = 1.5f;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
    }

    private void OnMouseDown()
    {
        Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        StartCoroutine(RippleEffect(clickPoint));
    }

    IEnumerator RippleEffect(Vector2 center)
    {
        // 动态创建波纹对象
        GameObject rippleObj = new GameObject("Ripple");
        SpriteRenderer renderer = rippleObj.AddComponent<SpriteRenderer>();

        // 配置渲染参数
        renderer.sprite = rippleSprite;
        renderer.color = rippleColor;
        renderer.sortingOrder = 999;

        // 设置初始位置
        rippleObj.transform.position = center;
        float currentScale = 0.1f;

        // 扩散动画
        while (currentScale < maxSize)
        {
            currentScale += Time.deltaTime * growSpeed;
            rippleObj.transform.localScale = Vector3.one * currentScale;

            // 透明度衰减
            Color c = renderer.color;
            c.a = Mathf.Clamp01(1 - (currentScale / maxSize));
            renderer.color = c;

            yield return null;
        }
        Destroy(rippleObj);
    }
}