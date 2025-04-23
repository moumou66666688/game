// StringRipple_Fixed.cs
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class StringRipple_Fixed : MonoBehaviour
{
    [Header("���Ʋ���")]
    public Sprite rippleSprite;    // ֱ������Բ��Sprite
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
        // ��̬�������ƶ���
        GameObject rippleObj = new GameObject("Ripple");
        SpriteRenderer renderer = rippleObj.AddComponent<SpriteRenderer>();

        // ������Ⱦ����
        renderer.sprite = rippleSprite;
        renderer.color = rippleColor;
        renderer.sortingOrder = 999;

        // ���ó�ʼλ��
        rippleObj.transform.position = center;
        float currentScale = 0.1f;

        // ��ɢ����
        while (currentScale < maxSize)
        {
            currentScale += Time.deltaTime * growSpeed;
            rippleObj.transform.localScale = Vector3.one * currentScale;

            // ͸����˥��
            Color c = renderer.color;
            c.a = Mathf.Clamp01(1 - (currentScale / maxSize));
            renderer.color = c;

            yield return null;
        }
        Destroy(rippleObj);
    }
}